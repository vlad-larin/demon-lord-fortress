using System.Collections.Generic;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Challenge : CombatActionBase
    {
        public int RiposteCount { get; private set; }
        public int RiposteDamage { get; private set; }

        public Challenge(int riposteCount, int riposteDamage)
            : base("Challenge")
        {
            RiposteCount = riposteCount;
            RiposteDamage = riposteDamage;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetEnemies(actor, combatants);

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            // A riposte fires in reaction to being attacked. Actions are only resolved on the
            // turn of their own actor, so the counter attacks cannot be armed from here and
            // the challenge is only declared.
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} challenges {target.Class}, promising {RiposteCount} ripostes of {RiposteDamage} damage"
                )
            );

            return gameEvents;
        }
    }
}
