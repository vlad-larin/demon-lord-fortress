using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Attributes;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    [SupportsGameMode(GameMode.Encounter)]
    internal class InsertCombatActionIntoBattlePlanActionHandler
        : PlayerActionHandler<InsertCombatActionIntoBattlePlan>
    {
        public InsertCombatActionIntoBattlePlanActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(
            InsertCombatActionIntoBattlePlan playerAction
        )
        {
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new SimpleGameEvent("Inserting combat action into the battle plan"));

            var actor = GameInstance.Encounter.Combatants.Single(c => c.Id == playerAction.ActorId);
            if (actor.Side != ConflictSide.DemonLord)
                throw new InvalidOperationException("Only demon lord actions can be inserted");

            var target = GameInstance.Encounter.Combatants.Single(c =>
                c.Id == playerAction.TargetId
            );

            var intentsToDelete = GameInstance.Encounter.Intents.FindAll(intent =>
                intent.Actor == actor
            );

            var newIntent = new CombatIntent
            {
                Actor = actor,
                Action = playerAction.Intent.Action,
                Target = target,
            };

            if (
                GameInstance.Encounter.Intents[playerAction.Index].Actor.Side
                == ConflictSide.DemonLord
            )
            {
                GameInstance.Encounter.Intents[playerAction.Index] = newIntent;
                gameEvents.Add(new SimpleGameEvent("Intent replaced"));
            }
            else
            {
                GameInstance.Encounter.Intents.Insert(playerAction.Index, newIntent);
                gameEvents.Add(new SimpleGameEvent("New intent inserted"));
            }

            foreach (var intent in intentsToDelete)
            {
                GameInstance.Encounter.Intents.Remove(intent);
                gameEvents.Add(new SimpleGameEvent("Old intent removed"));
            }

            return new PlayerActionResult(GameInstance, gameEvents);
        }
    }
}
