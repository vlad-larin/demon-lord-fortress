using System.Linq;
using GameCore.Models;
using GameCore.Models.Conditions.Abstractions;

namespace GameCore.Extensions
{
    public static class CombatantConditionExtensions
    {
        public static T GetCondition<T>(this Combatant combatant)
            where T : ConditionBase => combatant.Conditions.OfType<T>().FirstOrDefault();

        public static bool HasCondition<T>(this Combatant combatant)
            where T : ConditionBase => combatant.GetCondition<T>() != null;

        /// <summary>
        /// Inflicts a duration-only condition, or prolongs the one already in place.
        /// </summary>
        public static T ApplyForRounds<T>(this Combatant combatant, int rounds)
            where T : TimedConditionBase, new()
        {
            var condition = combatant.GetCondition<T>();
            if (condition == null)
            {
                condition = new T();
                combatant.Conditions.Add(condition);
            }

            condition.AddRounds(rounds);
            return condition;
        }
    }
}
