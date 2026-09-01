using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class HeadfirstAttack : CombatActionBase
    {
        public int Damage { get; private set; }
        public int ExposureRounds { get; private set; }

        public HeadfirstAttack(int damage, int exposureRounds)
            : base("Headfirst Attack")
        {
            Damage = damage;
            ExposureRounds = exposureRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => Damage;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetEnemies(actor, combatants);
    }
}
