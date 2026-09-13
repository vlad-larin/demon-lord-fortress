using System.Collections.Generic;
using System.Linq;
using GameCore.Interfaces;
using GameCore.Models.CombatActions;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies.Abstractions;
using GameCore.Models.HeroPartyStrategies.Helpers;

namespace GameCore.Models.HeroPartyStrategies
{
    public class Pressure : HeroPartyStrategyBase, IHeroPartyStrategy
    {
        public Pressure(Encounter encounter)
            : base(encounter) { }

        public GameEventBase GetPlanApprovalEvent() => new SimpleGameEvent("Give them hell!");

        public int CalculateDecisionWeight()
        {
            var weight = 30;

            if (HeroesHaveNumericalAdvantage())
                weight += 20;

            //if (EnemyIsWeakened())
            //    weight += 15;

            if (EnemiesHaveLowAverageHp())
                weight += 10;

            return weight;
        }

        private bool HeroesHaveNumericalAdvantage()
        {
            return GetEnemies().Count() < GetParty().Count();
        }

        private bool EnemiesHaveLowAverageHp()
        {
            var totalHp = GetEnemies().Sum(enemy => enemy.Hp);
            var maxHp = GetEnemies().Sum(enemy => enemy.MaxHp);
            return totalHp <= maxHp / 2;
        }

        public IEnumerable<GameEventBase> SetHeroPartyIntents()
        {
            var events = new List<GameEventBase>();
            var intents = new List<CombatIntent>();
            var heroes = GetParty();
            var monsters = GetEnemies();

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
                    new SimpleGameEvent($"{hero.Class} could not find a good pressure approach")
                );
            }

            Encounter.Intents = intents;

            return events;
        }

        public void RetargetAction(CombatIntent intent)
        {
            var bestAttack = AttackCalculator.FindStrongestAttack(intent.Actor, GetEnemies());
            if (bestAttack != default)
            {
                intent.Action = bestAttack.Action;
                intent.Target = bestAttack.Target;
                return;
            }

            intent.Action = new Wait();
            intent.Target = null;
        }
    }
}
