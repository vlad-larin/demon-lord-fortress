using System;
using System.Collections.Generic;
using System.Linq;
using GameScenarios.Scenarios;

namespace GameScenarios.Helpers
{
    public static class ScenarioRoster
    {
        private static readonly IReadOnlyList<ScenarioRosterItem> AvailableItems =
            BuildAvailableItems();

        /// <summary>
        /// The single place where a playable scenario is given its display name.
        /// Every scenario is constructed through a statically referenced factory so the
        /// compiler checks the binding and no linker can strip a scenario it cannot see
        /// being used.
        /// </summary>
        private static IReadOnlyList<ScenarioRosterItem> BuildAvailableItems()
        {
            var items = new List<ScenarioRosterItem>();

            void Register(string name, Func<ScenarioBase> createScenario)
            {
                if (items.Any(item => item.Name == name))
                    throw new InvalidOperationException(
                        $"[ScenarioRoster] Scenario '{name}' is already registered"
                    );

                items.Add(new ScenarioRosterItem(name, createScenario));
            }

            Register("Test combat scenario", () => new TestCombatScenario());

            return items;
        }

        public static string[] GetAvailableNames() =>
            AvailableItems.Select(item => item.Name).ToArray();

        public static ScenarioBase CreateScenario(string name)
        {
            var item = AvailableItems.FirstOrDefault(x => x.Name == name);
            if (item == null)
                throw new InvalidOperationException(
                    $"[ScenarioRoster] Unknown scenario '{name}'. Available scenarios: {string.Join(", ", GetAvailableNames())}"
                );

            return item.CreateScenario();
        }

        private class ScenarioRosterItem
        {
            public string Name { get; }
            public Func<ScenarioBase> CreateScenario { get; }

            public ScenarioRosterItem(string name, Func<ScenarioBase> createScenario)
            {
                Name = name;
                CreateScenario = createScenario;
            }
        }
    }
}
