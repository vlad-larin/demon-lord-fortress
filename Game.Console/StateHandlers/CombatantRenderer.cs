using GameCore.Models;

namespace GameConsoleApp.StateHandlers
{
    /// <summary>
    /// The single point where a combatant is turned into text for the UI. Every roster,
    /// prompt and battle plan line goes through here, so whatever a combatant shows
    /// (HP today, conditions tomorrow) stays consistent everywhere without hunting
    /// down interpolated strings.
    /// </summary>
    internal static class CombatantRenderer
    {
        public static string Render(Combatant combatant) =>
            $"{combatant.Class} {RenderHp(combatant)}";

        /// <summary>
        /// A planned action, with both of its participants rendered the usual way.
        /// </summary>
        public static string RenderIntent(CombatIntent intent) =>
            $"{Render(intent.Actor)}: {intent.Action.Name} -> {Render(intent.Target)}";

        private static string RenderHp(Combatant combatant) =>
            combatant.Hp <= 0 ? "[fallen]" : $"[{combatant.Hp}/{combatant.MaxHp} HP]";
    }
}
