using System;

namespace GameCore.Attributes
{
    /// <summary>
    /// The human-facing name of an enum value - what the UI and the battle log print
    /// instead of the identifier, so "GiantRat" reaches the player as "Giant Rat".
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayNameAttribute : Attribute
    {
        public string Name { get; }

        public DisplayNameAttribute(string name)
        {
            Name = name;
        }
    }
}
