namespace GameCore.Models.CombatActions
{
    public class Blast : CombatActionBase
    {
        public int Damage { get; private set; }

        public Blast(int damage)
            : base("Blast")
        {
            Damage = damage;
        }

        public override int GetDamage(Combatant actor, Combatant target) => Damage;

        public override int GetProtection(Combatant actor, Combatant target) => 0;
    }
}
