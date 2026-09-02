using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models.Conditions
{
    public class Distracted : ConditionBase
    {
        public int RoundsLeft { get; private set; }

        public Distracted(int roundsLeft)
        {
            RoundsLeft = roundsLeft;
        }

        internal void AddRounds(int rounds)
        {
            RoundsLeft += rounds;
        }
    }
}
