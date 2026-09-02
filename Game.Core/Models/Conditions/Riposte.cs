using System;
using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    internal class Riposte : ConditionBase
    {
        public int RiposteCount { get; private set; }
        public int RiposteDamage { get; private set; }

        public Riposte(int riposteCount, int riposteDamage)
        {
            RiposteCount = riposteCount;
            RiposteDamage = riposteDamage;
        }

        /// <summary>
        /// Counter attacks pile up, the hardest one sets the damage.
        /// </summary>
        internal void RenewRiposte(int riposteCount, int riposteDamage)
        {
            RiposteCount += riposteCount;
            RiposteDamage = Math.Max(riposteDamage, RiposteDamage);
        }
    }
}
