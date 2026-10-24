using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Extensions;
using GameCore.Models;
using GameCore.Models.CombatActions;
using GameCore.Models.Conditions.Abstractions;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Ai;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    internal class ExecuteBattlePlanActionHandler : PlayerActionHandler<ExecuteBattlePlanAction>
    {
        public ExecuteBattlePlanActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(ExecuteBattlePlanAction playerAction)
        {
            var encounter = GameInstance.Encounter;
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new SimpleGameEvent("Batlle plan execution has started"));

            for (var stepNumber = 1; stepNumber <= encounter.BattlePlan.Count; stepNumber++)
            {
                gameEvents.Add(new SimpleGameEvent($"Step {stepNumber}:"));
                gameEvents.AddRange(ResolveStep(encounter.BattlePlan[stepNumber - 1]));

                // The round stops the moment the encounter is decided, so nothing is swung
                // at a fight that is already over.
                var outcome = encounter.CheckEndConditions();
                if (outcome != null)
                {
                    encounter.Outcome = outcome;
                    break;
                }
            }
            gameEvents.AddRange(DecayConditions(encounter.Combatants));

            GameInstance.Encounter.Phase = EncounterPhase.Resolution;

            return new PlayerActionResult(GameInstance, gameEvents);
        }

        /// <summary>
        /// One clash, resolved as a single simultaneous exchange. Everyone in it acts on the
        /// board as it stood when the clash opened - the same conditions, the same living
        /// combatants - and only once every intent has been carried out do the conditions
        /// they inflicted come into force and the hit points that ran out count as death.
        /// The intents are walked in order because code has to run in some order; nothing
        /// inside a clash is meant to depend on that order.
        /// </summary>
        private IEnumerable<GameEventBase> ResolveStep(CombatStep step)
        {
            var encounter = GameInstance.Encounter;
            var gameEvents = new List<GameEventBase>();

            foreach (var intent in step.Intents)
            {
                // Only someone struck down in an earlier clash is out of this one.
                if (!encounter.Combatants.Contains(intent.Actor))
                {
                    gameEvents.Add(
                        new SimpleGameEvent(
                            $"Skipped {intent.Actor.DisplayName} because they are dead"
                        )
                    );
                    continue;
                }

                gameEvents.AddRange(UpdateIntentAccordingToConditions(intent));
                gameEvents.AddRange(HandleMissingTarget(intent));

                gameEvents.AddRange(intent.Action.Execute(intent.Actor, intent.Target, encounter));
                intent.IsExecuted = true;
            }

            foreach (var combatant in encounter.Combatants)
                combatant.SettleInflictedConditions();

            gameEvents.AddRange(CheckForDeadCombatants());

            return gameEvents;
        }

        private List<GameEventBase> UpdateIntentAccordingToConditions(CombatIntent intent)
        {
            var gameEvents = new List<GameEventBase>();
            foreach (var condition in intent.Actor.Conditions)
                gameEvents.AddRange(condition.UpdateIntentBeforeExecution(intent));

            // The actor's own conditions can change who they swing at, so whoever is standing
            // in the way is only known once that pass is done. Captured before the loop: a
            // condition here may point the intent somewhere else entirely.
            var target = intent.Target;
            if (target == null)
                return gameEvents;

            foreach (var condition in target.Conditions)
                gameEvents.AddRange(condition.UpdateIncomingIntentBeforeExecution(intent));

            return gameEvents;
        }

        private IEnumerable<GameEventBase> HandleMissingTarget(CombatIntent intent)
        {
            var gameEvents = new List<GameEventBase>();

            if (intent.Target == null) // No target => no retarget
                return gameEvents;

            var encounter = GameInstance.Encounter;
            if (encounter.Combatants.Contains(intent.Target)) // Valid target => no retarget
                return gameEvents;

            switch (intent.Actor.Side)
            {
                case ConflictSide.Heroes:
                    // Heroes re-target using their strategy
                    encounter.HeroPartyStrategy.RetargetAction(intent);
                    gameEvents.Add(
                        new SimpleGameEvent(
                            $"Retargeted. {intent.Actor.DisplayName}: {intent.Action.Name} -> {intent.Target.DisplayName}"
                        )
                    );
                    break;

                case ConflictSide.DemonLord:
                    // Monsters waste their action if their target is invalid
                    // because the player is supposed to think the plan through
                    intent.Action = new Wait();
                    intent.Target = null;
                    gameEvents.Add(
                        new SimpleGameEvent(
                            $"{intent.Actor.DisplayName} wastes their action because target became unavailable"
                        )
                    );
                    break;

                default:
                    throw new InvalidOperationException($"Unknown actor side: {intent.Actor.Side}");
            }

            return gameEvents;
        }

        private IEnumerable<GameEventBase> CheckForDeadCombatants()
        {
            var encounter = GameInstance.Encounter;
            var deadCombatants = encounter.Combatants.Where(c => c.Hp <= 0).ToList();
            foreach (var deadCombatant in deadCombatants)
            {
                encounter.Combatants.Remove(deadCombatant);
                encounter.DeadCombatants.Add(deadCombatant);
            }

            // A condition can hang on whoever granted it, and the fallen grant nothing any
            // more. Those lapse here rather than at the end of the round, so the clashes
            // still to come do not read a shield nobody is holding.
            foreach (var combatant in encounter.Combatants)
                combatant.Conditions.RemoveAll(c => c.ShouldBeRemoved());

            return deadCombatants.Select(c => new CombatantDiedGameEvent(c));
        }

        private IEnumerable<GameEventBase> DecayConditions(List<Combatant> combatants)
        {
            foreach (var combatant in combatants)
            {
                combatant
                    .Conditions.Where(c => c is TimedConditionBase)
                    .Cast<TimedConditionBase>()
                    .ToList()
                    .ForEach(c => c.Decay());

                combatant.Conditions.RemoveAll(c => c.ShouldBeRemoved());
            }
            return new GameEventBase[0];
        }
    }
}
