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

        internal void RenewWard(int wardRounds, int durability)
        {
            AddRounds(wardRounds <= RoundsLeft ? 0 : wardRounds - RoundsLeft);
            Durability += durability;
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
