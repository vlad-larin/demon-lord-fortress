using GameCore.Models.HeroObjectives.Abstractions;
using GameCore.Models.RoomProperties;

namespace GameCore.Models.HeroObjectives
{
    /// <summary>
    /// The one objective that cannot be steered away from: it lives in the throne room, at
    /// the top of the tower, and the party has to pay for every floor on the way there.
    /// </summary>
    public class SlayTheDemonLord : HeroObjectiveBase
    {
        public override string Name => "Slay the Demon Lord";

        public override string Briefing =>
            "The kingdom has stopped bargaining. The party is here to end him on his own throne.";

        public override string Achievement => "The Demon Lord lies dead on his own throne";

        public override bool IsHeldBy(Room room) => Holds<HoldsTheThrone>(room);
    }
}
