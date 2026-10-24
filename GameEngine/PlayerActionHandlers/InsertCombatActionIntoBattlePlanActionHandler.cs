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
    internal class InsertCombatActionIntoBattlePlanActionHandler
        : PlayerActionHandler<InsertCombatActionIntoBattlePlan>
    {
        public InsertCombatActionIntoBattlePlanActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(
            InsertCombatActionIntoBattlePlan playerAction
        )
        {
            var encounter = GameInstance.Encounter;
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new SimpleGameEvent("Inserting combat action into the battle plan"));

            var actor = encounter.Combatants.Single(c => c.Id == playerAction.ActorId);
            if (actor.Side != ConflictSide.DemonLord)
                throw new InvalidOperationException("Only demon lord actions can be inserted");

            var target = encounter.Combatants.Single(c => c.Id == playerAction.TargetId);

            var newIntent = new CombatIntent
            {
                Actor = actor,
                Action = playerAction.Intent.Action,
                Target = target,
            };

            PlaceIntent(playerAction, newIntent, gameEvents);

            // One monster, one action a round. Whatever this one was set to do before gives
            // way to the new order, and a clash nobody is left in is dropped.
            foreach (var step in encounter.BattlePlan)
                step.Intents.RemoveAll(intent => intent.Actor == actor && intent != newIntent);
            var abandonedSteps = encounter.BattlePlan.RemoveAll(step => step.Intents.Count == 0);
            if (abandonedSteps > 0)
                gameEvents.Add(new SimpleGameEvent("Old intent removed"));

            return new PlayerActionResult(GameInstance, gameEvents);
        }

        private void PlaceIntent(
            InsertCombatActionIntoBattlePlan playerAction,
            CombatIntent newIntent,
            List<GameEventBase> gameEvents
        )
        {
            var battlePlan = GameInstance.Encounter.BattlePlan;
            var stepIndex = playerAction.StepIndex;

            if (stepIndex < 0 || stepIndex > battlePlan.Count)
                throw new InvalidOperationException(
                    $"[InsertCombatActionIntoBattlePlanActionHandler] There is no step {stepIndex} in a plan of {battlePlan.Count}"
                );

            // Past the end of the plan is the one place a monster may open a clash of its
            // own, and it is always the last one. Anywhere earlier and the player could put
            // the whole warband in front of the party's first intent.
            if (stepIndex == battlePlan.Count)
            {
                battlePlan.Add(new CombatStep(newIntent));
                gameEvents.Add(new SimpleGameEvent("New step opened after the plan"));
                return;
            }

            // One monster to a clash, and the newcomer gets it. Whoever was there is left
            // without orders rather than refused, so changing your mind about a clash is one
            // decision instead of two.
            var occupant = battlePlan[stepIndex].MonsterIntent;
            if (occupant != null)
            {
                battlePlan[stepIndex].Intents.Remove(occupant);
                if (occupant.Actor != newIntent.Actor)
                    gameEvents.Add(
                        new SimpleGameEvent(
                            $"{occupant.Actor.DisplayName} is pulled out of step {stepIndex + 1} and awaits new orders"
                        )
                    );
            }

            battlePlan[stepIndex].Intents.Add(newIntent);
            gameEvents.Add(new SimpleGameEvent($"Joined step {stepIndex + 1}"));
        }
    }
}
