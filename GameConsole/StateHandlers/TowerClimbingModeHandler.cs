using System;
using GameConsoleApp.Models;
using GameConsoleApp.StateHandlers.Abstractions;
using GameCore.Models;
using GameCore.ObservableStates;

namespace GameConsoleApp.StateHandlers
{
    /// <summary>
    /// A read-only look at the tower: every floor, its rooms and the guardians standing
    /// in them. Climbing it is not implemented yet, so the only way out is quitting.
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
            RenderFrameStart();
            RenderFrameLine("THE FORTRESS");

            var floors = state.Tower.Floors;

            // Top down, the way the heroes will meet the floors in reverse.
            for (var floorNumber = floors.Count; floorNumber >= 1; floorNumber--)
            {
                RenderFrameDivider();
                RenderFrameLine($"FLOOR {floorNumber}");
                RenderFrameLine();

                foreach (var room in floors[floorNumber - 1].Rooms)
                    RenderRoom(room);
            }

            RenderFrameDivider();
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

            return GameModeHandlerResponse.NoAction();
        }

        private void RenderRoom(Room room)
        {
            var guardians = room.Guardians;
            RenderFrameLine(
                $"{room.Type} [{guardians.Count}/{room.Capacity} guardians]{(room.Cleared ? " [cleared]" : string.Empty)}"
            );

            foreach (var guardian in guardians)
                RenderFrameLine($"    * {CombatantRenderer.Render(guardian)}");

            RenderFrameLine();
        }
    }
}
