using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public abstract class CombatActionBase
    {
        public string Name { get; }

        protected CombatActionBase(string name)
        {
            Name = name;
        }

        public abstract int GetDamage(Combatant actor, Combatant target);
        public abstract int GetProtection(Combatant actor, Combatant target);
        public abstract List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        );

        protected List<Combatant> GetEnemies(Combatant actor, List<Combatant> combatants) =>
            combatants.Where(c => c.Side != actor.Side).ToList();

        protected List<Combatant> GetAllies(Combatant actor, List<Combatant> combatants) =>
            combatants.Where(c => c.Side == actor.Side).ToList();

        public abstract IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        );

        protected IEnumerable<GameEventBase> DealDamage(
            Combatant actor,
            Combatant target,
            int baseDamage
        )
        {
            foreach (var condition in actor.Conditions)
                baseDamage += condition.GetDamageFlatModifier();

            var multiplier = 1m;
            foreach (var condition in actor.Conditions)
                multiplier *= condition.GetDamageMultiplier();
            foreach (var condition in target.Conditions)
                multiplier *= condition.GetIncomingDamageMultiplier();

            var actualDamage = Convert.ToInt32(
                Math.Round(baseDamage * multiplier, 0, MidpointRounding.AwayFromZero)
            );

            foreach (var condition in target.Conditions)
                actualDamage += condition.GetIncomingDamageFlatModifier(actualDamage);

            var gameEvents = new List<GameEventBase>
            {
                new HpReducedGameEvent(target, actualDamage),
            };
            target.Hp -= actualDamage;

            // Reactions run once the blow has landed, so a counter-attack can depend on
            // the damage that was actually taken. A snapshot guards against a reaction
            // that spends itself and drops off the list mid-pass.
            foreach (var condition in target.Conditions.ToList())
                gameEvents.AddRange(condition.ReactToIncomingDamage(target, actor, actualDamage));

            return gameEvents;
        }
    }
}
