using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCore.Attributes;

namespace GameCore.Extensions
{
    public static class DisplayNameExtensions
    {
        /// <summary>
        /// The name a player should see for this enum value. Every value of an enum that
        /// reaches the UI carries one, so a new value that forgets its attribute is caught
        /// the first time anything is rendered rather than leaking an identifier into the UI.
        /// </summary>
        public static string GetDisplayName<TEnum>(this TEnum value)
            where TEnum : struct, Enum =>
            DisplayNames<TEnum>.ByValue.TryGetValue(value, out var displayName)
                ? displayName
                : throw new InvalidOperationException(
                    $"[DisplayNameExtensions] Unknown {typeof(TEnum).Name}: {value}"
                );

        /// <summary>
        /// Read once per enum type, the first time one of its values is shown.
        /// </summary>
        private static class DisplayNames<TEnum>
            where TEnum : struct, Enum
        {
            public static readonly Dictionary<TEnum, string> ByValue = typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(
                    field => (TEnum)field.GetValue(null),
                    field =>
                        field.GetCustomAttribute<DisplayNameAttribute>()?.Name
                        ?? throw new InvalidOperationException(
                            $"[DisplayNameExtensions] {typeof(TEnum).Name} '{field.Name}' has no {nameof(DisplayNameAttribute)}"
                        )
                );
        }
    }
}
