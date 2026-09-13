using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Ai;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    internal class FinishEncounterRoundResolutionActionHandler
        : PlayerActionHandler<FinishEncounterRoundResolutionAction>
    {
        public FinishEncounterRoundResolutionActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(
            FinishEncounterRoundResolutionAction playerAction
        )
        {
            var encounter = GameInstance.Encounter;
            var gameEvents = new List<GameEventBase>();

            // The battle plan may have decided the encounter mid-round already; conditions
            // that can only be judged once a full round is behind them get their say here.
            var outcome = encounter.Outcome ?? encounter.CheckEndConditions();
            if (outcome != null)
            {
                encounter.Outcome = outcome;
                encounter.Phase = EncounterPhase.Debriefing;
                gameEvents.Add(new SimpleGameEvent($"The encounter is over. {outcome.Reason}"));
                return new PlayerActionResult(GameInstance, gameEvents);
            }

            var ai = new EncounterAi(GameInstance);
            gameEvents.AddRange(ai.SetHeroPartyBattlePlan());
            return new PlayerActionResult(GameInstance, gameEvents);
        }
    }
}
