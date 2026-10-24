using System;
using GameCore.Interfaces;
using GameCore.Models;

namespace GameCore.PlayerActions
{
    public class InsertCombatActionIntoBattlePlan : IPlayerAction
    {
        public CombatIntent Intent { get; private set; }

        /// <summary>
        /// Which clash of the battle plan the monster acts in, or the plan's own length for
        /// a clash of its own after everything the heroes have planned.
        ///
        /// Those are the only two choices on purpose. A monster can step into a clash the
        /// party already opened, or wait until the party's plan has run out - it can never
        /// wedge a clash in ahead of one. Together with one monster to a clash, that keeps
        /// the round interleaved: the warband cannot charge as a body and leave whatever
        /// heroes survive to act afterwards, so almost everyone gets their moment, and the
        /// player's real decision is which hero's intention to stand in front of.
        ///
        /// A clash that is already taken is not refused: the monster standing there is left
        /// without orders and can be given new ones, so changing your mind about who meets
        /// which hero is one decision rather than two.
        /// </summary>
        public int StepIndex { get; private set; }

        public Guid ActorId { get; private set; }
        public Guid TargetId { get; private set; }

        public InsertCombatActionIntoBattlePlan(
            CombatIntent intent,
            int stepIndex,
            Guid actorId,
            Guid targetId
        )
        {
            Intent = intent;
            StepIndex = stepIndex;
            ActorId = actorId;
            TargetId = targetId;
        }
    }
}
