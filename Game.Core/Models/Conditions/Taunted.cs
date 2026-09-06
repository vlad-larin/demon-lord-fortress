using System.Collections.Generic;
using System.Linq;
using GameCore.Extensions;
using GameCore.Models.Conditions.Abstractions;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies.Helpers;

namespace GameCore.Models.Conditions
{
    public class Taunted : TimedConditionBase
    {
        public Combatant TauntedBy { get; private set; }

        public Taunted(Combatant tauntedBy, int tauntRounds)
        {
            TauntedBy = tauntedBy;
            AddRounds(tauntRounds);
        }

        /// <summary>
        /// The latest provocation is the one that sticks, keeping whichever rage lasts longer.
        /// </summary>
        internal void Retaunt(Combatant tauntedBy, int tauntRounds)
        {
            TauntedBy = tauntedBy;
            if (tauntRounds > RoundsLeft)
                AddRounds(tauntRounds - RoundsLeft);
        }

        public override IEnumerable<GameEventBase> UpdateIntentBeforeExecution(CombatIntent intent)
        {
            var gameEvents = new List<GameEventBase>();

            if (intent.Target == TauntedBy)
            {
                // Already targeting the taunting characted - do nothing.
                return gameEvents;
            }

            if (intent.Actor.HasCondition<Stunned>())
            {
                // Someone already took this action away. Rage cannot give it back.
                return gameEvents;
            }

            // The planned attack simply changes hands.
            if (intent.Action.GetDamage(intent.Actor, TauntedBy) > 0)
            {
                gameEvents.Add(
                    new SimpleGameEvent(
                        $"{intent.Actor.Class} is taunted by {TauntedBy.Class} and changes the target!"
                    )
                );
                intent.Target = TauntedBy;
                return gameEvents;
            }

            // Anything else - a heal, a ward, protecting an ally - is abandoned for a swing
            // at the provocateur, which is what makes a taunt worth spending an action on.
            if (!intent.Actor.Actions.Any(a => a.GetDamage(intent.Actor, TauntedBy) > 0))
            {
                gameEvents.Add(
                    new SimpleGameEvent(
                        $"{intent.Actor.Class} is enraged, but has no way to strike back"
                    )
                );
                return gameEvents;
            }

            var retaliation = AttackCalculator.FindStrongestAttack(
                intent.Actor,
                new Combatant[] { TauntedBy }
            );
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{intent.Actor.Class} abandons the plan to punish the {TauntedBy.Class}"
                )
            );
            intent.Action = retaliation.Action;
            intent.Target = TauntedBy;

            return gameEvents;
        }

        public override bool ShouldBeRemoved() => base.ShouldBeRemoved() || TauntedBy.Hp <= 0;
    }
}
