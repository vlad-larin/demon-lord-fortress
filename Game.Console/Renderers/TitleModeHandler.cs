using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameConsoleApp.Models;
using GameCore.ObservableStates;
using GameCore.PlayerActions;

namespace GameConsoleApp.Renderers
{
    public static class TitleModeHandler
    {
        internal static void RenderState(TitleState state)
        {
            Console.WriteLine("Welcome to Demon Lord Fortess!");
            Console.WriteLine();
            Console.WriteLine("Choose the option:");

            for (int i = 0; i < state.NewScenarioNames.Length; i++)
            {
                Console.WriteLine($"{i + 1}: Start '{state.NewScenarioNames[i]}' scenario");
            }
            Console.WriteLine($"Q: Quit");
        }

        internal static GameModeHandlerResponse ProcessKey(ConsoleKeyInfo key, TitleState state)
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
                && scenarioNumber <= state.NewScenarioNames.Length
            )
            {
                var scenarioName = state.NewScenarioNames[scenarioNumber - 1];
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
