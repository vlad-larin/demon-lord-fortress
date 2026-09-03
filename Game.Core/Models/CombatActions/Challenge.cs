using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Models.Conditions;
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
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} challenges {target.Class}, promising {RiposteCount} ripostes of {RiposteDamage} damage"
                )
            );

            var taunted = target.GetCondition<Taunted>();
            if (taunted == null)
                target.Conditions.Add(new Taunted(actor));
            else
                taunted.Retaunt(actor);

            var riposte = actor.GetCondition<Riposte>();
            if (riposte == null)
                actor.Conditions.Add(
                    new Riposte(riposteCount: RiposteCount, riposteDamage: RiposteDamage)
                );
            else
                riposte.RenewRiposte(riposteCount: RiposteCount, riposteDamage: RiposteDamage);

            return gameEvents;
        }
    }
}
