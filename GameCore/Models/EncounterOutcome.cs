using System.Collections.Generic;

namespace GameCore.Models
{
    /// <summary>
    /// How an encounter ended and what it left behind. Filled in by the end condition that
    /// was met and shown to the player during the debriefing phase.
    /// </summary>
    public class EncounterOutcome
    {
        /// <summary>
        /// The side that came out on top, or <see cref="ConflictSide.Unknown"/> when nobody
        /// did - a mutual wipe, or a fight both sides walked away from.
        /// </summary>
        public ConflictSide Winner { get; set; } = ConflictSide.Unknown;

        /// <summary>
        /// Why the fighting stopped, in the words of the end condition that was met.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// What the encounter changed beyond the battlefield: a room plundered, an objective
        /// reached, hate collected, loot carried off. Nothing writes here yet - the debriefing
        /// already renders whatever it finds, so the systems that produce consequences only
        /// have to add their lines.
        /// </summary>
        public List<string> Consequences { get; set; } = new List<string>();
    }
}
