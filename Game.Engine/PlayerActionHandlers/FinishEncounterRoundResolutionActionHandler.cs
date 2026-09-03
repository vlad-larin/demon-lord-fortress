using System.Collections.Generic;
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
            // TODO: Check if one side has won and go to the debriefing if yes
            var victory = CheckVictoryConditions();
            if (victory != null)
            {
                var gameEvents = new List<GameEventBase>();
                gameEvents.Add(new SimpleGameEvent("Victory condition achieved"));
                var encounter = GameInstance.Encounter;
                encounter.Phase = EncounterPhase.Debriefing;
                return new PlayerActionResult(GameInstance, gameEvents);
            }
            else
            {
                var ai = new EncounterAi(GameInstance);
                var gameEvents = ai.SetHeroPartyBattlePlan();
                return new PlayerActionResult(GameInstance, gameEvents);
            }
        }

        private object CheckVictoryConditions() => null; // placeholder
    }
}
