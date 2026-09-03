namespace GameCore.Models.Conditions.Abstractions
{
    /// <summary>
    /// A condition whose only state is how much longer it lasts. Inflict and prolong these
    /// through <see cref="GameCore.Extensions.CombatantConditionExtensions.ApplyForRounds"/>
    /// so a combatant never ends up carrying two of the same kind.
    /// </summary>
    public abstract class TimedConditionBase : ConditionBase
    {
        public int RoundsLeft { get; private set; }

        internal void AddRounds(int rounds)
        {
            RoundsLeft += rounds;
        }
    }
}
