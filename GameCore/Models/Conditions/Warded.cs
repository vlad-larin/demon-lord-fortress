using System;
using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Warded : TimedConditionBase
    {
        public int Durability { get; private set; }

        public Warded(int wardRounds, int durability)
        {
            AddRounds(wardRounds);
            Durability = durability;
        }

        /// <summary>
        /// Wards pile their strength together, and the longer of the two decides how long
        /// the whole thing holds.
        /// </summary>
        public override void Absorb(ConditionBase incoming)
        {
            var renewal = (Warded)incoming;
            AddRounds(renewal.RoundsLeft - RoundsLeft);
            Durability += renewal.Durability;
        }

        public override int GetIncomingDamageFlatModifier(int actualDamage)
        {
            var wardedDamage = Math.Clamp(actualDamage, 0, Durability);
            Durability -= wardedDamage;
            return -wardedDamage;
        }

        public override bool ShouldBeRemoved() => base.ShouldBeRemoved() || Durability <= 0;
    }
}
