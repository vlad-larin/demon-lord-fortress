using System;
using System.Collections.Generic;
using System.Linq;
using GameConsoleApp.Models;
using GameConsoleApp.StateHandlers.Abstractions;
using GameCore.Models;
using GameCore.Models.CombatActions;
using GameCore.ObservableStates;
using GameCore.PlayerActions;

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

            switch (encounter.Phase)
            {
                case EncounterPhase.Briefing:
                    RenderBriefingPrompt();
                    break;
                case EncounterPhase.Planning:
                    RenderPlanningPrompt();
                    break;
                case EncounterPhase.Resolution:
                    RenderResolutionPrompt();
                    break;
                case EncounterPhase.Debriefing:
                    RenderDebriefingPrompt();
                    break;
                default:
                    throw new NotImplementedException($"Unknown phase: {encounter.Phase}");
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
                case EncounterPhase.Resolution:
                    return ProcessKeyAtResolutionPhase(key);
                case EncounterPhase.Debriefing:
                    return ProcessKeyAtDebriefingPhase(key);
                default:
                    throw new NotImplementedException(
                        $"[EncounterModeHandler] Unexpected phase: {encounter.Phase}"
                    );
            }
        }

        #region Briefing
        private void RenderBriefingPrompt()
        {
            RenderFrameStart();
            RenderFrameLine("BATTLE START");
            RenderFrameLine();
            RenderBattleSides();
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
        #endregion

        #region Planning
        private void RenderPlanningPrompt()
        {
            RenderFrameStart();

            RenderFrameLine("BATTLE PLANNING");
            RenderFrameLine();
            RenderBattleSides();

            RenderFrameDivider();

            RenderFrameLine("BATTLE PLAN");
            RenderFrameLine("A step happens all at once: everyone in it acts before anyone falls.");
            RenderFrameLine(
                "One monster to a step; those who do not fit act after the plan runs out."
            );
            RenderFrameLine();
            RenderBattlePlan();
            RenderFrameLine();

            switch (innerState)
            {
                case InnerState.ChooseMonster:
                    RenderChooseMonsterPrompt();
                    break;
                case InnerState.ChooseAction:
                    RenderChooseActionPrompt();
                    break;
                case InnerState.ChooseActionTarget:
                    RenderChooseActionTargetPrompt();
                    break;
                case InnerState.ChooseActionPosition:
                    RenderChooseActionPositionPrompt();
                    break;
                default:
                    throw new NotImplementedException(
                        $"[Planning] Unknown inner state: {innerState}"
                    );
            }
        }
        #endregion

        /// <summary>
        /// The plan as the round will actually run it: an ordered list of clashes, with
        /// everything inside one clash happening at the same moment.
        /// </summary>
        private void RenderBattlePlan()
        {
            var battlePlan = State.Encounter.BattlePlan;
            if (battlePlan.Count == 0)
            {
                RenderFrameLine("* Nothing is planned");
                return;
            }

            for (var stepIndex = 0; stepIndex < battlePlan.Count; stepIndex++)
            {
                RenderFrameLine($"STEP {stepIndex + 1}");
                foreach (var intent in battlePlan[stepIndex].Intents)
                    RenderFrameLine($"  * {CombatantRenderer.RenderIntent(intent)}");
            }
        }

        private bool IsAssigned(Combatant monster) =>
            State.Encounter.BattlePlan.Any(step =>
                step.Intents.Any(intent => intent.Actor == monster)
            );

        private Combatant[] GetHeroes() =>
            State.Encounter.Combatants.Where(x => x.Side == ConflictSide.Heroes).ToArray();

        private Combatant[] GetMonsters() =>
            State.Encounter.Combatants.Where(x => x.Side == ConflictSide.DemonLord).ToArray();

        private void RenderBattleSides()
        {
            RenderFrameLine("ENEMIES:");
            RenderSideRoster(ConflictSide.Heroes);

            RenderFrameLine();

            RenderFrameLine("YOUR FORCES:");
            RenderSideRoster(ConflictSide.DemonLord);
        }

        /// <summary>
        /// Everyone who entered the battle on that side, the fallen included, so a wiped
        /// side does not simply vanish from the roster.
        /// </summary>
        private void RenderSideRoster(ConflictSide side)
        {
            var encounter = State.Encounter;
            var roster = encounter
                .Combatants.Concat(encounter.DeadCombatants)
                .Where(combatant => combatant.Side == side);

            foreach (var combatant in roster)
                RenderFrameLine($"* {CombatantRenderer.Render(combatant)}");
        }

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
            RenderFrameDivider();
            RenderFrameLine("CHOOSE MONSTER");
            RenderFrameLine();

            var monsters = GetMonsters();
            for (int i = 0; i < monsters.Length; i++)
            {
                var monster = monsters[i];
                RenderFrameLine(
                    $"[{RenderOptionLabel(i)}]: {CombatantRenderer.Render(monster)}"
                        + $"{(IsAssigned(monster) ? " (assigned)" : "")}"
                );
            }
            RenderFrameLine();

            RenderFrameLine("[Space]: Finish");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseMonster(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();

            if (option == " ")
                return new GameModeHandlerResponse
                {
                    ActionType = GameModeHandlerActionType.Execute,
                    Action = new ExecuteBattlePlanAction(),
                };

            var monsters = GetMonsters();
            if (TryReadOption(key, monsters.Length, out var monsterIndex))
            {
                selectedMonster = monsters[monsterIndex];
                innerState = InnerState.ChooseAction;
            }
            return GameModeHandlerResponse.NoAction();
        }

        #endregion

        #region Inner state: Choose Action
        private void RenderChooseActionPrompt()
        {
            RenderFrameDivider();
            RenderFrameLine($"MONSTER: {CombatantRenderer.Render(selectedMonster)}");
            RenderFrameLine("CHOOSE ACTION");
            RenderFrameLine();

            var actions = selectedMonster.Actions;
            for (int i = 0; i < actions.Count; i++)
            {
                RenderFrameLine($"[{RenderOptionLabel(i)}]: {actions[i].Name}");
            }
            RenderFrameLine();

            RenderFrameLine("[Esc]: Back to monster selection");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseAction(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                selectedMonster = null;
                innerState = InnerState.ChooseMonster;
            }
            else if (TryReadOption(key, selectedMonster.Actions.Count, out var actionIndex))
            {
                selectedAction = selectedMonster.Actions[actionIndex];
                innerState = InnerState.ChooseActionTarget;
            }

            return GameModeHandlerResponse.NoAction();
        }
        #endregion

        #region Inner state: Choose Action Target
        private void RenderChooseActionTargetPrompt()
        {
            RenderFrameDivider();
            RenderFrameLine($"MONSTER: {CombatantRenderer.Render(selectedMonster)}");
            RenderFrameLine($"ACTION: {selectedAction.Name}");
            RenderFrameLine($"CHOOSE TARGET");
            RenderFrameLine();

            var targets = selectedAction.GetValidTargets(
                selectedMonster,
                State.Encounter.Combatants
            );

            for (int i = 0; i < targets.Count; i++)
            {
                RenderFrameLine(
                    $"[{RenderOptionLabel(i)}]: {CombatantRenderer.Render(targets[i])}"
                );
            }
            RenderFrameLine();

            RenderFrameLine("[Esc]: Back to action selection");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseActionTarget(
            ConsoleKeyInfo key
        )
        {
            if (key.Key == ConsoleKey.Escape)
            {
                selectedAction = null;
                innerState = InnerState.ChooseAction;
                return GameModeHandlerResponse.NoAction();
            }

            var targets = selectedAction.GetValidTargets(
                selectedMonster,
                State.Encounter.Combatants
            );

            if (TryReadOption(key, targets.Count, out var targetIndex))
            {
                selectedTarget = targets[targetIndex];
                innerState = InnerState.ChooseActionPosition;
            }

            return GameModeHandlerResponse.NoAction();
        }
        #endregion

        #region Inner state: Choose Action Position
        private void RenderChooseActionPositionPrompt()
        {
            RenderFrameDivider();
            RenderFrameLine($"MONSTER: {CombatantRenderer.Render(selectedMonster)}");
            RenderFrameLine($"ACTION: {selectedAction.Name}");
            RenderFrameLine($"TARGET: {CombatantRenderer.Render(selectedTarget)}");
            RenderFrameLine("CHOOSE WHEN TO ACT");
            RenderFrameLine();

            // Every clash of the plan is offered under its own index, and the plan's length
            // is the one past the end - the same numbering ProcessKey reads back.
            var battlePlan = State.Encounter.BattlePlan;
            for (var stepIndex = 0; stepIndex < battlePlan.Count; stepIndex++)
            {
                RenderFrameLine($"STEP {stepIndex + 1}");
                foreach (var intent in battlePlan[stepIndex].Intents)
                    RenderFrameLine($"  * {CombatantRenderer.RenderIntent(intent)}");

                RenderFrameLine(
                    $"  [{RenderOptionLabel(stepIndex)}] ACT IN THIS STEP{RenderDisplaced(battlePlan[stepIndex])}"
                );
            }
            RenderFrameLine();
            RenderFrameLine(
                $"[{RenderOptionLabel(battlePlan.Count)}] A STEP OF ITS OWN, AFTER THE PLAN"
            );

            RenderFrameLine();
            RenderFrameLine("[Esc]: Back to target selection");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtPlanningPhaseChooseActionPosition(
            ConsoleKeyInfo key
        )
        {
            if (key.Key == ConsoleKey.Escape)
            {
                selectedTarget = null;
                innerState = InnerState.ChooseActionTarget;
                return GameModeHandlerResponse.NoAction();
            }

            // One option per clash, plus the one past the end of the plan.
            var battlePlan = State.Encounter.BattlePlan;
            if (!TryReadOption(key, battlePlan.Count + 1, out var stepIndex))
                return GameModeHandlerResponse.NoAction();

            return new GameModeHandlerResponse()
            {
                ActionType = GameModeHandlerActionType.Execute,
                Action = new InsertCombatActionIntoBattlePlan(
                    intent: new CombatIntent { Actor = selectedMonster, Action = selectedAction },
                    stepIndex: stepIndex,
                    actorId: selectedMonster.Id,
                    targetId: selectedTarget.Id
                ),
            };
        }

        /// <summary>
        /// The warning that taking this clash costs somebody else their orders. The chosen
        /// monster's own clash says nothing - picking that one just leaves it where it is.
        /// </summary>
        private string RenderDisplaced(CombatStep step)
        {
            var occupant = step.MonsterIntent;
            if (occupant == null || occupant.Actor == selectedMonster)
                return string.Empty;

            return $", PULLING {occupant.Actor.DisplayName.ToUpperInvariant()} OUT";
        }

        #endregion

        #region Resolution
        private void RenderResolutionPrompt()
        {
            RenderFrameStart();
            RenderFrameLine("ROUND RESOLVED");
            RenderFrameLine();
            RenderBattleSides();
            RenderFrameLine();
            RenderFrameLine("[Space]: Proceed");
            RenderFrameFinish();
        }

        private GameModeHandlerResponse ProcessKeyAtResolutionPhase(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();
            if (option == " ")
            {
                return new GameModeHandlerResponse
                {
                    ActionType = GameModeHandlerActionType.Execute,
                    Action = new FinishEncounterRoundResolutionAction(),
                };
            }
            return GameModeHandlerResponse.NoAction();
        }
        #endregion

        #region Debriefing
        private void RenderDebriefingPrompt()
        {
            var outcome = State.Encounter.Outcome;

            RenderFrameStart();
            RenderFrameLine(RenderOutcomeHeadline(outcome));
            RenderFrameLine(outcome.Reason);
            RenderFrameLine();
            RenderBattleSides();

            RenderFrameDivider();

            RenderFrameLine("AFTERMATH");
            RenderFrameLine();
            if (outcome.Consequences.Count == 0)
                RenderFrameLine("* Nothing to report yet");
            else
                foreach (var consequence in outcome.Consequences)
                    RenderFrameLine($"* {consequence}");
            RenderFrameLine();

            RenderFrameLine("[Space]: Conclude the encounter");
            RenderFrameFinish();
        }

        /// <summary>
        /// The encounter is read from where the player sits, which is the demon lord's throne.
        /// </summary>
        private static string RenderOutcomeHeadline(EncounterOutcome outcome)
        {
            switch (outcome.Winner)
            {
                case ConflictSide.DemonLord:
                    return "VICTORY";
                case ConflictSide.Heroes:
                    return "DEFEAT";
                default:
                    return "NOBODY WON";
            }
        }

        private GameModeHandlerResponse ProcessKeyAtDebriefingPhase(ConsoleKeyInfo key)
        {
            var option = key.KeyChar.ToString().ToUpperInvariant();
            if (option == " ")
            {
                return new GameModeHandlerResponse
                {
                    ActionType = GameModeHandlerActionType.Execute,
                    Action = new FinishEncounterDebriefingAction(),
                };
            }
            return GameModeHandlerResponse.NoAction();
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
