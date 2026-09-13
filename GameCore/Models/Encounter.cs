using System.Collections.Generic;
using GameCore.Interfaces;
using GameCore.Models.EncounterEndConditions;
using GameCore.Models.EncounterEndConditions.Abstractions;

namespace GameCore.Models
{
    public class Encounter
    {
        public EncounterPhase Phase { get; set; } = EncounterPhase.Unknown;
        public IHeroPartyStrategy HeroPartyStrategy { get; set; }
        public List<Combatant> Combatants { get; set; } = new List<Combatant>();
        public List<Combatant> DeadCombatants { get; set; } = new List<Combatant>();
        public List<CombatIntent> Intents { get; set; } = new List<CombatIntent>();

        /// <summary>
        /// Everything that can stop this encounter. A wiped side always does, so it is here
        /// by default and no scenario can forget it into an endless fight; a scenario adds
        /// its own - an objective reached, a side withdrawing - or replaces the list outright.
        /// </summary>
        public List<EncounterEndConditionBase> EndConditions { get; set; } =
            new List<EncounterEndConditionBase> { new SideEliminated() };

        /// <summary>
        /// Set once an end condition has been met, and null while the encounter goes on.
        /// </summary>
        public EncounterOutcome Outcome { get; set; }
    }
}
