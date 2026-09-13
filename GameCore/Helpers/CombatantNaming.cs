using System.Collections.Generic;
using System.Linq;
using GameCore.Models;

namespace GameCore.Helpers
{
    public static class CombatantNaming
    {
        /// <summary>
        /// Numbers everyone who shares a class with someone else in the group, so a battle
        /// plan can say which of four giant rats it means. Combatants that already carry a
        /// proper name are left alone, and a class with a single representative keeps its
        /// bare name.
        /// </summary>
        public static void NumberDuplicates(IEnumerable<Combatant> combatants)
        {
            var nameless = combatants.Where(c => string.IsNullOrEmpty(c.Name)).ToList();

            foreach (var kind in nameless.GroupBy(c => c.Class).Where(group => group.Count() > 1))
            {
                var ordinal = 1;
                foreach (var combatant in kind)
                    combatant.Ordinal = ordinal++;
            }
        }
    }
}
