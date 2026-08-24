using System;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Attributes;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;
using GameScenarios.Helpers;
using GameScenarios.Scenarios;

namespace GameEngine.PlayerActionHandlers
{
    [SupportsGameMode(GameMode.Title)]
    internal class TitleActionHandler : PlayerActionHandler<StartScenarioAction>
    {
        public TitleActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(StartScenarioAction playerAction)
        {
            var scenarioType = ScenarioRoster.GetScenarioTypeByName(playerAction.Name);
            var scenario = (ScenarioBase)Activator.CreateInstance(scenarioType);
            var gameInstance = scenario.StartScenario();
            var gameEvents = new GameEventBase[]
            {
                new SimpleGameEvent($"Scenario started: {playerAction.Name}"),
            };
            return new PlayerActionResult(gameInstance, gameEvents);
        }
    }
}
