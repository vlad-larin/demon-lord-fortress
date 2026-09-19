namespace GameCore.Models.RoomProperties.Abstractions
{
    /// <summary>
    /// Something true about a room beyond the monsters standing in it: what it keeps, what
    /// it does to a fight, what it is worth to somebody climbing towards it. A room carries
    /// its own list of these, so the tower is described by what its rooms hold rather than
    /// by a growing set of flags on the room itself.
    /// </summary>
    public abstract class RoomPropertyBase { }
}
