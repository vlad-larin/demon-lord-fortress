using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
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

                // Change intentions due to conditions

                // Retarget impossible targets
                if (intent.Target != null && !encounter.Combatants.Contains(intent.Target))
                {
                    encounter.HeroPartyStrategy.RetargetAction(intent);
                    gameEvents.Add(
                        new SimpleGameEvent(
                            $"Retargeted. {intent.Actor.Class}: {intent.Action.Name} -> {intent.Target.Class}"
                        )
                    );
                }

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

            GameInstance.Encounter.Phase = EncounterPhase.Resolution;

            return new PlayerActionResult(GameInstance, gameEvents);
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
    }
}
