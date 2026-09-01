using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    internal class InitializeActionHandler : PlayerActionHandler<InitializeAction>
    {
        public InitializeActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(InitializeAction playerAction)
        {
            var gameEvents = new GameEventBase[] { new SimpleGameEvent("Game initialized") };
            var gameInstance = new GameInstance() { GameMode = GameMode.Title };
            return new PlayerActionResult(gameInstance, gameEvents);
        }
    }
}
