using System.Collections.Generic;
using System.Linq;
using GameCore.Factories.Combatants;
using GameCore.Helpers;
using GameCore.Models;
using GameCore.Models.HeroObjectives.Abstractions;
using GameCore.Models.RoomProperties;
using GameCore.Models.RoomProperties.Abstractions;

namespace GameScenarios.Scenarios
{
    /// <summary>
    /// A three floor tower with a party climbing it: two floors of rooms the heroes pick
    /// between, and a throne room on top where the Demon Lord waits with his retinue. What
    /// the party is after is handed in, because the objective decides how far up they come
    /// and which room they walk into once they get there.
    /// </summary>
    public class TowerClimbingScenario : ScenarioBase
    {
        private readonly HeroObjectiveBase objective;

        public TowerClimbingScenario(HeroObjectiveBase objective)
        {
            this.objective = objective;
        }

        public override GameInstance StartScenario()
        {
            var floors = new List<Floor>
            {
                BuildDungeonsFloor(),
                BuildInnerSanctumFloor(),
                BuildThroneFloor(),
            };

            // A floor is the unit the Demon Lord deploys across, so guardians are numbered
            // floor wide: a rat that reinforces the room next door does not walk in sharing
            // a name with the rats already fighting there.
            foreach (var floor in floors)
                CombatantNaming.NumberDuplicates(floor.Rooms.SelectMany(room => room.Guardians));

            var heroes = BuildHeroParty();
            CombatantNaming.NumberDuplicates(heroes);

            return new GameInstance
            {
                GameMode = GameMode.TowerClimbing,
                Tower = new Tower { Floors = floors },
                Expedition = new Expedition
                {
                    Heroes = heroes,
                    Objective = objective,
                    CurrentFloorNumber = 1,
                },
            };
        }

        /// <summary>
        /// The four that walked in the front door, one of each trade the kingdom could
        /// spare.
        /// </summary>
        private static List<Combatant> BuildHeroParty() =>
            new List<Combatant>
            {
                PaladinFactory.BuildCombatant(),
                FighterFactory.BuildCombatant(),
                WizardFactory.BuildCombatant(),
                RogueFactory.BuildCombatant(),
            };

        /// <summary>
        /// Floor 1: the crude working guts of the fortress, held by whatever is cheap
        /// to lose - and, in the oubliette, holding what the fortress took.
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
                    Holding(
                        BuildRoom(
                            "Oubliette",
                            capacity: 3,
                            GhoulFactory.BuildCombatant(),
                            CultistFactory.BuildCombatant(),
                            GiantRatFactory.BuildCombatant()
                        ),
                        new HoldsThePrincess()
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
                    Holding(
                        BuildRoom(
                            "Library",
                            capacity: 3,
                            CultistFactory.BuildCombatant(),
                            CultistFactory.BuildCombatant(),
                            GargoyleFactory.BuildCombatant()
                        ),
                        new HoldsTheForbiddenLore()
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
                    Holding(
                        BuildRoom(
                            "Throne Room",
                            capacity: 3,
                            Named(DemonLordFactory.BuildCombatant(), "Malgrath the Unmade"),
                            Named(LieutenantFactory.BuildCombatant(), "Sister Vaine"),
                            Named(BodyguardFactory.BuildCombatant(), "Korrun")
                        ),
                        new HoldsTheThrone()
                    ),
                },
            };

        /// <summary>
        /// The three in the throne room are characters rather than stock monsters, so they
        /// go by their own names instead of their class.
        /// </summary>
        private static Combatant Named(Combatant combatant, string name)
        {
            combatant.Name = name;
            return combatant;
        }

        /// <summary>
        /// What the room keeps, which is what makes it worth climbing to.
        /// </summary>
        private static Room Holding(Room room, params RoomPropertyBase[] properties)
        {
            room.Properties.AddRange(properties);
            return room;
        }

        private static Room BuildRoom(string type, int capacity, params Combatant[] guardians) =>
            new Room
            {
                Type = type,
                Capacity = capacity,
                Guardians = new List<Combatant>(guardians),
            };
    }
}
