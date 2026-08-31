using System;
using System.Collections.Generic;
using GameCore.Models.CombatActions;

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
        public List<CombatActionBase> Actions { get; set; }
    }
}
