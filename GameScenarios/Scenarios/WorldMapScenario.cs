using System.Collections.Generic;
using GameCore.Helpers;
using GameCore.Models;

namespace GameScenarios.Scenarios
{
    /// <summary>
    /// The kingdom as the Demon Lord first looks out over it: the tower in the north-eastern
    /// waste, the capital in the heartland, and the villages, mines and wilds the roads string
    /// together in between. Nothing moves on it yet; it is the board later turns play on.
    /// </summary>
    public class WorldMapScenario : ScenarioBase
    {
        public override GameInstance StartScenario()
        {
            var worldMap = BuildWorldMap();
            WorldMapValidation.EnsureWellFormed(worldMap);

            return new GameInstance
            {
                GameMode = GameMode.Map,
                WorldMap = worldMap,
                Turn = 1,
            };
        }

        /// <summary>
        /// Two ways lead out of the tower - through the fen, or past the ruins - and both
        /// take a few roads to reach the heartland. Locations are listed tower first, then
        /// roughly north to south, which is the order the screen labels them in.
        /// </summary>
        private static WorldMap BuildWorldMap()
        {
            var nightspire = Place("Nightspire", LocationType.Tower, 92, 6);
            var blackmireFen = Place("Blackmire Fen", LocationType.Swamp, 70, 22);
            var hollowbrook = Place("Hollowbrook", LocationType.Village, 25, 14);
            var whisperwood = Place("Whisperwood", LocationType.Forest, 47, 32);
            var millford = Place("Millford", LocationType.Village, 6, 36);
            var vaelMorath = Place("Ruins of Vael Morath", LocationType.Ruins, 90, 50);
            var aldmere = Place("Aldmere", LocationType.Capital, 22, 60);
            var greyveinMine = Place("Greyvein Mine", LocationType.Mine, 64, 60);
            var stonewatchKeep = Place("Stonewatch Keep", LocationType.Keep, 48, 76);
            var saltmarsh = Place("Saltmarsh Harbor", LocationType.Port, 4, 90);
            var shrineOfElora = Place("Shrine of Saint Elora", LocationType.Shrine, 78, 90);
            var ravensford = Place("Ravensford", LocationType.Town, 32, 94);

            return new WorldMap
            {
                Locations = new List<Location>
                {
                    nightspire,
                    blackmireFen,
                    hollowbrook,
                    whisperwood,
                    millford,
                    vaelMorath,
                    aldmere,
                    greyveinMine,
                    stonewatchKeep,
                    saltmarsh,
                    shrineOfElora,
                    ravensford,
                },
                Roads = new List<Road>
                {
                    Between(nightspire, blackmireFen),
                    Between(nightspire, vaelMorath),
                    Between(blackmireFen, whisperwood),
                    Between(vaelMorath, greyveinMine),
                    Between(vaelMorath, shrineOfElora),
                    Between(whisperwood, hollowbrook),
                    Between(whisperwood, aldmere),
                    Between(whisperwood, greyveinMine),
                    Between(hollowbrook, millford),
                    Between(millford, aldmere),
                    Between(aldmere, saltmarsh),
                    Between(aldmere, ravensford),
                    Between(aldmere, stonewatchKeep),
                    Between(greyveinMine, stonewatchKeep),
                    Between(stonewatchKeep, shrineOfElora),
                    Between(stonewatchKeep, ravensford),
                    Between(ravensford, saltmarsh),
                },
            };
        }

        private static Location Place(string name, LocationType type, int x, int y) =>
            new Location
            {
                Name = name,
                Type = type,
                X = x,
                Y = y,
            };

        private static Road Between(Location first, Location second) =>
            new Road { First = first, Second = second };
    }
}
