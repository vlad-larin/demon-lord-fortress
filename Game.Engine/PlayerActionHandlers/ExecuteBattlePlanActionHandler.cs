using System;
using System.Collections.Generic;
using System.Linq;
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

            foreach (var intent in encounter.Intents)
            {
                // Check if actor is still alive
                if (!encounter.Combatants.Contains(intent.Actor))
                {
                    gameEvents.Add(
                        new SimpleGameEvent($"Skipped {intent.Actor.Class} because they are dead")
                    );
                    continue;
                }

                gameEvents.AddRange(UpdateIntentAccordingToConditions(intent));
                gameEvents.AddRange(HandleMissingTarget(intent));

                // Execute the action
                var executionEvents = intent.Action.Execute(intent.Actor, intent.Target, encounter);
                gameEvents.AddRange(executionEvents);
                intent.IsExecuted = true;

                // Remove dead combatants
                var deathEvents = CheckForDeadCombatants();
                gameEvents.AddRange(deathEvents);

                // Check if any side has combatants alive
                if (
                    !encounter.Combatants.Any(c => c.Side == ConflictSide.Heroes)
                    || !encounter.Combatants.Any(c => c.Side == ConflictSide.DemonLord)
                )
                    break;
            }
            gameEvents.AddRange(DecayTimeBasedConditions(encounter.Combatants));

            GameInstance.Encounter.Phase = EncounterPhase.Resolution;

            return new PlayerActionResult(GameInstance, gameEvents);
        }

        private List<GameEventBase> UpdateIntentAccordingToConditions(CombatIntent intent)
        {
            var gameEvents = new List<GameEventBase>();
            foreach (var condition in intent.Actor.Conditions)
                gameEvents.AddRange(condition.UpdateIntentBeforeExecution(intent));
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
                            $"Retargeted. {intent.Actor.Class}: {intent.Action.Name} -> {intent.Target.Class}"
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
                            $"{intent.Actor.Class} wastes their action because target became unavailable"
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

            return deadCombatants.Select(c => new CombatantDiedGameEvent(c));
        }

        private IEnumerable<GameEventBase> DecayTimeBasedConditions(List<Combatant> combatants)
        {
            foreach (var combatant in combatants)
            {
                foreach (var condition in combatant.Conditions)
                {
                    if (condition is TimedConditionBase timedCondition)
                    {
                        timedCondition.Decay();
                    }
                }

                combatant.Conditions.RemoveAll(c =>
                    c is TimedConditionBase timedCondition && timedCondition.RoundsLeft <= 0
                );
            }
            return new CombatantDiedGameEvent[0];
        }
    }
}
