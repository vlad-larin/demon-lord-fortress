using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameConsoleApp.Models;
using GameConsoleApp.StateHandlers.Abstractions;
using GameCore.Models;
using GameCore.Models.CombatActions;
using GameCore.ObservableStates;
using GameCore.PlayerActions;
using GameScenarios.Scenarios;

namespace GameConsoleApp.StateHandlers
{
    internal class EncounterModeHandler : StateHandlerBase<EncounterState>
    {
        public EncounterState State { get; private set; }

        private InnerState innerState;
        private Combatant selectedMonster;
        private CombatActionBase selectedAction;
        private Combatant selectedTarget;

        public EncounterModeHandler(EncounterState state)
        {
            State = state;
            innerState = InnerState.ChooseMonster;
        }

        public override void RenderState(EncounterState state)
        {
            var encounter = State.Encounter;
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

            switch (encounter.Phase)
            {
                case EncounterPhase.Briefing:
                    RenderBriefingPrompt();
                    break;
                case EncounterPhase.Planning when innerState == InnerState.ChooseMonster:
                    RenderChooseMonsterPrompt();
                    break;
                case EncounterPhase.Planning when innerState == InnerState.ChooseAction:
                    RenderChooseActionPrompt();
                    break;
                case EncounterPhase.Planning when innerState == InnerState.ChooseActionTarget:
                    RenderChooseActionTargetPrompt();
                    break;
                case EncounterPhase.Planning when innerState == InnerState.ChooseActionPosition:
                    RenderChooseActionPositionPrompt();
                    break;
                default:
                    throw new NotImplementedException(
                        $"Unknown combination: {encounter.Phase} + {innerState}"
                    );
            }
        }

        public override GameModeHandlerResponse ProcessKey(ConsoleKeyInfo key)
        {
            var encounter = State.Encounter;
            var option = key.KeyChar.ToString().ToUpperInvariant();

            switch (encounter.Phase)
            {
                case EncounterPhase.Briefing:
                    return ProcessKeyAtBriefingPhase(key);
                case EncounterPhase.Planning:
                    return ProcessKeyAtPlanningPhase(key);
                default:
                    throw new NotImplementedException(
                        $"[EncounterModeHandler] Unexpected phase: {encounter.Phase}"
                    );
            }
        }

