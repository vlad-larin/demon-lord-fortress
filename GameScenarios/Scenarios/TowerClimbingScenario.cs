using System.Collections.Generic;
using GameCore.Factories.Combatants;
using GameCore.Models;

namespace GameScenarios.Scenarios
{
    /// <summary>
    /// A three floor tower to look at: two floors of three rooms the heroes could pick
    /// between, and a throne room on top where the Demon Lord waits with his retinue.
    /// The climb itself is not wired up yet - this scenario only lays out the fortress.
    /// </summary>
    public class TowerClimbingScenario : ScenarioBase
    {
        public override GameInstance StartScenario() =>
            new GameInstance
            {
                GameMode = GameMode.TowerClimbing,
                Tower = new Tower
                {
                    Floors = new List<Floor>
                    {
                        BuildDungeonsFloor(),
                        BuildInnerSanctumFloor(),
                        BuildThroneFloor(),
                    },
                },
            };

        /// <summary>
        /// Floor 1: the crude working guts of the fortress, held by whatever is cheap
        /// to lose.
        /// </summary>
        private static Floor BuildDungeonsFloor() =>
            new Floor
            {
                Rooms = new List<Room>
                {
                    BuildRoom(
                        "Acid Pool",
                        capacity: 4,
                        AcidSlimeFactory.BuildCombatant(),
                        AcidSlimeFactory.BuildCombatant(),
                        GiantRatFactory.BuildCombatant()
                    ),
                    BuildRoom(
                        "Butchery",
                        capacity: 3,
                        GhoulFactory.BuildCombatant(),
                        GhoulFactory.BuildCombatant(),
                        CultistFactory.BuildCombatant()
                    ),
                    BuildRoom(
                        "Rat Warrens",
                        capacity: 5,
                        GiantRatFactory.BuildCombatant(),
                        GiantRatFactory.BuildCombatant(),
                        GiantRatFactory.BuildCombatant(),
                        GiantRatFactory.BuildCombatant()
                    ),
                },
            };

        /// <summary>
        /// Floor 2: the rooms the Demon Lord actually cares about, and the better
        /// monsters that come with them.
        /// </summary>
        private static Floor BuildInnerSanctumFloor() =>
            new Floor
            {
                Rooms = new List<Room>
                {
                    BuildRoom(
                        "Blood Chapel",
                        capacity: 4,
                        VampireFactory.BuildCombatant(),
                        CultistFactory.BuildCombatant(),
                        CultistFactory.BuildCombatant()
                    ),
                    BuildRoom(
                        "Library",
                        capacity: 3,
                        CultistFactory.BuildCombatant(),
                        CultistFactory.BuildCombatant(),
                        GargoyleFactory.BuildCombatant()
                    ),
                    BuildRoom(
                        "Armory",
                        capacity: 4,
                        BoneKnightFactory.BuildCombatant(),
                        BoneKnightFactory.BuildCombatant(),
                        GargoyleFactory.BuildCombatant()
                    ),
                },
            };

        /// <summary>
        /// Floor 3: one room, no choice to make, and nowhere left to retreat to.
        /// </summary>
        private static Floor BuildThroneFloor() =>
            new Floor
            {
                Rooms = new List<Room>
                {
                    BuildRoom(
                        "Throne Room",
                        capacity: 3,
                        DemonLordFactory.BuildCombatant(),
                        LieutenantFactory.BuildCombatant(),
                        BodyguardFactory.BuildCombatant()
                    ),
                },
            };

        private static Room BuildRoom(string type, int capacity, params Combatant[] guardians) =>
            new Room
            {
                Type = type,
                Capacity = capacity,
                Guardians = new List<Combatant>(guardians),
                Properties = new List<RoomProperty>(),
            };
    }
}
