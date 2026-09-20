using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;

namespace GameCore.ObservableStates
{
    public class TowerClimbingState : ObservableStateBase
    {
        public Tower Tower { get; set; }

        /// <summary>
        /// The party climbing it: who they are, what they want and which floor they are
        /// standing on right now.
        /// </summary>
        public Expedition Expedition { get; set; }

        public TowerClimbingState(IEnumerable<GameEventBase> gameEvents)
            : base(gameEvents)
        {
            GameMode = GameMode.TowerClimbing;
        }
    }
}
