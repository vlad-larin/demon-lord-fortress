using GameCore.Interfaces;
using GameCore.Models;
using GameEngine.Interfaces;
using GameEngine.Models;

namespace GameEngine.PlayerActionHandlers.Abstractions
{
    internal abstract class PlayerActionHandler<TPlayerAction>
        : PlayerActionHandlerBase,
            IPlayerActionHandler<TPlayerAction>
        where TPlayerAction : IPlayerAction
    {
        protected PlayerActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public abstract PlayerActionResult HandlePlayerAction(TPlayerAction playerAction);

        public PlayerActionResult HandlePlayerAction(IPlayerAction playerAction) =>
            HandlePlayerAction((TPlayerAction)playerAction);
    }
}
