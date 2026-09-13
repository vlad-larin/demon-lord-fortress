namespace GameCore.Models.EncounterEndConditions.Abstractions
{
    /// <summary>
    /// A reason for an encounter to stop. Each encounter carries its own list of these, so a
    /// scenario decides what "over" means for it: everyone on a side down, an objective
    /// reached, a side withdrawing.
    /// </summary>
    public abstract class EncounterEndConditionBase
    {
        /// <summary>
        /// Return the outcome when this condition has been met, or null while the fight goes on.
        /// Called both in the middle of a round, after every executed intent, and once the
        /// round is resolved - so a condition that can only be judged at the end of a round
        /// should say so by returning null until then.
        /// </summary>
        public abstract EncounterOutcome Check(Encounter encounter);
    }
}
