using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Exposed : ConditionBase
    {
        public int RoundsLeft { get; private set; }

        public Exposed(int roundsLeft)
        {
            RoundsLeft = roundsLeft;
        }

        internal void AddRounds(int rounds)
        {
            RoundsLeft += rounds;
        }
    }
}
