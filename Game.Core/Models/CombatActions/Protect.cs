using System.Collections.Generic;

namespace GameCore.Models.CombatActions
{
    public class Protect : CombatActionBase
    {
        public int ProtectRounds { get; private set; }

        public Protect(int protectRounds)
            : base("Protect")
        {
            ProtectRounds = protectRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) =>
            target == actor ? 0 : actor.Hp / 2;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetAllies(actor, combatants);
    }
}
