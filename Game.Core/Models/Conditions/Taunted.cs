using System.Collections.Generic;
using GameCore.Models.Conditions.Abstractions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.Conditions
{
    public class Taunted : ConditionBase
    {
        public Combatant TauntedBy { get; private set; }

        public Taunted(Combatant tauntedBy)
        {
            TauntedBy = tauntedBy;
        }

        /// <summary>
        /// The latest provocation is the one that sticks.
        /// </summary>
        internal void Retaunt(Combatant tauntedBy)
        {
            TauntedBy = tauntedBy;
        }

        public override IEnumerable<GameEventBase> UpdateIntentBeforeExecution(CombatIntent intent)
        {
            var gameEvents = new List<GameEventBase>();
            if (intent.Target == TauntedBy)
            {
                // Already targeting the taunting characted - do nothing.
                return gameEvents;
            }

            if (intent.Action.GetDamage(intent.Actor, intent.Target) > 0)
            {
                gameEvents.Add(
                    new SimpleGameEvent(
                        $"{intent.Actor.Class} is taunted by {TauntedBy.Class} and changes the target!"
                    )
                );
                intent.Target = TauntedBy;
            }
            return gameEvents;
        }
    }
}
