using System.Collections.Generic;
using GameCore.Extensions;
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
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} wraps {target.Class} in a ward of {Durability} points for {WardRounds} rounds"
                )
            );

            var warded = target.GetCondition<Warded>();
            if (warded == null)
                target.Conditions.Add(new Warded(wardRounds: WardRounds, durability: Durability));
            else
                warded.RenewWard(wardRounds: WardRounds, durability: Durability);

            return gameEvents;
        }
    }
}
