using System.Collections.Generic;

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

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetEnemies(actor, combatants);

        public override void Execute(Combatant actor, Combatant target, Encounter encounter)
        {
            target.Hp -= Damage;
        }
    }
}
