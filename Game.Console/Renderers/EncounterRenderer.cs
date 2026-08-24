using System;
using System.Linq;
using GameCore.Models;
using GameCore.ObservableStates;

namespace GameConsoleApp.Renderers
{
    public class EncounterRenderer
    {
        internal static void RenderState(EncounterState state)
        {
            var encounter = state.Encounter;
            Console.WriteLine($"Encounter: Phase {encounter.Phase}");

            Console.WriteLine();

            Console.WriteLine("Enemies:");
            foreach (
                var enemyCombatant in encounter.Combatants.Where(x => x.Side == ConflictSide.Enemy)
            )
            {
                Console.WriteLine($"* {enemyCombatant.Class}");
            }

            Console.WriteLine();

            Console.WriteLine("Your forces:");
            foreach (
                var alliedCombatant in encounter.Combatants.Where(x =>
                    x.Side == ConflictSide.Player
                )
            )
            {
                Console.WriteLine($"* {alliedCombatant.Class}");
            }
        }
    }
}
