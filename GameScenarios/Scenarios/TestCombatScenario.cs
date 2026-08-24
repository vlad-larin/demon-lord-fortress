using System.Collections.Generic;
using GameCore.Models;
using GameScenarios.Attributes;

namespace GameScenarios.Scenarios
{
    [Scenario("Test combat scenario")]
    public class TestCombatScenario : ScenarioBase
    {
        public override GameInstance StartScenario()
        {
            var paladin = new Combatant
            {
                Class = CharacterClass.Paladin,
                Side = ConflictSide.Enemy,
            };
            var wizard = new Combatant { Class = CharacterClass.Wizard, Side = ConflictSide.Enemy };
            var rogue = new Combatant { Class = CharacterClass.Rogue, Side = ConflictSide.Enemy };

            var cultist = new Combatant
            {
                Class = CharacterClass.Cultist,
                Side = ConflictSide.Player,
            };
            var boneKnight = new Combatant
            {
                Class = CharacterClass.BoneKnight,
                Side = ConflictSide.Player,
            };
            var vampire = new Combatant
            {
                Class = CharacterClass.Vampire,
                Side = ConflictSide.Player,
            };

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
                        wizard,
                        rogue,
                        cultist,
                        boneKnight,
                        vampire,
                    },
                    Intents = new List<CombatIntent>
                    {
                        new CombatIntent { Combatant = paladin },
                        new CombatIntent { Combatant = wizard },
                        new CombatIntent { Combatant = rogue },
                    },
                },
            };
        }
    }
}
