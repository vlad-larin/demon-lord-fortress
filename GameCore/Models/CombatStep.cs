using System.Collections.Generic;
using System.Linq;

namespace GameCore.Models
{
    /// <summary>
    /// One clash of the battle plan. Everything inside it happens at the same moment: every
    /// intent is carried out against the board as it stood when the clash opened, then the
    /// conditions it inflicted come into force, then whoever ran out of hit points falls.
    /// That is what lets a monster the heroes open on still get its blow in, instead of
    /// being cut down before its own intent is ever reached.
    /// </summary>
    public class CombatStep
    {
        public List<CombatIntent> Intents { get; set; } = new List<CombatIntent>();

        /// <summary>
        /// The one monster acting in this clash, or null while it is still free. A clash
        /// never holds two - a monster sent into a taken one pushes out whoever was there -
        /// and a monster can only step into a clash the party opened or wait past the end of
        /// their plan. Both halves of the same rule, which keeps the round interleaved
        /// instead of letting the warband charge in a single body. See
        /// <see cref="GameCore.PlayerActions.InsertCombatActionIntoBattlePlan.StepIndex"/>.
        /// </summary>
        public CombatIntent MonsterIntent =>
            Intents.FirstOrDefault(intent => intent.Actor.Side == ConflictSide.DemonLord);

        public CombatStep() { }

        public CombatStep(CombatIntent intent)
        {
            Intents.Add(intent);
        }
    }
}
