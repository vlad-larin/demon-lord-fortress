using System;
using GameCore.Interfaces;
using GameCore.Models;

namespace GameCore.PlayerActions
{
    public class InsertCombatActionIntoBattlePlan : IPlayerAction
    {
        public CombatIntent Intent { get; private set; }
        public int Index { get; private set; }
        public Guid ActorId { get; private set; }
        public Guid TargetId { get; private set; }

        public InsertCombatActionIntoBattlePlan(
            CombatIntent intent,
            int index,
            Guid actorId,
            Guid targetId
        )
        {
            Intent = intent;
            Index = index;
            ActorId = actorId;
            TargetId = targetId;
        }
    }
}
