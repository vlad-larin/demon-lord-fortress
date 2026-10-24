using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Models.Conditions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Challenge : CombatActionBase
    {
        public int TauntRounds { get; private set; }
        public int RiposteCount { get; private set; }
        public int RiposteDamage { get; private set; }

        public Challenge(int tauntRounds, int riposteCount, int riposteDamage)
            : base("Challenge")
        {
            TauntRounds = tauntRounds;
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
                    $"{actor.DisplayName} challenges {target.DisplayName}, promising {RiposteCount} ripostes of {RiposteDamage} damage"
                )
            );

            target.Inflict(new Taunted(actor, TauntRounds));
            actor.Inflict(new Riposte(riposteCount: RiposteCount, riposteDamage: RiposteDamage));

            return gameEvents;
        }
    }
}
