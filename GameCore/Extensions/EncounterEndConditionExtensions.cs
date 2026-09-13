using GameCore.Models;

namespace GameCore.Extensions
{
    public static class EncounterEndConditionExtensions
    {
        /// <summary>
        /// The outcome of the first end condition that has been met, or null while the
        /// encounter goes on. Conditions are asked in the order the encounter lists them, so
        /// a scenario decides which one speaks first when several could fire at once.
        /// </summary>
        public static EncounterOutcome CheckEndConditions(this Encounter encounter)
        {
            foreach (var endCondition in encounter.EndConditions)
            {
                var outcome = endCondition.Check(encounter);
                if (outcome != null)
                    return outcome;
            }

            return null;
        }
    }
}
