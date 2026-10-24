using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    /// <summary>
    /// A turn on the world map with no orders given. Nothing in the kingdom acts on its own
    /// yet, so all that happens is that the turn passes.
    /// </summary>
    internal class SkipTurnActionHandler : PlayerActionHandler<SkipTurnAction>
    {
        public SkipTurnActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(SkipTurnAction playerAction)
        {
            GameInstance.Turn++;

            var gameEvents = new GameEventBase[]
            {
                new SimpleGameEvent(
                    $"The kingdom goes about its business. Turn {GameInstance.Turn} begins."
                ),
            };
            return new PlayerActionResult(GameInstance, gameEvents);
        }
    }
}
