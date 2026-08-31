using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class SneakyStrike : CombatActionBase
    {
        public int Damage { get; private set; }

        public SneakyStrike(int damage)
            : base("Sneaky strike")
        {
            Damage = damage;
        }

        public override int GetDamage(Combatant actor, Combatant target) => Damage;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetEnemies(actor, combatants);
    }
}
