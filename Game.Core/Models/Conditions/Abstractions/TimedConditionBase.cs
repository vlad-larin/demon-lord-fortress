using System;

namespace GameCore.Models.Conditions.Abstractions
{
    /// <summary>
    /// A condition whose only state is how much longer it lasts. Inflict and prolong these
    /// through <see cref="GameCore.Extensions.CombatantConditionExtensions.ApplyForRounds"/>
    /// so a combatant never ends up carrying two of the same kind.
    /// </summary>
    public abstract class TimedConditionBase : ConditionBase
    {
        private bool _grantedThisRound;

        public int RoundsLeft { get; private set; }

        internal void AddRounds(int rounds)
        {
            if (rounds <= 0)
                return;

            _grantedThisRound = _grantedThisRound || RoundsLeft == 0;
            RoundsLeft += rounds;
        }

        /// <summary>
        /// Rounds are spent at the end of a round, but never the round the condition was
        /// granted in. Otherwise a combatant who happened to act before being stunned would
        /// shrug the stun off before it ever reached one of their intents, and how long a
        /// condition really lasted would depend on where its victim sat in the turn order.
        /// </summary>
        public void Decay()
        {
            if (_grantedThisRound)
            {
                _grantedThisRound = false;
                return;
            }

            RoundsLeft--;
        }

        public override bool ShouldBeRemoved() => RoundsLeft <= 0;
    }
}
