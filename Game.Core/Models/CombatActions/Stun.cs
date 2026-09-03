using System.Collections.Generic;
using System.Linq;
using GameCore.Extensions;
using GameCore.Models.Conditions;
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

            target.ApplyForRounds<Stunned>(StunRounds);

            // TEMPORARY: replace with condition processing during executions
            var stunnedIntents = encounter
                .Intents.Where(i => i.Actor == target && !i.IsExecuted)
                .ToList();
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
