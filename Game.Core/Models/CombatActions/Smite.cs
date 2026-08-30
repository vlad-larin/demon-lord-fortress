namespace GameCore.Models.CombatActions
{
    public class Smite : CombatActionBase
    {
        public int HolyDamage { get; private set; }

        public Smite(int holyDamage)
            : base("Smite")
        {
            HolyDamage = holyDamage;
        }

        public override int GetDamage(Combatant actor, Combatant target) => HolyDamage;

        public override int GetProtection(Combatant actor, Combatant target) => 0;
    }
}
