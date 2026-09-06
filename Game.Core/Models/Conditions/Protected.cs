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
        /// The newest protection takes over.
        /// </summary>
        internal void RenewProtection(Combatant protectedBy, int protectRounds)
        {
            ProtectedBy = protectedBy;
            AddRounds(protectRounds - RoundsLeft);
        }

        public override IEnumerable<GameEventBase> UpdateIncomingIntentBeforeExecution(
            CombatIntent intent
        )
        {
            var gameEvents = new List<GameEventBase>();
            var ward = intent.Target;

            if (intent.Actor.Side == ward.Side)
            {
                // Heals, wards and other friendly attention reach the ward untouched.
                return gameEvents;
            }

            if (intent.Action.GetDamage(intent.Actor, ward) <= 0)
                return gameEvents;

            if (ward.Hp <= 0 || ProtectedBy.Hp <= 0)
            {
                // Nobody left to shield, or nobody left to do the shielding. A blow aimed at
                // a fallen ward is the caller's problem to retarget, the same as any other.
                return gameEvents;
            }

            gameEvents.Add(
                new SimpleGameEvent($"{ProtectedBy.Class} takes the blow meant for {ward.Class}")
            );
            intent.Target = ProtectedBy;

            return gameEvents;
        }

        public override bool ShouldBeRemoved() => base.ShouldBeRemoved() || ProtectedBy.Hp <= 0;
    }
}
