using GameCore.Models.HeroObjectives.Abstractions;
using GameCore.Models.RoomProperties;

namespace GameCore.Models.HeroObjectives
{
    /// <summary>
    /// The objective that stops one floor short of the throne: the party climbs almost the
    /// whole tower, takes what it came for and leaves the Demon Lord alive above them.
    /// </summary>
    public class SeizeTheForbiddenLore : HeroObjectiveBase
    {
        public override string Name => "Seize the forbidden lore";

        public override string Briefing =>
            "The order wants to know where his power comes from before anyone tries the throne room again.";

        public override string Achievement =>
            "The Demon Lord's own writings are carried out of the tower";

        public override bool IsHeldBy(Room room) => Holds<HoldsTheForbiddenLore>(room);
    }
}
