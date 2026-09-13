using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;

namespace GameCore.ObservableStates
{
    public class TowerClimbingState : ObservableStateBase
    {
        public Tower Tower { get; set; }

        public TowerClimbingState(IEnumerable<GameEventBase> gameEvents)
            : base(gameEvents)
        {
            GameMode = GameMode.TowerClimbing;
        }
    }
}
