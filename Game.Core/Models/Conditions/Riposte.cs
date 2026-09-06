using System;
using System.Collections.Generic;
using GameCore.Models.Conditions.Abstractions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.Conditions
{
    public class Riposte : ConditionBase
    {
        public int RiposteCount { get; private set; }
        public int RiposteDamage { get; private set; }

        public Riposte(int riposteCount, int riposteDamage)
        {
            RiposteCount = riposteCount;
            RiposteDamage = riposteDamage;
        }

        /// <summary>
        /// Counter attacks pile up, the hardest one sets the damage.
        /// </summary>
        internal void RenewRiposte(int riposteCount, int riposteDamage)
        {
            RiposteCount += riposteCount;
            RiposteDamage = Math.Max(riposteDamage, RiposteDamage);
        }

        /// <summary>
        /// A promised counter attack is spent on whoever draws blood. The damage is the one
        /// that was promised: it is not fed back through the modifier pipeline, so a riposte
        /// cannot be sharpened, warded off, or answered with another riposte.
        /// </summary>
        public override IEnumerable<GameEventBase> ReactToIncomingDamage(
            Combatant bearer,
            Combatant attacker,
            int damageTaken
        )
        {
            var gameEvents = new List<GameEventBase>();

            if (RiposteCount <= 0 || damageTaken <= 0)
                return gameEvents;

            if (attacker.Side == bearer.Side)
                return gameEvents;

            if (bearer.Hp <= 0 || attacker.Hp <= 0)
            {
                // A counter attack needs someone still standing to throw it, and someone
                // still standing to receive it.
                return gameEvents;
            }

            RiposteCount--;
            gameEvents.Add(
                new SimpleGameEvent($"{bearer.Class} ripostes and cuts {attacker.Class} back!")
            );
            gameEvents.Add(new HpReducedGameEvent(attacker, RiposteDamage));
            attacker.Hp -= RiposteDamage;

            return gameEvents;
        }

        public override bool ShouldBeRemoved() => RiposteCount <= 0;
    }
}
