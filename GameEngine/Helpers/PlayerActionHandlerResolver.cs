using System;
using System.Collections.Generic;
using GameCore.Interfaces;
using GameCore.Models;
using GameCore.PlayerActions;
using GameEngine.Interfaces;
using GameEngine.PlayerActionHandlers;

namespace GameEngine.Helpers
{
    internal static class PlayerActionHandlerResolver
    {
        private static readonly Dictionary<
            (Type PlayerActionType, GameMode GameMode),
            Func<GameInstance, IPlayerActionHandler>
        > Handlers = BuildHandlers();

        /// <summary>
        /// The single place where a player action is bound to the handler that serves it.
        /// Every handler is referenced statically so the compiler checks the binding and
        /// no linker (Unity/IL2CPP included) can strip a handler it cannot see being used.
        /// </summary>
        private static Dictionary<
            (Type, GameMode),
            Func<GameInstance, IPlayerActionHandler>
        > BuildHandlers()
        {
            var handlers =
                new Dictionary<(Type, GameMode), Func<GameInstance, IPlayerActionHandler>>();

            void Register<TPlayerAction>(
                GameMode gameMode,
                Func<GameInstance, IPlayerActionHandler<TPlayerAction>> createHandler
            )
                where TPlayerAction : IPlayerAction
            {
                var key = (typeof(TPlayerAction), gameMode);
                if (handlers.ContainsKey(key))
                    throw new InvalidOperationException(
                        $"[PlayerActionHandlerResolver] {typeof(TPlayerAction).Name} is already registered for {gameMode} game mode"
                    );

                handlers.Add(key, gameInstance => createHandler(gameInstance));
            }

            Register<InitializeAction>(
                GameMode.NotInitialized,
                gameInstance => new InitializeActionHandler(gameInstance)
            );
            Register<StartScenarioAction>(
                GameMode.Title,
                gameInstance => new TitleActionHandler(gameInstance)
            );
            Register<ComposeHeroPartyPlanAction>(
                GameMode.Encounter,
                gameInstance => new ComposeHeroPartyPlanActionHandler(gameInstance)
            );
            Register<InsertCombatActionIntoBattlePlan>(
                GameMode.Encounter,
                gameInstance => new InsertCombatActionIntoBattlePlanActionHandler(gameInstance)
            );
            Register<ExecuteBattlePlanAction>(
                GameMode.Encounter,
                gameInstance => new ExecuteBattlePlanActionHandler(gameInstance)
            );
            Register<FinishEncounterRoundResolutionAction>(
                GameMode.Encounter,
                gameInstance => new FinishEncounterRoundResolutionActionHandler(gameInstance)
            );

            return handlers;
        }

        public static IPlayerActionHandler Resolve(
            IPlayerAction playerAction,
            GameInstance gameInstance
        )
        {
            var playerActionType = playerAction.GetType();
            var gameMode = gameInstance.GameMode;

            if (!Handlers.TryGetValue((playerActionType, gameMode), out var createHandler))
                throw new NotImplementedException(
                    $"[PlayerActionHandlerResolver] Not found resolver for {playerActionType.Name} that supports {gameInstance.GameMode} game mode"
                );

            return createHandler(gameInstance);
        }
    }
}
