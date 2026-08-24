using System;
using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.ObservableStates;

namespace GameEngine.Extensions
{
    public static class GameInstanceExtensions
    {
        internal static ObservableStateBase ToObservableState(
            this GameInstance gameInstance,
            IEnumerable<GameEventBase> gameEvents
        )
        {
            var gameMode = gameInstance.GameMode;
            switch (gameMode)
            {
                case GameMode.Title:
                    return new TitleState(gameEvents)
                    {
                        NewScenarioNames = GameScenarios.Helpers.ScenarioRoster.GetAvailableNames(),
                    };

                case GameMode.Encounter:
                    return new EncounterState(gameEvents) { Encounter = gameInstance.Encounter };

                default:
                    throw new NotImplementedException(
                        $"[GameInstance] Unexpected game mode: {gameMode}"
                    );
            }
        }
    }
}
