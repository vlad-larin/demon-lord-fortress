using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class Distract : CombatActionBase
    {
        public int DistractRounds { get; private set; }

        public Distract(int distractRounds)
            : base("Distract")
        {
            DistractRounds = distractRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetEnemies(actor, combatants);
    }
}
