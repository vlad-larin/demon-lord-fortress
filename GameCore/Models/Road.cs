using System;

namespace GameCore.Models
{
    /// <summary>
    /// A way between two locations, travelled in either direction. Which end is first means
    /// nothing.
    /// </summary>
    public class Road
    {
        public Location First { get; set; }

        public Location Second { get; set; }

        public bool Connects(Location location) => First == location || Second == location;

        public Location OtherEnd(Location location) =>
            location == First ? Second
            : location == Second ? First
            : throw new InvalidOperationException(
                $"[Road] {location.Name} is not on the road between {First.Name} and {Second.Name}"
            );
    }
}
