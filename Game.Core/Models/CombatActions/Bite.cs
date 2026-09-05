using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies.Helpers;

namespace GameCore.Models.CombatActions
{
    public class Bite : CombatActionBase
    {
        public int Damage { get; private set; }

        public Bite(int damage)
            : base("Bite (+50% damage to wounded, heal, exposed while drinks blood)")
        {
            Damage = damage;
        }

        public override int GetDamage(Combatant actor, Combatant target)
        {
            var damage = Damage;
            if (target.Hp < target.MaxHp)
                damage = Convert.ToInt32(Math.Ceiling(damage * 1.5));
            return damage;
        }

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
            gameEvents.Add(new SimpleGameEvent($"{actor.Class} bites {target.Class}'s neck!"));

            var targetIsWounded = target.Hp < target.MaxHp;
            var damage = targetIsWounded
                ? Convert.ToInt32(Math.Ceiling(decimal.Multiply(Damage, 1.5m)))
                : Damage;
            var heal = Math.Ceiling(decimal.Divide(damage, 2m));

            gameEvents.Add(new HpReducedGameEvent(target, damage));

            gameEvents.AddRange(target.InflictDamage(damage));

            actor.Hp += Convert.ToInt32(Math.Max(0, Math.Min(actor.MaxHp - actor.Hp, heal)));

            return gameEvents;
        }
    }
}
