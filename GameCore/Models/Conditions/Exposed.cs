using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Exposed : TimedConditionBase
    {
        public override decimal GetIncomingDamageMultiplier() => 1.5m;
    }
}
