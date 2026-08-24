using System;

namespace GameScenarios.Attributes
{
    internal class ScenarioAttribute : Attribute
    {
        public string Name { get; set; }

        public ScenarioAttribute(string name)
        {
            Name = name;
        }
    }
}
