using System;
using GameConsoleApp.Models;
using GameConsoleApp.StateHandlers.Abstractions;
using GameCore.Models;
using GameCore.ObservableStates;
using GameCore.PlayerActions;

namespace GameConsoleApp.StateHandlers
{
    /// <summary>
    /// The floor between two fights: what the party wants, what is left of it, and the
    /// rooms it could walk into next. The Demon Lord does not get to pick for them - all
    /// he can do here is stop holding the stairs and see where they go.
    /// </summary>
    internal class TowerClimbingModeHandler : StateHandlerBase<TowerClimbingState>
    {
        public TowerClimbingState State { get; private set; }

        public TowerClimbingModeHandler(TowerClimbingState state)
        {
            State = state;
        }

        public override void RenderState(TowerClimbingState state)
        {
            var expedition = state.Expedition;

            RenderFrameStart();
            RenderFrameLine("THE FORTRESS");

            RenderFrameDivider();
            RenderExpedition(expedition);

            var floors = state.Tower.Floors;

            // Top down, the way the heroes will meet the floors in reverse.
            for (var floorNumber = floors.Count; floorNumber >= 1; floorNumber--)
            {
                var isCurrentFloor = floorNumber == expedition.CurrentFloorNumber;

                RenderFrameDivider();
                RenderFrameLine(
                    $"FLOOR {floorNumber}{(isCurrentFloor ? "  <- the party is here" : string.Empty)}"
                );
                RenderFrameLine();

                foreach (var room in floors[floorNumber - 1].Rooms)
                    RenderRoom(room, expedition, isCurrentFloor);
            }

            RenderFrameDivider();
            RenderFrameLine("[Space] Let them come");
            RenderFrameLine("[Q] Quit");
            RenderFrameFinish();
        }

        public override GameModeHandlerResponse ProcessKey(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();
            if (option == "Q")
            {
                return new GameModeHandlerResponse { ActionType = GameModeHandlerActionType.Quit };
            }

            if (option == " ")
            {
                return new GameModeHandlerResponse
                {
                    ActionType = GameModeHandlerActionType.Execute,
                    Action = new AdvanceExpeditionAction(),
                };
            }

            return GameModeHandlerResponse.NoAction();
        }

        private void RenderExpedition(Expedition expedition)
        {
            var objective = expedition.Objective;

            RenderFrameLine($"THEIR OBJECTIVE: {objective.Name}");
            RenderFrameLine(objective.Briefing);
            RenderFrameLine();

            RenderFrameLine("THE PARTY");
            foreach (var hero in expedition.Heroes)
                RenderFrameLine($"    * {CombatantRenderer.RenderWithClass(hero)}");
            RenderFrameLine();
        }

        /// <summary>
        /// A room on the floor the party stands on is a room they could walk into, so it
        /// shows what they weigh when they choose: how dangerous it looks, and whether what
        /// they came for is in it.
        /// </summary>
        private void RenderRoom(Room room, Expedition expedition, bool isCurrentFloor)
        {
            var guardians = room.Guardians;
            var threat =
                isCurrentFloor && !room.Cleared
                    ? $" [threat {room.PerceivedThreat}]"
                    : string.Empty;
            var objective =
                isCurrentFloor && expedition.Objective.IsHeldBy(room)
                    ? " [their objective]"
                    : string.Empty;

            RenderFrameLine(
                $"{room.Type} [{guardians.Count}/{room.Capacity} guardians]{threat}{objective}{(room.Cleared ? " [cleared]" : string.Empty)}"
            );

            foreach (var guardian in guardians)
                RenderFrameLine($"    * {CombatantRenderer.RenderWithClass(guardian)}");

            RenderFrameLine();
        }
    }
}
