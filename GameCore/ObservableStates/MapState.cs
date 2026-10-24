using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;

namespace GameCore.ObservableStates
{
    public class MapState : ObservableStateBase
    {
        public WorldMap WorldMap { get; set; }

        public int Turn { get; set; }

        public MapState(IEnumerable<GameEventBase> gameEvents)
            : base(gameEvents)
        {
            GameMode = GameMode.Map;
        }
    }
}
