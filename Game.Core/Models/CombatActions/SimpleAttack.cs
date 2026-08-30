namespace GameCore.Models.CombatActions
{
    public class SimpleAttack : CombatActionBase
    {
        public int Damage { get; private set; }

        public SimpleAttack(int damage)
            : base("Attack")
        {
            Damage = damage;
        }

        public override int GetDamage(Combatant actor, Combatant target) => Damage;

        public override int GetProtection(Combatant actor, Combatant target) => 0;
    }
}
