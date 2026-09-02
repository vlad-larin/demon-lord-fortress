using System.Collections.Generic;
using System.Linq;
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

            target.Hp -= Damage;
            InflictExposed(actor, ExposureRounds);

            // The self-inflicted exposure is only reported: combatants carry no conditions,
            // so there is nowhere to store it for the next ExposureRounds rounds.
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} charges in headfirst and stays exposed for {ExposureRounds} rounds"
                )
            );

            gameEvents.Add(new HpReducedGameEvent(target, Damage));

            return gameEvents;
        }

        private void InflictExposed(Combatant target, int rounds)
        {
            if (target.Conditions.FirstOrDefault(c => c is Exposed) is Exposed exposed)
                exposed.AddRounds(rounds);
            else
                target.Conditions.Add(new Exposed(rounds));
        }
    }
}
