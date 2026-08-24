using GameCore.Interfaces;

namespace GameConsoleApp.Models
{
    internal class GameModeHandlerResponse
    {
        public GameModeHandlerActionType ActionType { get; set; }

        public IPlayerAction Action { get; set; }
    }
}
