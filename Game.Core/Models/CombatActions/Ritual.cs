using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Models.Conditions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Ritual : CombatActionBase
    {
        public int Strength { get; private set; }

        public Ritual(int strength)
            : base("Ritual (grow strength)")
        {
            Strength = strength;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => new List<Combatant> { actor };

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} channels a dark ritual and grows {Strength} more terrifying"
                )
            );

            // TEMPORARY: the growing strength should come from the condition alone once
            // condition processing is in place.
            actor.PerceivedDanger += Strength;

            var strengthened = actor.GetCondition<Strengthened>();
            if (strengthened == null)
                actor.Conditions.Add(new Strengthened(Strength));
            else
                strengthened.AddStrength(Strength);

            return gameEvents;
        }
    }
}
