using System.Collections.Generic;

namespace GameCore.Models
{
    public class Encounter
    {
        public EncounterPhase Phase { get; set; } = EncounterPhase.Unknown;
        public List<Combatant> Combatants { get; set; } = new List<Combatant>();
        public List<CombatIntent> Intents { get; set; } = new List<CombatIntent>();
    }
}
