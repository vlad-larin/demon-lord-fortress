using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Distracted : TimedConditionBase
    {
        public override decimal GetDamageMultiplier() => 0.5m;
    }
}
