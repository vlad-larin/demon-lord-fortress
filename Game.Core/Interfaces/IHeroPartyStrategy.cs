using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.GameEvents;

namespace GameCore.Interfaces
{
    public interface IHeroPartyStrategy
    {
        GameEventBase GetPlanApprovalEvent();

        int CalculateDecisionWeight();

        IEnumerable<GameEventBase> SetHeroPartyIntents();
    }
}
