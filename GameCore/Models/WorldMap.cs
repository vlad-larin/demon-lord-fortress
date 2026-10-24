using System.Collections.Generic;
using System.Linq;

namespace GameCore.Models
{
    /// <summary>
    /// The kingdom around the tower: its locations and the roads between them. Not every
    /// location has a road to every other, so reaching a place is a matter of which way you
    /// go - the roads form a planar graph, laid out so that none of them cross.
    /// <see cref="Helpers.WorldMapValidation"/> holds a map to that.
    /// </summary>
    public class WorldMap
    {
        public List<Location> Locations { get; set; } = new List<Location>();

        public List<Road> Roads { get; set; } = new List<Road>();

        /// <summary>
        /// The Demon Lord's own seat. Every map has exactly one.
        /// </summary>
        public Location Tower => Locations.Single(location => location.Type == LocationType.Tower);

        public IEnumerable<Location> GetNeighbors(Location location) =>
            Roads.Where(road => road.Connects(location)).Select(road => road.OtherEnd(location));
    }
}
