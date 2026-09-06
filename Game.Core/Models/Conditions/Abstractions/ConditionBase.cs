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
        /// Override this in the conditions that change the incoming damage.
        /// </summary>
        public virtual decimal GetIncomingDamageMultiplier() => 1;

        /// <summary>
        /// Default modifier is 0 - condition does not change the amount of incoming damage.
        /// Override this in the conditions that change the incoming damage.
        /// </summary>
        public virtual int GetIncomingDamageFlatModifier(int actualDamage) => 0;

        /// <summary>
        /// Return true if you want the condition to be cleared at the end of the round.
        /// </summary>
        public virtual bool ShouldBeRemoved() => false;
    }
}
