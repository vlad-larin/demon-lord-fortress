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
        public override void Absorb(ConditionBase incoming)
        {
            var promise = (Riposte)incoming;
            RiposteCount += promise.RiposteCount;
            RiposteDamage = Math.Max(promise.RiposteDamage, RiposteDamage);
        }

        /// <summary>
        /// A promised counter attack is spent on whoever draws blood. The damage is the one
        /// that was promised: it is not fed back through the modifier pipeline, so a riposte
        /// cannot be sharpened, warded off, or answered with another riposte. A fatal blow
        /// is answered like any other: nobody falls until their clash is over.
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

            RiposteCount--;
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{bearer.DisplayName} ripostes and cuts {attacker.DisplayName} back!"
                )
            );
            gameEvents.Add(new HpReducedGameEvent(attacker, RiposteDamage));
            attacker.Hp -= RiposteDamage;

            return gameEvents;
        }

        public override bool ShouldBeRemoved() => RiposteCount <= 0;
    }
}
