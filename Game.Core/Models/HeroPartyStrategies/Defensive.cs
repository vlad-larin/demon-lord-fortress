using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameCore.Interfaces;
using GameCore.Models.CombatActions;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies.Abstractions;
using GameCore.Models.HeroPartyStrategies.Helpers;

namespace GameCore.Models.HeroPartyStrategies
{
    public class Defensive : HeroPartyStrategyBase, IHeroPartyStrategy
    {
        public Defensive(Encounter encounter)
            : base(encounter) { }

        public GameEventBase GetPlanApprovalEvent() => new SimpleGameEvent("Hold the line!");

        public int CalculateDecisionWeight()
        {
            var weight = 20;

            if (AtLeastOneHeroIsNearlyDead())
                weight += 20;

            if (OneEnemyIsVeryDangerous())
                weight += 15;

            if (MoreThanOneHeroHasLessThanHalfHp())
                weight += 10;

            return weight;
        }

        private bool AtLeastOneHeroIsNearlyDead()
        {
            return GetParty().Any(hero => hero.Hp <= 4);
        }

        private bool OneEnemyIsVeryDangerous()
        {
            var enemiesByPerceivedThreat = GetEnemies()
                .OrderByDescending(enemy => enemy.PerceivedDanger)
                .ToList();
            return enemiesByPerceivedThreat.Count > 1
                && enemiesByPerceivedThreat[0].PerceivedDanger
                    - enemiesByPerceivedThreat[1].PerceivedDanger
                    > enemiesByPerceivedThreat[1].PerceivedDanger;
        }

        private bool MoreThanOneHeroHasLessThanHalfHp()
        {
            return GetParty().Where(hero => hero.Hp < hero.MaxHp / 2).Count() > 1;
        }

        public IEnumerable<GameEventBase> SetHeroPartyIntents()
        {
            var events = new List<GameEventBase>();
            var intents = new List<CombatIntent>();
            var heroes = GetParty();
            var monsters = GetEnemies();

            // Find heroes that might be killed in one blow
            var heroesToProtect = GetHeroesInDanger();
            if (heroesToProtect.Count == 0)
            {
                // Find a weakest wounded hero to protect
                var lowestHpHero = LowestHpHero();
                if (lowestHpHero.Hp <= lowestHpHero.MaxHp / 2)
                {
                    heroesToProtect.Add(lowestHpHero);
                }
            }

            foreach (var heroToProtect in heroesToProtect)
            {
                var protectiveAction = FindProtectiveAction(heroToProtect, heroes);
                if (protectiveAction != null)
                {
                    intents.Add(
                        new CombatIntent(
                            actor: protectiveAction?.Actor,
                            action: protectiveAction?.Action,
                            target: heroToProtect
                        )
                    );
                    heroes.Remove(protectiveAction?.Actor);
                }
            }

            foreach (var hero in heroes.ToArray())
            {
                var bestAttack = AttackCalculator.FindStrongestAttack(hero, monsters);
                if (bestAttack != default)
                {
                    intents.Add(
                        new CombatIntent(
                            actor: hero,
                            action: bestAttack.Action,
                            target: bestAttack.Target
                        )
                    );
                    heroes.Remove(hero);
                }
            }

            foreach (var hero in heroes.ToArray())
            {
                events.Add(
                    new SimpleGameEvent($"{hero.Class} could not find a good defensive approach")
                );
            }

            Encounter.Intents = intents;

            return events;
        }

        private List<Combatant> GetHeroesInDanger()
        {
            var heroes = GetParty();
            var monsters = GetEnemies();

            var heroesInDanger = new List<Combatant>();

            foreach (var hero in heroes)
            {
                bool heroInDanger = false;
                foreach (var monster in monsters)
                {
                    foreach (var action in monster.Actions)
                    {
                        var damage = action.GetDamage(monster, hero);
                        if (damage >= hero.Hp)
                        {
                            heroInDanger = true;
                            break;
                        }
                    }
                }

                if (heroInDanger)
                {
                    heroesInDanger.Add(hero);
                }
            }

            return heroesInDanger;
        }

        private Combatant LowestHpHero() => GetParty().OrderBy(hero => hero.Hp).First();

        private (Combatant Actor, CombatActionBase Action)? FindProtectiveAction(
            Combatant target,
            IEnumerable<Combatant> actors
        )
        {
            var protectiveActions = new List<(Combatant Actor, CombatActionBase Action)>();
            foreach (var actor in actors)
            {
                foreach (var action in actor.Actions)
                {
                    var protection = action.GetProtection(actor, target);
                    if (protection > 0)
                    {
                        protectiveActions.Add((actor, action));
                    }
                }
            }
            return protectiveActions.Count == 0
                ? ((Combatant Actor, CombatActionBase Action)?)null
                : protectiveActions[Rnd.Next(protectiveActions.Count)];
        }
    }
}
