using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Ai;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    /// <summary>
    /// The step between two fights: the party looks over the floor it stands on, walks into
    /// one of its rooms, and that room's guardians become the encounter.
    /// </summary>
    internal class AdvanceExpeditionActionHandler : PlayerActionHandler<AdvanceExpeditionAction>
    {
        public AdvanceExpeditionActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(AdvanceExpeditionAction playerAction)
        {
            var expedition = GameInstance.Expedition;
            var choice = new ExpeditionAi(GameInstance).ChooseRoom();
            var room = choice.Room;

            // Deployment is not in yet, so a room without guardians cannot happen - and an
            // unopposed room is not an encounter, so there would be nothing to hand over to.
            if (room.Guardians.Count == 0)
                throw new NotImplementedException(
                    $"[AdvanceExpeditionActionHandler] The party walked into an undefended {room.Type}, and rooms taken without a fight are not handled yet"
                );

            expedition.CurrentRoom = room;

            GameInstance.Encounter = new Encounter
            {
                Phase = EncounterPhase.Briefing,
                Combatants = expedition.Heroes.Concat(room.Guardians).ToList(),
            };
            GameInstance.GameMode = GameMode.Encounter;

            var gameEvents = new List<GameEventBase>
            {
                new SimpleGameEvent($"The party breaks into the {room.Type}: {choice.Reason}"),
            };
            return new PlayerActionResult(GameInstance, gameEvents);
        }
    }
}
