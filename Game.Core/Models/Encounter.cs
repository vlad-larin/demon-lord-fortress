using System.Collections.Generic;

namespace GameCore.Models
{
    public class Encounter
    {
        public EncounterPhase Phase { get; set; }
        public List<Combatant> Combatants { get; set; }
        public List<CombatIntent> Intents { get; set; }
    }
}
