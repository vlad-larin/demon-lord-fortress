using System.Linq;
using GameCore.Models.RoomProperties.Abstractions;

namespace GameCore.Models.HeroObjectives.Abstractions
{
    /// <summary>
    /// What a hero party came to the tower to do. An expedition carries exactly one, and it
    /// is the party's compass: on every floor they look for the room that holds it, and the
    /// expedition is over the moment they take that room.
    /// </summary>
    public abstract class HeroObjectiveBase
    {
        /// <summary>
        /// The objective in a few words, the way a briefing would name it.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Why the party came, for the player reading the expedition sheet.
        /// </summary>
        public abstract string Briefing { get; }

        /// <summary>
        /// What the kingdom gets to say once the party pulls it off.
        /// </summary>
        public abstract string Achievement { get; }

        /// <summary>
        /// True for the one room the party is climbing towards. Answered from what the room
        /// holds rather than from who is guarding it, so the objective survives the fight:
        /// a throne room is still the throne room after its defenders are cut down.
        /// </summary>
        public abstract bool IsHeldBy(Room room);

        protected static bool Holds<TRoomProperty>(Room room)
            where TRoomProperty : RoomPropertyBase =>
            room.Properties.Any(property => property is TRoomProperty);
    }
}
