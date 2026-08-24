using System;
using GameCore.Models;

namespace GameEngine.Attributes
{
    internal class SupportsGameModeAttribute : Attribute
    {
        public GameMode GameMode { get; private set; }

        public SupportsGameModeAttribute(GameMode gameMode)
        {
            GameMode = gameMode;
        }
    }
}
