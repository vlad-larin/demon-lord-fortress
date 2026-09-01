using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Interfaces;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies;
using GameCore.PlayerActions;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    internal class ComposeHeroPartyPlanActionHandler
        : PlayerActionHandler<ComposeHeroPartyPlanAction>
    {
        private static readonly Random _rnd = new Random();

        public ComposeHeroPartyPlanActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(
            ComposeHeroPartyPlanAction playerAction
        )
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
            return new PlayerActionResult(GameInstance, gameEvents);
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
