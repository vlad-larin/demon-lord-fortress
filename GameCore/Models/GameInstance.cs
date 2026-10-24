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

        /// <summary>
        /// The kingdom around the tower, or null in scenarios that never leave the tower.
        /// </summary>
        public WorldMap WorldMap { get; set; }

        /// <summary>
        /// The campaign turn on the world map, counted from one.
        /// </summary>
        public int Turn { get; set; }
    }
}
