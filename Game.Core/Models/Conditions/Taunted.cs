using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Taunted : ConditionBase
    {
        public Combatant TauntedBy { get; private set; }

        public Taunted(Combatant tauntedBy)
        {
            TauntedBy = tauntedBy;
        }
    }
}
