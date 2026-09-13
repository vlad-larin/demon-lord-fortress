namespace GameCore.Models
{
    public class GameInstance
    {
        public GameMode GameMode { get; set; }

        public Tower Tower { get; set; }

        public Encounter Encounter { get; set; }
    }
}
