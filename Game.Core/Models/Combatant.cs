using System;
using System.Collections.Generic;
using GameCore.Models.CombatActions;
using GameCore.Models.Conditions.Abstractions;
using GameCore.Models.GameEvents;

namespace GameCore.Models
{
    public class Combatant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CharacterClass Class { get; set; }
        public ConflictSide Side { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int PerceivedDanger { get; set; }
        public List<CombatActionBase> Actions { get; set; } = new List<CombatActionBase>();
        public List<ConditionBase> Conditions { get; set; } = new List<ConditionBase>();

        internal IEnumerable<GameEventBase> InflictDamage(int damage)
        {
            var multiplier = 1m;
            foreach (var condition in Conditions)
                multiplier = multiplier * condition.GetIncomingDamageMultiplier();

            var actualDamage = Convert.ToInt32(
                Math.Round(damage * multiplier, 0, MidpointRounding.AwayFromZero)
            );

            foreach (var condition in Conditions)
                actualDamage = actualDamage + condition.GetIncomingDamageFlatModifier(actualDamage);

            var gameEvent = new HpReducedGameEvent(this, actualDamage);
            Hp -= actualDamage;

            return new HpReducedGameEvent[] { gameEvent };
        }
    }
}
