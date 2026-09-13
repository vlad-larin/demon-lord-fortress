using System;
using System.Collections.Generic;
using GameCore.Extensions;
using GameCore.Models.CombatActions;
using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Models
{
    public class Combatant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public CharacterClass Class { get; set; }

        /// <summary>
        /// A proper name, for the few who have earned one. Rank and file monsters leave it
        /// empty and are known by their class instead.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tells apart the nameless standing shoulder to shoulder with their own kind:
        /// the second of four giant rats is "Giant Rat 2". Zero means there is only one
        /// of its kind around and it needs no number.
        /// </summary>
        public int Ordinal { get; set; }

        public ConflictSide Side { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int PerceivedDanger { get; set; }
        public List<CombatActionBase> Actions { get; set; } = new List<CombatActionBase>();
        public List<ConditionBase> Conditions { get; set; } = new List<ConditionBase>();

        /// <summary>
        /// The single name everything player facing uses - the battle log, the roster, the
        /// battle plan - so one combatant is called the same thing everywhere.
        /// </summary>
        public string DisplayName =>
            !string.IsNullOrEmpty(Name) ? Name
            : Ordinal > 0 ? $"{Class.GetDisplayName()} {Ordinal}"
            : Class.GetDisplayName();
    }
}
