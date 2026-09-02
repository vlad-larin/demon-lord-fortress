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
    }
}
