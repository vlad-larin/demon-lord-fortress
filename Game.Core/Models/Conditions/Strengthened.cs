using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Strengthened : ConditionBase
    {
        public int Strength { get; private set; }

        public Strengthened(int strength)
        {
            Strength = strength;
        }

        internal void AddStrength(int strength)
        {
            Strength += strength;
        }
    }
}
