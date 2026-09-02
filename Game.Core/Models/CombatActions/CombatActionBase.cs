using System.Collections.Generic;
using System.Linq;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public abstract class CombatActionBase
    {
        public string Name { get; }

        protected CombatActionBase(string name)
        {
            Name = name;
        }

        public abstract int GetDamage(Combatant actor, Combatant target);
        public abstract int GetProtection(Combatant actor, Combatant target);
        public abstract List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        );

        protected List<Combatant> GetEnemies(Combatant actor, List<Combatant> combatants) =>
            combatants.Where(c => c.Side != actor.Side).ToList();

        protected List<Combatant> GetAllies(Combatant actor, List<Combatant> combatants) =>
            combatants.Where(c => c.Side == actor.Side).ToList();

        public abstract IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        );
    }
}
