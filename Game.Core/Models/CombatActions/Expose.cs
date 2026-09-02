using System.Collections.Generic;
using GameCore.Models.GameEvents;

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

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            // Exposure is a condition that later attacks are supposed to read, and there is no
            // condition storage on a combatant yet, so this only reports the opening.
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} finds a weak spot: {target.Class} is exposed for {ExposeRounds} rounds"
                )
            );

            return gameEvents;
        }
    }
}
