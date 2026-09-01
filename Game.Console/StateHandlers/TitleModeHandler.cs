using System;
using GameConsoleApp.Models;
using GameConsoleApp.StateHandlers.Abstractions;
using GameCore.ObservableStates;
using GameCore.PlayerActions;

namespace GameConsoleApp.StateHandlers
{
    internal class TitleModeHandler : StateHandlerBase<TitleState>
    {
        public TitleState State { get; private set; }

        public TitleModeHandler(TitleState state)
        {
            State = state;
        }

        public override void RenderState(TitleState state)
        {
            RenderFrameStart();
            RenderFrameLine("Welcome to Demon Lord Fortess!");
            RenderFrameLine();
            RenderFrameLine("CHOOSE THE OPTION");
            RenderFrameLine();
            for (int i = 0; i < state.NewScenarioNames.Length; i++)
            {
                RenderFrameLine($"[{i + 1}] Start '{state.NewScenarioNames[i]}' scenario");
            }
            RenderFrameLine();
            RenderFrameLine($"[Q] Quit");
            RenderFrameFinish();
        }

        public override GameModeHandlerResponse ProcessKey(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();
            if (option == "Q")
            {
                return new GameModeHandlerResponse()
                {
                    ActionType = GameModeHandlerActionType.Quit,
                };
            }
            else if (
                int.TryParse(option, out var scenarioNumber)
                && scenarioNumber > 0
                && scenarioNumber <= State.NewScenarioNames.Length
            )
            {
                var scenarioName = State.NewScenarioNames[scenarioNumber - 1];
                return new GameModeHandlerResponse()
                {
                    ActionType = GameModeHandlerActionType.Execute,
                    Action = new StartScenarioAction { Name = scenarioName },
                };
            }

            return new GameModeHandlerResponse()
            {
                ActionType = GameModeHandlerActionType.NoAction,
            };
        }
    }
}
