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
    }
}
