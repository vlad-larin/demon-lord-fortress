using System.Collections.Generic;
using System.Linq;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    internal class FinishEncounterDebriefingActionHandler
        : PlayerActionHandler<FinishEncounterDebriefingAction>
    {
        public FinishEncounterDebriefingActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(
            FinishEncounterDebriefingAction playerAction
        )
        {
            var gameEvents = new List<GameEventBase>
            {
                new SimpleGameEvent("The encounter is concluded"),
            };

            var encounter = GameInstance.Encounter;
            GameInstance.Encounter = null;

            // A one off fight has no climb to go back to, so it hands the player back to
            // the title screen the way it always did.
            if (GameInstance.Expedition == null)
            {
                GameInstance.GameMode = GameMode.Title;
                return new PlayerActionResult(GameInstance, gameEvents);
            }

            gameEvents.AddRange(ResolveExpeditionProgress(encounter));
            return new PlayerActionResult(GameInstance, gameEvents);
        }

        /// <summary>
        /// What the fight did to the expedition: who walks away from the room, whether the
        /// party has anywhere left to go, and where they go next.
        /// </summary>
        private IEnumerable<GameEventBase> ResolveExpeditionProgress(Encounter encounter)
        {
            var gameEvents = new List<GameEventBase>();
            var expedition = GameInstance.Expedition;
            var room = expedition.CurrentRoom;

            // Whoever is still standing at the end of the fight is who the tower and the
            // party carry on with, wounds and all.
            room.Guardians = encounter
                .Combatants.Where(combatant => combatant.Side == ConflictSide.DemonLord)
                .ToList();
            expedition.Heroes = encounter
                .Combatants.Where(combatant => combatant.Side == ConflictSide.Heroes)
                .ToList();
            expedition.CurrentRoom = null;

            if (encounter.Outcome.Winner != ConflictSide.Heroes)
            {
                gameEvents.Add(
                    new SimpleGameEvent($"The expedition ends in the {room.Type}. The tower holds")
                );
                return EndExpedition(gameEvents);
            }

            room.Cleared = true;
            gameEvents.Add(new SimpleGameEvent($"The {room.Type} is lost"));

            if (expedition.Objective.IsHeldBy(room))
            {
                gameEvents.Add(new SimpleGameEvent(expedition.Objective.Achievement));
                gameEvents.Add(
                    new SimpleGameEvent("The party got what it came for and leaves the tower")
                );
                return EndExpedition(gameEvents);
            }

            // Nothing above them can happen yet: the objective is always somewhere in the
            // tower, so a party that runs out of floors has already been sent home above.
            if (expedition.CurrentFloorNumber >= GameInstance.Tower.Floors.Count)
            {
                gameEvents.Add(
                    new SimpleGameEvent("The party has run out of tower and turns back")
                );
                return EndExpedition(gameEvents);
            }

            expedition.CurrentFloorNumber++;
            GameInstance.GameMode = GameMode.TowerClimbing;
            gameEvents.Add(
                new SimpleGameEvent($"The party climbs to floor {expedition.CurrentFloorNumber}")
            );
            return gameEvents;
        }

        private IEnumerable<GameEventBase> EndExpedition(List<GameEventBase> gameEvents)
        {
            GameInstance.Expedition = null;
            GameInstance.GameMode = GameMode.Title;
            return gameEvents;
        }
    }
}
