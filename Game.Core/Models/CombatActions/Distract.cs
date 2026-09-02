using System.Collections.Generic;
using System.Linq;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Distract : CombatActionBase
    {
        public int DistractRounds { get; private set; }

        public Distract(int distractRounds)
            : base("Distract")
        {
            DistractRounds = distractRounds;
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
            gameEvents.Add(
                new SimpleGameEvent($"{actor.Class} draws the attention of {target.Class} away")
            );

            // Same limitation as Stun: the distraction can only spoil what the target had
            // planned for this round, DistractRounds is not tracked anywhere.
            var distractedIntents = encounter.Intents.Where(i => i.Actor == target).ToList();
            foreach (var distractedIntent in distractedIntents)
            {
                distractedIntent.Action = new Wait();
                distractedIntent.Target = null;
            }

            if (distractedIntents.Count > 0)
                gameEvents.Add(new SimpleGameEvent($"{target.Class} loses track of the plan"));

            return gameEvents;
        }
    }
}
