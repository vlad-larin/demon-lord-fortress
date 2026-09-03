using System;
using GameCore.Models;
using GameCore.PlayerActions;
using GameEngine.Ai;
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
            var ai = new EncounterAi(GameInstance);
            var gameEvents = ai.SetHeroPartyBattlePlan();
            return new PlayerActionResult(GameInstance, gameEvents);
        }
    }
}
