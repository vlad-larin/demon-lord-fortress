using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models;

namespace GameConsoleApp.StateHandlers
{
    /// <summary>
    /// The single point where the world map becomes a picture: every location as its label
    /// in brackets where it lies, every road as a straight line of characters between two
    /// of them. The map is stretched to fill the canvas, so only the relative positions the
    /// scenario gave matter.
    /// </summary>
    internal static class WorldMapRenderer
    {
        private const int MarkerWidth = 3;

        public static IEnumerable<string> Draw(
            WorldMap map,
            IReadOnlyDictionary<Location, string> labels,
            int width,
            int height
        )
        {
            var canvas = new char[height, width];
            for (var row = 0; row < height; row++)
            for (var column = 0; column < width; column++)
                canvas[row, column] = ' ';

            var spots = Project(map.Locations, width, height);

            foreach (var road in map.Roads)
                DrawRoad(canvas, spots[road.First], spots[road.Second]);

            // Markers go on last, so a road ends at the bracket rather than running into it.
            var marked = new Dictionary<(int Row, int Column), Location>();
            foreach (var location in map.Locations)
                DrawMarker(canvas, marked, location, spots[location], labels[location]);

            for (var row = 0; row < height; row++)
            {
                var line = new char[width];
                for (var column = 0; column < width; column++)
                    line[column] = canvas[row, column];
                yield return new string(line);
            }
        }

        /// <summary>
        /// Where each location's marker is centered on the canvas. A marker is wider than a
        /// character, so the outermost columns are kept clear for its brackets.
        /// </summary>
        private static Dictionary<Location, (int Row, int Column)> Project(
            IReadOnlyCollection<Location> locations,
            int width,
            int height
        )
        {
            var minX = locations.Min(location => location.X);
            var maxX = locations.Max(location => location.X);
            var minY = locations.Min(location => location.Y);
            var maxY = locations.Max(location => location.Y);
            var margin = MarkerWidth / 2;

            return locations.ToDictionary(
                location => location,
                location =>
                    (
                        Scale(location.Y - minY, maxY - minY, height - 1),
                        margin + Scale(location.X - minX, maxX - minX, width - 1 - 2 * margin)
                    )
            );
        }

        private static int Scale(int offset, int span, int size) =>
            span == 0 ? size / 2 : (int)Math.Round((double)offset * size / span);

        /// <summary>
        /// One stroke for the whole road, picked from its slope as it looks on screen - a
        /// character cell is about twice as tall as it is wide - so a road reads as a line
        /// rather than as a staircase of mixed characters.
        /// </summary>
        private static void DrawRoad(
            char[,] canvas,
            (int Row, int Column) from,
            (int Row, int Column) to
        )
        {
            var dx = to.Column - from.Column;
            var dy = to.Row - from.Row;
            var run = Math.Abs(dx);
            var rise = Math.Abs(dy) * 2;

            var stroke =
                rise * 2 <= run ? '-'
                : run * 2 <= rise ? '|'
                : (dx > 0) == (dy > 0) ? '\\'
                : '/';

            // Bresenham, so the line steps evenly whichever way it leans.
            var stepX = Math.Sign(dx);
            var stepY = Math.Sign(dy);
            var error = run - Math.Abs(dy);
            var (row, column) = from;
            while (true)
            {
                canvas[row, column] = stroke;
                if (row == to.Row && column == to.Column)
                    break;

                var doubled = 2 * error;
                if (doubled > -Math.Abs(dy))
                {
                    error -= Math.Abs(dy);
                    column += stepX;
                }
                if (doubled < run)
                {
                    error += run;
                    row += stepY;
                }
            }
        }

        private static void DrawMarker(
            char[,] canvas,
            Dictionary<(int Row, int Column), Location> marked,
            Location location,
            (int Row, int Column) spot,
            string label
        )
        {
            var marker = $"[{label}]";
            if (marker.Length != MarkerWidth)
                throw new NotImplementedException(
                    $"[WorldMapRenderer] {location.Name} is labeled '{label}', and only single-character labels fit a marker"
                );

            var start = spot.Column - MarkerWidth / 2;
            for (var offset = 0; offset < MarkerWidth; offset++)
            {
                var cell = (spot.Row, start + offset);
                if (marked.TryGetValue(cell, out var other))
                    throw new NotImplementedException(
                        $"[WorldMapRenderer] {location.Name} and {other.Name} lie too close together to draw apart"
                    );

                marked.Add(cell, location);
                canvas[spot.Row, start + offset] = marker[offset];
            }
        }
    }
}
