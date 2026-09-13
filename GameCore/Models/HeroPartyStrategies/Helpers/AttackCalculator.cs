using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models.CombatActions;

namespace GameCore.Models.HeroPartyStrategies.Helpers
{
    internal static class AttackCalculator
    {
        private static readonly Random Rnd = new Random();

        internal static (CombatActionBase Action, int Damage, Combatant Target) FindStrongestAttack(
            Combatant hero,
            IEnumerable<Combatant> monsters
        )
        {
            var attacks = hero
                .Actions.SelectMany(
                    action => monsters,
                    (action, monster) => (Action: action, Monster: monster)
                )
                .Select(possibleAction =>
                    (
                        Action: possibleAction.Action,
                        Damage: possibleAction.Action.GetDamage(hero, possibleAction.Monster),
                        Target: possibleAction.Monster
                    )
                )
                .Where(x => x.Damage > 0)
                .OrderByDescending(x => x.Damage);

            var bestAttackDamage = attacks.FirstOrDefault().Damage;
            var bestAttacks = attacks.Where(x => x.Damage == bestAttackDamage).ToList();

            return bestAttacks[Rnd.Next(bestAttacks.Count)];
        }
    }
}
