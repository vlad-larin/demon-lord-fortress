using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models;

namespace GameCore.Helpers
{
    public static class WorldMapValidation
    {
        /// <summary>
        /// Holds a hand-built map to what the rest of the game takes for granted: names that
        /// tell locations apart, one tower, roads that are drawn as straight lines without
        /// crossing or running through a location, and no corner of the kingdom the tower
        /// cannot reach. A map is laid out by hand in a scenario, so a slip there should
        /// stop the game at once rather than show up as a road nobody can follow.
        /// </summary>
        public static void EnsureWellFormed(WorldMap map)
        {
            EnsureNamesAreUnique(map);
            EnsureSingleTower(map);
            EnsureRoadsAreDistinct(map);
            EnsureRoadsMissLocations(map);
            EnsureRoadsDoNotCross(map);
            EnsureEverythingIsReachable(map);
        }

        private static void EnsureNamesAreUnique(WorldMap map)
        {
            var duplicate = map
                .Locations.GroupBy(location => location.Name)
                .FirstOrDefault(group => group.Count() > 1);
            if (duplicate != null)
                throw new InvalidOperationException(
                    $"[WorldMapValidation] More than one location is called {duplicate.Key}"
                );
        }

        private static void EnsureSingleTower(WorldMap map)
        {
            var towers = map.Locations.Count(location => location.Type == LocationType.Tower);
            if (towers != 1)
                throw new InvalidOperationException(
                    $"[WorldMapValidation] A map needs exactly one tower, this one has {towers}"
                );
        }

        private static void EnsureRoadsAreDistinct(WorldMap map)
        {
            var seen = new List<Road>();
            foreach (var road in map.Roads)
            {
                if (!map.Locations.Contains(road.First) || !map.Locations.Contains(road.Second))
                    throw new InvalidOperationException(
                        $"[WorldMapValidation] The road between {road.First.Name} and {road.Second.Name} leads off the map"
                    );

                if (road.First == road.Second)
                    throw new InvalidOperationException(
                        $"[WorldMapValidation] A road leads from {road.First.Name} back to itself"
                    );

                if (seen.Any(other => other.Connects(road.First) && other.Connects(road.Second)))
                    throw new InvalidOperationException(
                        $"[WorldMapValidation] {road.First.Name} and {road.Second.Name} are joined by more than one road"
                    );

                seen.Add(road);
            }
        }

        /// <summary>
        /// A road drawn through a location would read as two roads meeting there. It also
        /// covers two roads from one location running over each other, since then the
        /// nearer far end lies on the longer road.
        /// </summary>
        private static void EnsureRoadsMissLocations(WorldMap map)
        {
            foreach (var road in map.Roads)
            {
                var passedThrough = map.Locations.FirstOrDefault(location =>
                    !road.Connects(location)
                    && Orientation(road.First, road.Second, location) == 0
                    && LiesWithinBounds(road.First, road.Second, location)
                );
                if (passedThrough != null)
                    throw new InvalidOperationException(
                        $"[WorldMapValidation] The road between {road.First.Name} and {road.Second.Name} runs through {passedThrough.Name}"
                    );
            }
        }

        private static void EnsureRoadsDoNotCross(WorldMap map)
        {
            for (var i = 0; i < map.Roads.Count; i++)
            for (var j = i + 1; j < map.Roads.Count; j++)
            {
                var a = map.Roads[i];
                var b = map.Roads[j];

                // Roads out of the same location meet there by design; any other contact
                // between them is a location lying on a road, caught above.
                if (a.Connects(b.First) || a.Connects(b.Second))
                    continue;

                if (Intersect(a, b))
                    throw new InvalidOperationException(
                        $"[WorldMapValidation] The road between {a.First.Name} and {a.Second.Name} crosses the one between {b.First.Name} and {b.Second.Name}"
                    );
            }
        }

        private static void EnsureEverythingIsReachable(WorldMap map)
        {
            var reached = new HashSet<Location> { map.Tower };
            var frontier = new Queue<Location>(reached);
            while (frontier.Count > 0)
            {
                foreach (var neighbor in map.GetNeighbors(frontier.Dequeue()))
                {
                    if (reached.Add(neighbor))
                        frontier.Enqueue(neighbor);
                }
            }

            var cutOff = map.Locations.FirstOrDefault(location => !reached.Contains(location));
            if (cutOff != null)
                throw new InvalidOperationException(
                    $"[WorldMapValidation] No road leads from {map.Tower.Name} to {cutOff.Name}"
                );
        }

        private static bool Intersect(Road a, Road b)
        {
            var o1 = Orientation(a.First, a.Second, b.First);
            var o2 = Orientation(a.First, a.Second, b.Second);
            var o3 = Orientation(b.First, b.Second, a.First);
            var o4 = Orientation(b.First, b.Second, a.Second);

            if (o1 != o2 && o3 != o4)
                return true;

            // Collinear roads only meet if they overlap.
            return o1 == 0 && LiesWithinBounds(a.First, a.Second, b.First)
                || o2 == 0 && LiesWithinBounds(a.First, a.Second, b.Second)
                || o3 == 0 && LiesWithinBounds(b.First, b.Second, a.First)
                || o4 == 0 && LiesWithinBounds(b.First, b.Second, a.Second);
        }

        /// <summary>
        /// Which side of the line from <paramref name="from"/> to <paramref name="to"/> the
        /// point is on: 1 or -1, or 0 when it lies on the line itself.
        /// </summary>
        private static int Orientation(Location from, Location to, Location point) =>
            Math.Sign(
                (long)(to.X - from.X) * (point.Y - from.Y)
                    - (long)(to.Y - from.Y) * (point.X - from.X)
            );

        private static bool LiesWithinBounds(Location from, Location to, Location point) =>
            point.X >= Math.Min(from.X, to.X)
            && point.X <= Math.Max(from.X, to.X)
            && point.Y >= Math.Min(from.Y, to.Y)
            && point.Y <= Math.Max(from.Y, to.Y);
    }
}
