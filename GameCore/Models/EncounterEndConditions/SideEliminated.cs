using System.Linq;
using GameCore.Models.EncounterEndConditions.Abstractions;

namespace GameCore.Models.EncounterEndConditions
{
    /// <summary>
    /// The plain one: a side with nobody left standing cannot go on fighting.
    /// </summary>
    public class SideEliminated : EncounterEndConditionBase
    {
        public override EncounterOutcome Check(Encounter encounter)
        {
            var heroesStanding = encounter.Combatants.Any(c => c.Side == ConflictSide.Heroes);
            var demonLordStanding = encounter.Combatants.Any(c => c.Side == ConflictSide.DemonLord);

            if (heroesStanding && demonLordStanding)
                return null;

            if (!heroesStanding && !demonLordStanding)
                return new EncounterOutcome
                {
                    Winner = ConflictSide.Unknown,
                    Reason = "Both sides have fallen",
                };

            return heroesStanding
                ? new EncounterOutcome
                {
                    Winner = ConflictSide.Heroes,
                    Reason = "The tower's defenders have fallen",
                }
                : new EncounterOutcome
                {
                    Winner = ConflictSide.DemonLord,
                    Reason = "The hero party has fallen",
                };
        }
    }
}
