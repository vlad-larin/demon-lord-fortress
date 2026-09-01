using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;
using GameScenarios.Helpers;

namespace GameEngine.PlayerActionHandlers
{
    internal class TitleActionHandler : PlayerActionHandler<StartScenarioAction>
    {
        public TitleActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(StartScenarioAction playerAction)
        {
            var scenario = ScenarioRoster.CreateScenario(playerAction.Name);
            var gameInstance = scenario.StartScenario();
            var gameEvents = new GameEventBase[]
            {
                new SimpleGameEvent($"Scenario started: {playerAction.Name}"),
            };
            return new PlayerActionResult(gameInstance, gameEvents);
        }
    }
}
