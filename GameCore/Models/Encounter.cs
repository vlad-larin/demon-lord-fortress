using System.Collections.Generic;
using GameCore.Interfaces;

namespace GameCore.Models
{
    public class Encounter
    {
        public EncounterPhase Phase { get; set; } = EncounterPhase.Unknown;
        public IHeroPartyStrategy HeroPartyStrategy { get; set; }
        public List<Combatant> Combatants { get; set; } = new List<Combatant>();
        public List<Combatant> DeadCombatants { get; set; } = new List<Combatant>();
        public List<CombatIntent> Intents { get; set; } = new List<CombatIntent>();
    }
}
