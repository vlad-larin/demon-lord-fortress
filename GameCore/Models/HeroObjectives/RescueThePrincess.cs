using GameCore.Models.HeroObjectives.Abstractions;
using GameCore.Models.RoomProperties;

namespace GameCore.Models.HeroObjectives
{
    /// <summary>
    /// A short expedition with a hard limit on it: the party wants one room, and once they
    /// have her they have no reason left to climb.
    /// </summary>
    public class RescueThePrincess : HeroObjectiveBase
    {
        public override string Name => "Rescue the princess";

        public override string Briefing =>
            "The crown wants its daughter back alive. Everything above her cell is somebody else's problem.";

        public override string Achievement => "The princess is carried out of the fortress alive";

        public override bool IsHeldBy(Room room) => Holds<HoldsThePrincess>(room);
    }
}
