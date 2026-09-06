using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Models.Conditions;
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

            target.ApplyForRounds<Distracted>(DistractRounds);

            return gameEvents;
        }
    }
}
