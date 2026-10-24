using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameCore.Attributes;
using GameCore.Models;

namespace GameCore.Extensions
{
    public static class CharacterClassExtensions
    {
        private static readonly Dictionary<CharacterClass, string> DisplayNames =
            BuildDisplayNames();

        /// <summary>
        /// The name a player should see for this class. Every value carries one, so a new
        /// class that forgets its attribute is caught the first time anything is rendered
        /// rather than leaking an identifier into the UI.
        /// </summary>
        public static string GetDisplayName(this CharacterClass characterClass) =>
            DisplayNames.TryGetValue(characterClass, out var displayName)
                ? displayName
                : throw new InvalidOperationException(
                    $"[CharacterClassExtensions] Unknown character class: {characterClass}"
                );

        private static Dictionary<CharacterClass, string> BuildDisplayNames() =>
            typeof(CharacterClass)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(
                    field => (CharacterClass)field.GetValue(null),
                    field =>
                        field.GetCustomAttribute<DisplayNameAttribute>()?.Name
                        ?? throw new InvalidOperationException(
                            $"[CharacterClassExtensions] Character class '{field.Name}' has no {nameof(DisplayNameAttribute)}"
                        )
                );
    }
}
