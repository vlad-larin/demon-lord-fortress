using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Interfaces;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies;

namespace GameEngine.Ai
{
    public class EncounterAi
    {
        private GameInstance GameInstance { get; }

        private static readonly Random _rnd = new Random();

        public EncounterAi(GameInstance gameInstance)
        {
            GameInstance = gameInstance;
        }

        internal IEnumerable<GameEventBase> SetHeroPartyBattlePlan()
        {
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new SimpleGameEvent("Party starts planning"));

            var encounter = GameInstance.Encounter;
            var strategies = new List<IHeroPartyStrategy>
            {
                new Focus(encounter),
                new Defensive(encounter),
                new Pressure(encounter),
            };
            var strategy = ChooseRandomlyConsideringWeight(strategies);
            gameEvents.Add(strategy.GetPlanApprovalEvent());

            strategy.SetHeroPartyIntents();

            encounter.Phase = EncounterPhase.Planning;
            encounter.HeroPartyStrategy = strategy;
            return gameEvents;
        }

        private IHeroPartyStrategy ChooseRandomlyConsideringWeight(
            List<IHeroPartyStrategy> strategies
        )
        {
            var weights = strategies
                .Select(strategy =>
                    (Weight: strategy.CalculateDecisionWeight(), Strategy: strategy)
                )
                .ToList();

            var summaryWeight = weights.Sum(w => w.Weight);
            var randomWeight = _rnd.Next(summaryWeight);
            foreach (var (weight, strategy) in weights)
            {
                if (randomWeight < weight)
                    return strategy;
                else
                    randomWeight = randomWeight - weight;
            }

            throw new InvalidOperationException("Error with choosing the strategy!");
        }
    }
}
