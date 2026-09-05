using System;
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

        /// <summary>
        /// Default damage multiplier is 1 - condition does not change the amount of incoming damage.
        /// Override in the conditions that modify that amount;
        /// </summary>
        public virtual decimal GetIncomingDamageMultiplier() => 1;
    }
}
