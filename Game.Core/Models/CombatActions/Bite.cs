using System;

namespace GameCore.Models.CombatActions
{
    public class Bite : CombatActionBase
    {
        public int Damage { get; private set; }

        public Bite(int damage)
            : base("Bite (+50% damage to wounded, heal, exposed while drinks blood)")
        {
            Damage = damage;
        }

        public override int GetDamage(Combatant actor, Combatant target)
        {
            var damage = Damage;
            if (target.Hp < target.MaxHp)
                damage = Convert.ToInt32(Math.Ceiling(damage * 1.5));
            return damage;
        }

        public override int GetProtection(Combatant actor, Combatant target) => 0;
    }
}
