using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class Wait : CombatActionBase
    {
        public Wait()
            : base("Wait") { }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => new List<Combatant> { actor };
    }
}
