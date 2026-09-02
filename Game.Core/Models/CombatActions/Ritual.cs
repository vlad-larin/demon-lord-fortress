using System.Collections.Generic;
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
            // Combatants have no damage modifier to grow, so the ritual only makes the caster
            // look more dangerous, which is what actually pulls the attention of the party.
            var gameEvents = new List<GameEventBase>();

            actor.PerceivedDanger += Strength;

            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} channels a dark ritual and grows {Strength} more terrifying"
                )
            );

            return gameEvents;
        }
    }
}
