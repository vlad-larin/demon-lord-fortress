using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models.CombatActions;

namespace GameCore.Models.HeroPartyStrategies.Abstractions
{
    public abstract class HeroPartyStrategyBase
    {
        protected static Random Rnd = new Random();

        protected Encounter Encounter { get; private set; }

        protected HeroPartyStrategyBase(Encounter encounter)
        {
            Encounter = encounter;
        }

        protected List<Combatant> GetEnemies() =>
            Encounter.Combatants.Where(c => c.Side == ConflictSide.DemonLord).ToList();

        protected List<Combatant> GetParty() =>
            Encounter.Combatants.Where(c => c.Side == ConflictSide.Heroes).ToList();

        /// <summary>
        /// Hands the finished party plan over to the encounter. Every hero opens a clash of
        /// their own, in the order the strategy wrote them; deciding which of those clashes
        /// to step into is the demon lord's half of the plan.
        /// </summary>
        protected void SetBattlePlan(IEnumerable<CombatIntent> intents) =>
            Encounter.BattlePlan = intents.Select(intent => new CombatStep(intent)).ToList();
    }
}
