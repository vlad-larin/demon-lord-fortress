using System;
using System.Linq;
using System.Reflection;
using GameScenarios.Attributes;

namespace GameScenarios.Helpers
{
    public static class ScenarioRoster
    {
        private static ScenarioRosterItem[]? _availableItems = null;

        private static ScenarioRosterItem[] AvailableItems
        {
            get
            {
                if (_availableItems == null)
                {
                    _availableItems = Assembly
                        .GetExecutingAssembly()
                        .GetTypes()
                        .Where(type => type.IsClass)
                        .Select(type => new ScenarioRosterItem(
                            type,
                            type.GetCustomAttribute<ScenarioAttribute>()
                        ))
                        .Where(item => item.ScenarioAttribute != null)
                        .ToArray();
                }
                return _availableItems;
            }
        }

        public static string[] GetAvailableNames() =>
            AvailableItems.Select(x => x.ScenarioAttribute.Name).ToArray();

        public static Type GetScenarioTypeByName(string name) =>
            AvailableItems.First(item => item.ScenarioAttribute.Name == name).Type;

        private class ScenarioRosterItem
        {
            public Type Type { get; private set; }
            public ScenarioAttribute ScenarioAttribute { get; private set; }

            public ScenarioRosterItem(Type type, ScenarioAttribute scenarioAttribute)
            {
                Type = type;
                ScenarioAttribute = scenarioAttribute;
            }
        }
    }
}
