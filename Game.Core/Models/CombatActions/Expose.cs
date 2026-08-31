using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class Expose : CombatActionBase
    {
        public int ExposeRounds { get; private set; }

        public Expose(int exposeRounds)
            : base("Expose")
        {
            ExposeRounds = exposeRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetEnemies(actor, combatants);
    }
}
