using System.Collections.Generic;
using System.Linq;
using GameCore.Models.RoomProperties.Abstractions;

namespace GameCore.Models
{
    public class Room
    {
        public string Type { get; set; }
        public int Capacity { get; set; }
        public List<Combatant> Guardians { get; set; } = new List<Combatant>();
        public List<RoomPropertyBase> Properties { get; set; } = new List<RoomPropertyBase>();

        /// <summary>
        /// Set once a hero party has fought its way through this room. A floor gives up one
        /// room per expedition, so a cleared room is out of the running for the rest of it.
        /// </summary>
        public bool Cleared { get; set; }

        /// <summary>
        /// What the room looks like it will cost to walk into: the danger the party reads
        /// off the guardians standing in it. Nothing hidden counts - this is the number the
        /// heroes weigh, not the one the Demon Lord knows.
        /// </summary>
        public int PerceivedThreat => Guardians.Sum(guardian => guardian.PerceivedDanger);
    }
}
