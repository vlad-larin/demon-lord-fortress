using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.ObservableStates;

namespace GameEngine.Models
{
    internal class PlayerActionResult
    {
        public GameInstance GameInstance { get; private set; }
        public IEnumerable<GameEventBase> GameEvents { get; protected set; }

        public PlayerActionResult(GameInstance gameInstance, IEnumerable<GameEventBase> gameEvents)
        {
            GameInstance = gameInstance;
            GameEvents = gameEvents;
        }
    }
}
