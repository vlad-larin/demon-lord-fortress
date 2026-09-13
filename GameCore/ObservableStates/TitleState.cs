using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;

namespace GameCore.ObservableStates
{
    public class TitleState : ObservableStateBase
    {
        public string[] NewScenarioNames { get; set; }

        public TitleState(IEnumerable<GameEventBase> gameEvents)
            : base(gameEvents)
        {
            GameMode = GameMode.Title;
        }
    }
}
