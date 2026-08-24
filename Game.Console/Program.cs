using System;
using GameConsoleApp.Models;
using GameConsoleApp.Renderers;
using GameCore.Models;
using GameCore.ObservableStates;
using GameCore.PlayerActions;
using GameEngine;

namespace GameConsoleApp
{
    class Program
    {
        private static readonly MainChannel mainChannel = new MainChannel();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var state = mainChannel.Execute(new InitializeAction());
            while (true)
            {
                Console.WriteLine($"State: {state}");
                RenderState(state);
                var keyInfo = Console.ReadKey();

                Console.WriteLine();
                Console.WriteLine("-------");

                var handlerResponse = ProcessKey(keyInfo, state);
                var actionType = handlerResponse.ActionType;

                if (actionType == GameModeHandlerActionType.Quit)
                {
                    Console.WriteLine("Quitting the game...");
                    break;
                }

                if (actionType == GameModeHandlerActionType.NoAction)
                {
                    continue;
                }

                if (actionType == GameModeHandlerActionType.Execute)
                {
                    state = mainChannel.Execute(handlerResponse.Action);
                    continue;
                }

                throw new NotImplementedException($"[Main] Unknown action: {actionType}");
            }
            Console.WriteLine("Game finished.");
        }

        private static void RenderState(ObservableStateBase state)
        {
            foreach (var gameEvent in state.GameEvents)
            {
                Console.WriteLine($"{gameEvent.GetType().Name}: {gameEvent.Description}");
            }

            switch (state.GameMode)
            {
                case GameMode.Title:
                    TitleModeHandler.RenderState((TitleState)state);
                    break;
                case GameMode.Encounter:
                    EncounterRenderer.RenderState((EncounterState)state);
                    break;
                case GameMode.Map:
                    MapRenderer.RenderState(state);
                    break;
                default:
                    throw new NotImplementedException(
                        $"[RenderState] Unknown game mode: {state.GameMode}"
                    );
            }
        }

        private static GameModeHandlerResponse ProcessKey(
            ConsoleKeyInfo key,
            ObservableStateBase state
        )
        {
            switch (state.GameMode)
            {
                case GameMode.Title:
                    return TitleModeHandler.ProcessKey(key, (TitleState)state);
                default:
                    throw new NotImplementedException(
                        $"[ProcessKey] Unknown game mode: {state.GameMode}"
                    );
            }
        }
    }
}
