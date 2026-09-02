using System.Collections.Generic;
using System.Linq;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Stun : CombatActionBase
    {
        public int StunRounds { get; private set; }

        public Stun(int stunRounds)
            : base("Stun")
        {
            StunRounds = stunRounds;
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
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new SimpleGameEvent($"{actor.Class} stuns {target.Class}!"));

            // Only the current round can be taken away: nothing tracks conditions across
            // rounds, so StunRounds is reported but not enforced.
            var stunnedIntents = encounter.Intents.Where(i => i.Actor == target).ToList();
            foreach (var stunnedIntent in stunnedIntents)
            {
                stunnedIntent.Action = new Wait();
                stunnedIntent.Target = null;
            }

            if (stunnedIntents.Count > 0)
                gameEvents.Add(new SimpleGameEvent($"{target.Class} loses their action"));

            return gameEvents;
        }
    }
}
