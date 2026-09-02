using System.Collections.Generic;
using System.Linq;
using GameCore.Models.Conditions;
using GameCore.Models.GameEvents;

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

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            // A ward is a pool of shield points that absorbs damage for WardRounds rounds.
            // Combatants have nowhere to keep it, so the shield is only announced.
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} wraps {target.Class} in a ward of {Durability} points for {WardRounds} rounds"
                )
            );

            ApplyWard(target, WardRounds, Durability);

            return gameEvents;
        }

        private void ApplyWard(Combatant target, int wardRounds, int durability)
        {
            if (target.Conditions.FirstOrDefault(c => c is Warded) is Warded warded)
                warded.RenewWard(wardRounds: wardRounds, durability: durability);
            else
                target.Conditions.Add(new Warded(wardRounds: wardRounds, durability: durability));
        }
    }
}
