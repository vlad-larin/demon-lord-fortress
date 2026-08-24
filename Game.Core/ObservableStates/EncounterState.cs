using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;

namespace GameCore.ObservableStates
{
    public class EncounterState : ObservableStateBase
    {
        public Encounter Encounter { get; set; }

        public EncounterState(IEnumerable<GameEventBase> gameEvents)
            : base(gameEvents)
        {
            GameMode = GameMode.Encounter;
        }
    }
}
