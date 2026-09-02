using System;
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

        /// <summary>
        /// The newest protector takes over, keeping whichever cover lasts longer.
        /// </summary>
        internal void RenewProtection(Combatant protectedBy, int protectRounds)
        {
            ProtectedBy = protectedBy;
            ProtectRounds = Math.Max(protectRounds, ProtectRounds);
        }
    }
}
