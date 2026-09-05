using System.Collections.Generic;
using GameCore.Models.GameEvents;

namespace GameCore.Models.Conditions.Abstractions
{
    public abstract class ConditionBase
    {
        /// <summary>
        /// Default behavior is not to change the intent. But specific conditions can do this.
        /// </summary>
        public virtual IEnumerable<GameEventBase> UpdateIntentBeforeExecution(
            CombatIntent intent
        ) => new GameEventBase[] { };
    }
}