        private void RenderBriefingPrompt()
        {
            RenderFrameStart();
            RenderFrameLine("BATTLE START");
            RenderFrameLine();
            RenderFrameLine("[Space]: Proceed to battle planning");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtBriefingPhase(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();
            if (option == " ")
            {
                return new GameModeHandlerResponse
                {
                    ActionType = GameModeHandlerActionType.Execute,
                    Action = new ComposeHeroPartyPlanAction(),
                };
            }
            return GameModeHandlerResponse.NoAction();
        }

        private Combatant[] GetMonsters() =>
            State.Encounter.Combatants.Where(x => x.Side == ConflictSide.DemonLord).ToArray();

        private GameModeHandlerResponse ProcessKeyAtPlanningPhase(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();

            if (innerState == InnerState.ChooseMonster)
            {
                return ProcessKeyAtPlanningPhaseChooseMonster(key);
            }
            else if (innerState == InnerState.ChooseAction)
            {
                return ProcessKeyAtPlanningPhaseChooseAction(key);
            }
            else if (innerState == InnerState.ChooseActionTarget)
            {
                return ProcessKeyAtPlanningPhaseChooseActionTarget(key);
            }
            else if (innerState == InnerState.ChooseActionPosition)
            {
                return ProcessKeyAtPlanningPhaseChooseActionPosition(key);
            }

            return GameModeHandlerResponse.NoAction();
        }

        #region Inner state: Choose Monster
        private void RenderChooseMonsterPrompt()
        {
            RenderFrameStart();
            RenderFrameLine("CHOOSE MONSTER");
            RenderFrameLine();

            var monsters = GetMonsters();
            for (int i = 0; i < monsters.Length; i++)
            {
                RenderFrameLine($"[{i + 1}]: {monsters[i].Class}");
            }
            RenderFrameLine();

            RenderFrameLine("[Space]: Finish");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseMonster(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();

            if (
                int.TryParse(option, out var optionNumber)
                && optionNumber > 0
                && optionNumber <= GetMonsters().Length
            )
            {
                selectedMonster = GetMonsters()[optionNumber - 1];
                innerState = InnerState.ChooseAction;
            }
            return GameModeHandlerResponse.NoAction();
        }

        #endregion

        #region Inner state: Choose Action
        private void RenderChooseActionPrompt()
        {
            RenderFrameStart();
            RenderFrameLine($"MONSTER: {selectedMonster.Class}");
            RenderFrameLine($"CHOOSE ACTION");
            RenderFrameLine();

            var actions = selectedMonster.Actions;
            for (int i = 0; i < actions.Count; i++)
            {
                RenderFrameLine($"[{i + 1}]: {actions[i].Name}");
            }
            RenderFrameLine();

            RenderFrameLine("[Esc]: Back to monster selection");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseAction(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();

            if (key.Key == ConsoleKey.Escape)
            {
                selectedMonster = null;
                innerState = InnerState.ChooseMonster;
            }
            else if (
                int.TryParse(option, out var optionNumber)
                && optionNumber > 0
                && optionNumber <= selectedMonster.Actions.Count
            )
            {
                selectedAction = selectedMonster.Actions[optionNumber - 1];
                innerState = InnerState.ChooseActionTarget;
            }

            return GameModeHandlerResponse.NoAction();
        }
        #endregion

        #region Inner state: Choose Action Target
        private void RenderChooseActionTargetPrompt()
        {
            RenderFrameStart();
            RenderFrameLine($"MONSTER: {selectedMonster.Class}");
            RenderFrameLine($"ACTION: {selectedAction.Name}");
            RenderFrameLine($"CHOOSE TARGET");
            RenderFrameLine();

            var targets = selectedAction.GetValidTargets(
                selectedMonster,
                State.Encounter.Combatants
            );

            for (int i = 0; i < targets.Count; i++)
            {
                RenderFrameLine($"[{i + 1}]: {targets[i].Class}");
            }
            RenderFrameLine();

            RenderFrameLine("[Esc]: Back to action selection");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseActionTarget(
            ConsoleKeyInfo key
        )
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();

            if (key.Key == ConsoleKey.Escape)
            {
                selectedAction = null;
                innerState = InnerState.ChooseAction;
            }
            else if (int.TryParse(option, out var optionNumber) && optionNumber > 0)
            {
                var targets = selectedAction.GetValidTargets(
                    selectedMonster,
                    State.Encounter.Combatants
                );

                if (optionNumber <= targets.Count)
                {
                    selectedTarget = targets[optionNumber - 1];
                    innerState = InnerState.ChooseActionPosition;
                }
            }

            return GameModeHandlerResponse.NoAction();
        }
        #endregion

        #region Inner state: Choose Action Position
        private void RenderChooseActionPositionPrompt()
        {
            RenderFrameStart();
            RenderFrameLine($"MONSTER: {selectedMonster.Class}");
            RenderFrameLine($"ACTION: {selectedAction.Name}");
            RenderFrameLine($"TARGET: {selectedTarget.Class}");
            RenderFrameLine("CHOOSE INSERT POSITION");
            RenderFrameLine();

            var availableSlots = GetAvailableSlots();
            for (var i = 0; i < State.Encounter.Intents.Count; i++)
            {
                var intent = State.Encounter.Intents[i];
                var availableSlot = availableSlots.SingleOrDefault(x => x.Index == i);
                if (availableSlot != null)
                {
                    if (availableSlot.Intent == null)
                    {
                        RenderFrameLine(
                            $"[{availableSlots.IndexOf(availableSlot) + 1}] SLOT AVAILABLE"
                        );
                    }
                    else
                    {
                        RenderFrameLine(
                            $"[{availableSlots.IndexOf(availableSlot) + 1}] {intent.Actor.Class}: {intent.Action.Name} -> {intent.Target.Class}"
                        );
                    }
                }
                if (intent.Actor.Side == ConflictSide.Heroes)
                {
                    RenderFrameLine(
                        $"* {intent.Actor.Class}: {intent.Action.Name} -> {intent.Target.Class}"
                    );
                }
            }

            RenderFrameLine();
            RenderFrameLine("[Esc]: Back to target selection");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseActionPosition(
            ConsoleKeyInfo key
        )
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();

            if (key.Key == ConsoleKey.Escape)
            {
                selectedTarget = null;
                innerState = InnerState.ChooseActionTarget;
                return GameModeHandlerResponse.NoAction();
            }
            else if (int.TryParse(option, out var optionNumber) && optionNumber > 0)
            {
                var availableSlots = GetAvailableSlots();
                if (optionNumber > availableSlots.Count)
                {
                    return GameModeHandlerResponse.NoAction();
                }

                var selectedSlot = availableSlots[optionNumber - 1];
                return new GameModeHandlerResponse()
                {
                    ActionType = GameModeHandlerActionType.Execute,
                    Action = new InsertCombatActionIntoBattlePlan(
                        intent: new CombatIntent
                        {
                            Actor = selectedMonster,
                            Action = selectedAction,
                        },
                        index: selectedSlot.Index,
                        actorId: selectedMonster.Id,
                        targetId: selectedTarget.Id
                    ),
                };
            }

            return GameModeHandlerResponse.NoAction();
        }

        private List<IntentSlot> GetAvailableSlots()
        {
            var list = new List<IntentSlot>();
            for (var i = 0; i < State.Encounter.Intents.Count; i++)
            {
                var intent = State.Encounter.Intents[i];
                var previousIntent = i == 0 ? null : State.Encounter.Intents[i - 1];

                if (
                    intent.Actor.Side == ConflictSide.Heroes
                    && previousIntent?.Actor.Side == ConflictSide.Heroes
                )
                {
                    list.Add(new IntentSlot(i, null));
                }
                else if (intent.Actor.Side == ConflictSide.DemonLord)
                {
                    list.Add(new IntentSlot(i, intent));
                }
            }

            return list;
        }

        private class IntentSlot
        {
            public int Index { get; private set; }

            /// <summary>
            /// null is for inserts and populated value is for overwriting
            /// </summary>
            public CombatIntent Intent { get; private set; }

            public IntentSlot(int index, CombatIntent intent)
            {
                Index = index;
                Intent = intent;
            }
        }
        #endregion

        private enum InnerState
        {
            ChooseMonster,
            ChooseAction,
            ChooseActionTarget,
            ChooseActionPosition,
        }
    }
}
