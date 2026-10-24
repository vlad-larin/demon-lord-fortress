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

        /// <summary>
        /// Rituals build on each other; the strength simply adds up.
        /// </summary>
        public override void Absorb(ConditionBase incoming) =>
            Strength += ((Strengthened)incoming).Strength;

        public override int GetDamageFlatModifier() => Strength;
    }
}
