using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GameCore.Interfaces;
using GameCore.Models;
using GameEngine.Attributes;
using GameEngine.Interfaces;
using GameEngine.PlayerActionHandlers;

namespace GameEngine.Helpers
{
    internal static class PlayerActionHandlerResolver
    {
        private static readonly Dictionary<(Type, GameMode), Type> HandlerTypes = GetHandlerTypes();

        private static Dictionary<(Type, GameMode), Type> GetHandlerTypes()
        {
            var result = new Dictionary<(Type, GameMode), Type>();

            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract);

            foreach (var type in types)
            {
                var ifaces = type.GetInterfaces();
                var ourIfaces = ifaces
                    .Where(i =>
                        i.IsGenericType
                        && i.GetGenericTypeDefinition() == typeof(IPlayerActionHandler<>)
                    )
                    .ToArray();
                if (ourIfaces.Length == 0)
                    continue;

                var gameModes = type.GetCustomAttributes<SupportsGameModeAttribute>()
                    .Select(attr => attr.GameMode);

                foreach (var iface in ourIfaces)
                {
                    foreach (var gameMode in gameModes)
                    {
                        result.Add((iface.GetGenericArguments()[0], gameMode), type);
                    }
                }
            }

            return result;
        }

        public static IPlayerActionHandler Resolve(
            IPlayerAction playerAction,
            GameInstance gameInstance
        )
        {
            var playerActionType = playerAction.GetType();
            if (
                !HandlerTypes.TryGetValue(
                    (playerActionType, gameInstance.GameMode),
                    out var handlerType
                )
            )
                throw new NotImplementedException(
                    $"[PlayerActionHandlerResolver] Not found resolver for {playerActionType.Name} that supports {gameInstance.GameMode} game mode"
                );

            var resolver = Activator.CreateInstance(handlerType, gameInstance);
            return (IPlayerActionHandler)resolver;
        }
    }
}
