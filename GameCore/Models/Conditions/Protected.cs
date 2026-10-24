using System.Collections.Generic;
using GameCore.Models.Conditions.Abstractions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.Conditions
{
    public class Protected : TimedConditionBase
    {
        public Combatant ProtectedBy { get; private set; }

        public Protected(Combatant protectedBy, int protectRounds)
        {
            ProtectedBy = protectedBy;
            AddRounds(protectRounds);
        }

        /// <summary>
        /// The newest protection takes over: whoever stepped in last is the one standing
        /// there, and the longer of the two watches is how long they stand.
        /// </summary>
        public override void Absorb(ConditionBase incoming)
        {
            var renewal = (Protected)incoming;
            ProtectedBy = renewal.ProtectedBy;
            AddRounds(renewal.RoundsLeft - RoundsLeft);
        }

        public override IEnumerable<GameEventBase> UpdateIncomingIntentBeforeExecution(
            CombatIntent intent
        )
        {
            // Nobody who is out of the fight is still carrying this condition: a protector
            // who falls takes it with them the moment the clash they fell in is resolved.
            var gameEvents = new List<GameEventBase>();
            var ward = intent.Target;

            if (intent.Actor.Side == ward.Side)
            {
                // Heals, wards and other friendly attention reach the ward untouched.
                return gameEvents;
            }

            if (intent.Action.GetDamage(intent.Actor, ward) <= 0)
                return gameEvents;

            gameEvents.Add(
                new SimpleGameEvent(
                    $"{ProtectedBy.DisplayName} takes the blow meant for {ward.DisplayName}"
                )
            );
            intent.Target = ProtectedBy;

            return gameEvents;
        }

        public override bool ShouldBeRemoved() => base.ShouldBeRemoved() || ProtectedBy.Hp <= 0;
    }
}
