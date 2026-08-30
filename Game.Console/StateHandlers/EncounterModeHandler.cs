using System;
using System.Linq;
using GameConsoleApp.Models;
using GameCore.Models;
using GameCore.ObservableStates;
using GameCore.PlayerActions;

namespace GameConsoleApp.StateHandlers
{
    public class EncounterModeHandler
    {
        internal static void RenderState(EncounterState state)
        {
            var encounter = state.Encounter;
            Console.WriteLine($"Encounter: Phase {encounter.Phase}");

            Console.WriteLine();

            Console.WriteLine("Enemies:");
            foreach (
                var enemyCombatant in encounter.Combatants.Where(x => x.Side == ConflictSide.Heroes)
            )
            {
                Console.WriteLine($"* {enemyCombatant.Class}");
            }

            Console.WriteLine();

            Console.WriteLine("Your forces:");
            foreach (
                var alliedCombatant in encounter.Combatants.Where(x =>
                    x.Side == ConflictSide.DemonLord
                )
            )
            {
                Console.WriteLine($"* {alliedCombatant.Class}");
            }

            Console.WriteLine();
            Console.WriteLine("Battle plan:");
            foreach (var intent in encounter.Intents)
            {
                Console.WriteLine(
                    $"* {intent.Actor.Class}: {intent.Action.Name} -> {intent.Target.Class}"
                );
            }
        }

        internal static GameModeHandlerResponse ProcessKey(ConsoleKeyInfo key, EncounterState state)
        {
            var encounter = state.Encounter;
            switch (encounter.Phase)
            {
                case EncounterPhase.Briefing:
                    return new GameModeHandlerResponse
                    {
                        ActionType = GameModeHandlerActionType.Execute,
                        Action = new ComposeHeroPartyPlanAction(),
                    };
                default:
                    throw new NotImplementedException(
                        $"[EncounterModeHandler] Unexpected phase: {encounter.Phase}"
                    );
            }
        }
    }
}
