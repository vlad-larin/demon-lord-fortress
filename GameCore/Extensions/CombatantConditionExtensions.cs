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
        /// Inflicts a condition. It does not take hold at once: it waits until the clash it
        /// was inflicted in has been resolved, so everyone in that clash acts on the board
        /// as they found it. Two of a kind inflicted in the same clash are folded together
        /// on the spot, so what finally lands is always a single condition.
        /// </summary>
        public static void Inflict(this Combatant combatant, ConditionBase condition)
        {
            var alreadyInflicted = combatant.InflictedConditions.FirstOrDefault(c =>
                c.GetType() == condition.GetType()
            );

            if (alreadyInflicted == null)
                combatant.InflictedConditions.Add(condition);
            else
                alreadyInflicted.Absorb(condition);
        }

        /// <summary>
        /// Inflicts a duration-only condition.
        /// </summary>
        public static void InflictForRounds<T>(this Combatant combatant, int rounds)
            where T : TimedConditionBase, new()
        {
            var condition = new T();
            condition.AddRounds(rounds);
            combatant.Inflict(condition);
        }

        /// <summary>
        /// Brings everything inflicted during the clash into force, folding each one into
        /// the condition already in place rather than letting anybody carry two of a kind.
        /// Call it once a clash is fully resolved and never in the middle of one.
        /// </summary>
        public static void SettleInflictedConditions(this Combatant combatant)
        {
            foreach (var inflicted in combatant.InflictedConditions)
            {
                var existing = combatant.Conditions.FirstOrDefault(c =>
                    c.GetType() == inflicted.GetType()
                );

                if (existing == null)
                    combatant.Conditions.Add(inflicted);
                else
                    existing.Absorb(inflicted);
            }

            combatant.InflictedConditions.Clear();
        }
    }
}
