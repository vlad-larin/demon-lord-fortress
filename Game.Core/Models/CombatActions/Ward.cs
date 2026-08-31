using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class Ward : CombatActionBase
    {
        public int WardRounds { get; private set; }
        public int Durability { get; private set; }

        public Ward(int wardRounds, int durability)
            : base("Ward")
        {
            WardRounds = wardRounds;
            Durability = durability;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => Durability;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetAllies(actor, combatants);
    }
}
