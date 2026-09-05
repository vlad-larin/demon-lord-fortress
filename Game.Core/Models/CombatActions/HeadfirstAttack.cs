using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Models.Conditions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class HeadfirstAttack : CombatActionBase
    {
        public int Damage { get; private set; }
        public int ExposureRounds { get; private set; }

        public HeadfirstAttack(int damage, int exposureRounds)
            : base("Headfirst Attack")
        {
            Damage = damage;
            ExposureRounds = exposureRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => Damage;

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
                new SimpleGameEvent(
                    $"{actor.Class} charges in headfirst and stays exposed for {ExposureRounds} rounds"
                )
            );

            gameEvents.AddRange(target.InflictDamage(Damage));
            actor.ApplyForRounds<Exposed>(ExposureRounds);

            return gameEvents;
        }
    }
}
