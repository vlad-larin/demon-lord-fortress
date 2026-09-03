using System.Collections.Generic;
using System.Linq;
using GameCore.Interfaces;
using GameCore.Models.CombatActions;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies.Abstractions;

namespace GameCore.Models.HeroPartyStrategies
{
    public class Focus : HeroPartyStrategyBase, IHeroPartyStrategy
    {
        public Combatant PriorityTarget { get; set; }

        public Focus(Encounter encounter)
            : base(encounter) { }

        public GameEventBase GetPlanApprovalEvent() =>
            new SimpleGameEvent("Let's focus on one enemy!");

        public int CalculateDecisionWeight()
        {
            var weight = 30;

            if (AtLeastOneEnemyIsNearlyDead())
                weight += 20;

            if (OneEnemyIsVeryDangerous())
                weight += 15;

            if (CanExposeTheMostDangerousEnemy())
                weight += 10;

            return weight;
        }

        private bool AtLeastOneEnemyIsNearlyDead()
        {
            return GetEnemies().Any(enemy => enemy.Hp <= 4);
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

        private bool CanExposeTheMostDangerousEnemy()
        {
            return GetParty().Any(hero => hero.Class == CharacterClass.Rogue);
        }

        public IEnumerable<GameEventBase> SetHeroPartyIntents()
        {
            var events = new List<GameEventBase>();
            var intents = new List<CombatIntent>();
            var heroes = GetParty();
            var monsters = GetEnemies();

            PriorityTarget = null;

            var mostDangerousEnemy = MostDangerousEnemy();
            var mostDangerousEnemyKillingBlow = GetKillingBlowForEnemy(mostDangerousEnemy, heroes);
            if (mostDangerousEnemyKillingBlow != null)
            {
                PriorityTarget = mostDangerousEnemy;
                intents.Add(
                    new CombatIntent(
                        actor: mostDangerousEnemyKillingBlow?.Actor,
                        action: mostDangerousEnemyKillingBlow?.Action,
                        target: mostDangerousEnemy
                    )
                );
                heroes.Remove(mostDangerousEnemyKillingBlow?.Actor);
            }

            if (PriorityTarget == null)
            {
                var lowestHpEnemy = LowestHpEnemy();
                var lowestHpEnemyKillingBlow = GetKillingBlowForEnemy(lowestHpEnemy, heroes);
                if (lowestHpEnemyKillingBlow != null)
                {
                    PriorityTarget = lowestHpEnemy;
                    intents.Add(
                        new CombatIntent(
                            actor: lowestHpEnemyKillingBlow?.Actor,
                            action: lowestHpEnemyKillingBlow?.Action,
                            target: lowestHpEnemy
                        )
                    );
                    heroes.Remove(lowestHpEnemyKillingBlow?.Actor);
                }
            }

            PriorityTarget ??= mostDangerousEnemy;
            foreach (var hero in heroes.ToArray())
            {
                var bestAttack = FindStrongestAttack(hero, PriorityTarget);
                if (bestAttack != default)
                {
                    intents.Add(
                        new CombatIntent(
                            actor: hero,
                            action: bestAttack.Action,
                            target: PriorityTarget
                        )
                    );
                    heroes.Remove(hero);
                }
            }

            var allPairs = heroes.SelectMany(hero => monsters, (hero, monster) => (hero, monster));
            foreach ((var hero, var monster) in allPairs)
            {
                var bestAttack = FindStrongestAttack(hero, monster);
                if (bestAttack != default)
                {
                    intents.Add(
                        new CombatIntent(actor: hero, action: bestAttack.Action, target: monster)
                    );
                    heroes.Remove(hero);
                }
            }

            foreach (var hero in heroes.ToArray())
            {
                events.Add(new SimpleGameEvent($"{hero.Class} could not find a good attack"));
            }

            Encounter.Intents = intents;

            return events;
        }

        private Combatant MostDangerousEnemy() =>
            GetEnemies().OrderByDescending(enemy => enemy.PerceivedDanger).First();

        private Combatant LowestHpEnemy() => GetEnemies().OrderBy(enemy => enemy.Hp).First();

        private static (CombatActionBase Action, int Damage) FindStrongestAttack(
            Combatant hero,
            Combatant monster
        ) =>
            hero
                .Actions.Select(action =>
                    (@Action: action, Damage: action.GetDamage(hero, monster))
                )
                .Where(x => x.Damage > 0)
                .OrderByDescending(x => x.Damage)
                .FirstOrDefault();

        private (Combatant Actor, CombatActionBase Action)? GetKillingBlowForEnemy(
            Combatant target,
            IEnumerable<Combatant> actors
        )
        {
            var killingBlows = new List<(Combatant Actor, CombatActionBase Action)>();
            foreach (var actor in actors)
            {
                foreach (var action in actor.Actions)
                {
                    var damage = action.GetDamage(actor, target);
                    if (damage > 0 && damage > target.Hp)
                    {
                        killingBlows.Add((actor, action));
                    }
                }
            }
            return killingBlows.Count == 0
                ? ((Combatant Actor, CombatActionBase Action)?)null
                : killingBlows[Rnd.Next(killingBlows.Count)];
        }

        public void RetargetAction(CombatIntent intent)
        {
            var mostDangerousEnemy = MostDangerousEnemy();
            var mostDangerousEnemyKillingBlow = GetKillingBlowForEnemy(
                mostDangerousEnemy,
                new Combatant[] { intent.Actor }
            );
            if (mostDangerousEnemyKillingBlow != null)
            {
                intent.Action = mostDangerousEnemyKillingBlow?.Action;
                intent.Target = mostDangerousEnemy;
                return;
            }

            var lowestHpEnemy = LowestHpEnemy();
            var lowestHpEnemyKillingBlow = GetKillingBlowForEnemy(
                lowestHpEnemy,
                new Combatant[] { intent.Actor }
            );
            if (lowestHpEnemyKillingBlow != null)
            {
                intent.Action = lowestHpEnemyKillingBlow?.Action;
                intent.Target = lowestHpEnemy;
                return;
            }

            var bestAttack = FindStrongestAttack(intent.Actor, lowestHpEnemy);
            if (bestAttack != default)
            {
                intent.Action = bestAttack.Action;
                intent.Target = lowestHpEnemy;
                return;
            }

            intent.Action = new Wait();
            intent.Target = null;
        }
    }
}
