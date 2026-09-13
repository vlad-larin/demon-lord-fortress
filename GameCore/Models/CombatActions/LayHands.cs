using System;
using System.Collections.Generic;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class LayHands : CombatActionBase
    {
        public int Heal { get; private set; }

        public LayHands(int heal)
            : base("Lay Hands")
        {
            Heal = heal;
        }

        public override int GetDamage(Combatant actor, Combatant target) => -Heal;

        public override int GetProtection(Combatant actor, Combatant target) => Heal;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetAllies(actor, combatants);

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            var heal = Math.Min(Heal, target.MaxHp - target.Hp);

            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(
                heal > 0
                    ? new SimpleGameEvent(
                        $"{actor.DisplayName} heals {target.DisplayName} for {heal} HP"
                    )
                    : new SimpleGameEvent(
                        $"{actor.DisplayName} lays hands on {target.DisplayName}, but they are already whole"
                    )
            );

            target.Hp += heal;
            gameEvents.Add(new HpIncreasedGameEvent(target, heal));

            return gameEvents;
        }
    }
}
