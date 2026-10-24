namespace GameCore.Models
{
    /// <summary>
    /// A named place in the kingdom. Where it lies is part of the model rather than left to
    /// the renderer, because roads run as straight lines between locations and only a fixed
    /// layout can promise that no two of them cross.
    /// </summary>
    public class Location
    {
        public string Name { get; set; }

        public LocationType Type { get; set; }

        /// <summary>
        /// West to east. Only relative positions matter: whatever span the map covers is
        /// stretched over the screen.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// North to south, on the same terms as <see cref="X"/>.
        /// </summary>
        public int Y { get; set; }
    }
}
