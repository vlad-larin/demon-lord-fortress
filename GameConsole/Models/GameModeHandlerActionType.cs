using System;
using System.Collections.Generic;
using System.Text;

namespace GameConsoleApp.Models
{
    internal enum GameModeHandlerActionType
    {
        /// <summary>
        /// Should never be used
        /// </summary>
        Unknown,

        /// <summary>
        /// UI does not react (usually because player pressed a key that does not correspond to anything)
        /// </summary>
        NoAction,

        /// <summary>
        /// Quit the game immediately
        /// </summary>
        Quit,

        /// <summary>
        /// Send the action to game engine
        /// </summary>
        Execute,
    }
}
