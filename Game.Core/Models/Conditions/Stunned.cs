using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Stunned : ConditionBase
    {
        public int RoundsLeft { get; private set; }

        public Stunned(int roundsLeft)
        {
            RoundsLeft = roundsLeft;
        }

        internal void AddRounds(int rounds)
        {
            RoundsLeft += rounds;
        }
    }
}
