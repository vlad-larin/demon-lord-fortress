using GameCore.Models;
using GameCore.Models.GameEvents;
using GameCore.PlayerActions;
using GameEngine.Models;
using GameEngine.PlayerActionHandlers.Abstractions;

namespace GameEngine.PlayerActionHandlers
{
    internal class FinishEncounterDebriefingActionHandler
        : PlayerActionHandler<FinishEncounterDebriefingAction>
    {
        public FinishEncounterDebriefingActionHandler(GameInstance gameInstance)
            : base(gameInstance) { }

        public override PlayerActionResult HandlePlayerAction(
            FinishEncounterDebriefingAction playerAction
        )
        {
            // Where the expedition picks up again: a surviving hero party climbs on to the
            // next room, and a broken one hands the tower back to the global map. Neither
            // mode is playable yet, so the debriefing returns the player to the title screen.
            GameInstance.GameMode = GameMode.Title;
            GameInstance.Encounter = null;

            var gameEvents = new GameEventBase[]
            {
                new SimpleGameEvent("The encounter is concluded"),
            };
            return new PlayerActionResult(GameInstance, gameEvents);
        }
    }
}
