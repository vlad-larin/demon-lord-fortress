using System.Collections.Generic;
using GameCore.Models.CombatActions;
using GameCore.Models.Conditions.Abstractions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.Conditions
{
    public class Stunned : TimedConditionBase
    {
        public override IEnumerable<GameEventBase> UpdateIntentBeforeExecution(CombatIntent intent)
        {
            if (intent.Action is Wait)
                return new GameEventBase[0];

            intent.Action = new Wait();
            intent.Target = null;
            return new GameEventBase[]
            {
                new SimpleGameEvent($"{intent.Actor.Class} is stunned and can not act"),
            };
        }
    }
}
