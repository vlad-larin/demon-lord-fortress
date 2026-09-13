using GameCore.Interfaces;

namespace GameConsoleApp.Models
{
    internal class GameModeHandlerResponse
    {
        public GameModeHandlerActionType ActionType { get; set; }

        public IPlayerAction Action { get; set; }

        public static GameModeHandlerResponse NoAction() =>
            new GameModeHandlerResponse { ActionType = GameModeHandlerActionType.NoAction };
    }
}
