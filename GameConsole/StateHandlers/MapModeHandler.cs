using System;
using System.Linq;
using GameConsoleApp.Models;
using GameConsoleApp.StateHandlers.Abstractions;
using GameCore.Extensions;
using GameCore.Models;
using GameCore.ObservableStates;
using GameCore.PlayerActions;

namespace GameConsoleApp.StateHandlers
{
    /// <summary>
    /// The kingdom seen from the tower: the map, and under it what each label stands for
    /// and where its roads lead. The Demon Lord has no orders to give out here yet, so the
    /// only thing to do is let the turn pass.
    /// </summary>
    internal class MapModeHandler : StateHandlerBase<MapState>
    {
        private const int MapHeight = 19;

        public MapState State { get; private set; }

        public MapModeHandler(MapState state)
        {
            State = state;
        }

        public override void RenderState(MapState state)
        {
            var map = state.WorldMap;

            // Labeled the way numbered options are, so the day the player picks a location
            // to act on, the key to press is the one already printed on the map.
            var labels = map
                .Locations.Select((location, index) => (location, index))
                .ToDictionary(entry => entry.location, entry => RenderOptionLabel(entry.index));

            RenderFrameStart();
            RenderFrameLine($"THE KINGDOM - TURN {state.Turn}");

            RenderFrameDivider();
            foreach (var line in WorldMapRenderer.Draw(map, labels, FrameContentWidth, MapHeight))
                RenderFrameLine(line);

            RenderFrameDivider();
            foreach (var location in map.Locations)
            {
                var roads = string.Join(
                    ", ",
                    map.GetNeighbors(location).Select(neighbor => neighbor.Name)
                );
                RenderFrameLine(
                    $"[{labels[location]}] {location.Name} ({location.Type.GetDisplayName()}) -> {roads}"
                );
            }

            RenderFrameDivider();
            RenderFrameLine("[Space] Skip turn");
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
                    Action = new SkipTurnAction(),
                };
            }

            return GameModeHandlerResponse.NoAction();
        }
    }
}
