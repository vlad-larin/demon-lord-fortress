namespace GameCore.Models.CombatActions
{
    public class Challenge : CombatActionBase
    {
        public int RiposteCount { get; private set; }
        public int RiposteDamage { get; private set; }

        public Challenge(int riposteCount, int riposteDamage)
            : base("Challenge")
        {
            RiposteCount = riposteCount;
            RiposteDamage = riposteDamage;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;
    }
}
