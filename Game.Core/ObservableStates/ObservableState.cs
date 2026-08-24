using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;

namespace GameCore.ObservableStates
{
    public class ObservableStateBase
    {
        public GameMode GameMode { get; protected set; }

        public IEnumerable<GameEventBase> GameEvents { get; protected set; }

        public ObservableStateBase(IEnumerable<GameEventBase> gameEvents)
        {
            GameEvents = gameEvents;
        }
    }
}
