using GameCore.Interfaces;
using GameCore.Models;
using GameCore.ObservableStates;
using GameEngine.Extensions;
using GameEngine.Helpers;

namespace GameEngine
{
    public class MainChannel
    {
        private GameInstance gameInstance = new GameInstance
        {
            GameMode = GameCore.Models.GameMode.NotInitialized,
        };

        public ObservableStateBase Execute(IPlayerAction action)
        {
            var handler = PlayerActionHandlerResolver.Resolve(action, gameInstance);
            var result = handler.HandlePlayerAction(action);
            gameInstance = result.GameInstance;
            return gameInstance.ToObservableState(result.GameEvents);
        }
    }
}
