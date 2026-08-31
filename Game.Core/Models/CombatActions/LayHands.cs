using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class LayHands : CombatActionBase
    {
        public int Heal { get; private set; }

        public LayHands(int heal)
            : base("Lay Hands")
        {
            Heal = heal;
        }

        public override int GetDamage(Combatant actor, Combatant target) => -Heal;

        public override int GetProtection(Combatant actor, Combatant target) => Heal;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetAllies(actor, combatants);
    }
}
