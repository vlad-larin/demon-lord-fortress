using System;
using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Warded : ConditionBase
    {
        public int WardRounds { get; private set; }
        public int Durability { get; private set; }

        public Warded(int wardRounds, int durability)
        {
            WardRounds = wardRounds;
            Durability = durability;
        }

        internal void RenewWard(int wardRounds, int durability)
        {
            WardRounds = Convert.ToInt32(Math.Max(wardRounds, WardRounds));
            Durability += durability;
        }
    }
}
