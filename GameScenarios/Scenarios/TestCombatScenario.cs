using System.Collections.Generic;
using GameCore.Factories.Combatants;
using GameCore.Models;

namespace GameScenarios.Scenarios
{
    public class TestCombatScenario : ScenarioBase
    {
        public override GameInstance StartScenario()
        {
            var paladin = PaladinFactory.BuildCombatant();
            var fighter = FighterFactory.BuildCombatant();
            var wizard = WizardFactory.BuildCombatant();
            var rogue = RogueFactory.BuildCombatant();

            var cultist = CultistFactory.BuildCombatant();
            var boneKnight = BoneKnightFactory.BuildCombatant();
            var vampire = VampireFactory.BuildCombatant();

            return new GameInstance
            {
                GameMode = GameMode.Encounter,
                Tower = new Tower
                {
                    Floors = new List<Floor>
                    {
                        new Floor { Rooms = new List<Room> { new Room { } } },
                        new Floor { },
                    },
                },
                Encounter = new Encounter
                {
                    Phase = EncounterPhase.Briefing,
                    Combatants = new List<Combatant>
                    {
                        paladin,
                        fighter,
                        wizard,
                        rogue,
                        cultist,
                        boneKnight,
                        vampire,
                    },
                },
            };
        }
    }
}
