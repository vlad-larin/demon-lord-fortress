namespace GameCore.Models
{
    public class GameInstance
    {
        public GameMode GameMode { get; set; }

        public Tower Tower { get; set; }

        /// <summary>
        /// The hero party currently inside the tower, or null when nobody is climbing it.
        /// </summary>
        public Expedition Expedition { get; set; }

        public Encounter Encounter { get; set; }
    }
}
