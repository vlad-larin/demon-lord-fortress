using System.Collections.Generic;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class SneakyStrike : CombatActionBase
    {
        public int Damage { get; private set; }

        public SneakyStrike(int damage)
            : base("Sneaky strike")
        {
            Damage = damage;
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
            // The bonus damage against an exposed target cannot be applied yet:
            // combatants carry no conditions, so exposure is not tracked anywhere.
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new HpReducedGameEvent(target, Damage));

            target.Hp -= Damage;

            return gameEvents;
        }
    }
}
