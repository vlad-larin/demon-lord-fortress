using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Models;

namespace GameEngine.PlayerActionHandlers.Abstractions
{
    public abstract class PlayerActionHandlerBase
    {
        protected GameInstance GameInstance { get; private set; }

        public PlayerActionHandlerBase(GameInstance gameInstance)
        {
            GameInstance = gameInstance;
        }
    }
}
