using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Protected : ConditionBase
    {
        public Combatant ProtectedBy { get; private set; }
        public int ProtectRounds { get; private set; }

        public Protected(Combatant protectedBy, int protectRounds)
        {
            ProtectedBy = protectedBy;
            ProtectRounds = protectRounds;
        }
    }
}
