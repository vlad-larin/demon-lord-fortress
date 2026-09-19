using System.Collections.Generic;
using GameCore.Models.HeroObjectives.Abstractions;

namespace GameCore.Models
{
    /// <summary>
    /// One hero party's run at the tower: who walked in, what they came for and how far up
    /// they have got. It outlives the individual encounters, so the wounds a floor costs
    /// them are still there on the next one.
    /// </summary>
    public class Expedition
    {
        /// <summary>
        /// Everyone still standing. The dead are dropped as encounters end, so the party
        /// that reaches the throne room is whatever is left of the one that came in.
        /// </summary>
        public List<Combatant> Heroes { get; set; } = new List<Combatant>();

        public HeroObjectiveBase Objective { get; set; }

        /// <summary>
        /// The floor the party stands on, counted from one at the bottom.
        /// </summary>
        public int CurrentFloorNumber { get; set; } = 1;

        /// <summary>
        /// The room the party has broken into, while the fight for it lasts, and null
        /// between rooms.
        /// </summary>
        public Room CurrentRoom { get; set; }
    }
}
