using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Models;

namespace GameEngine.Ai
{
    /// <summary>
    /// How a hero party decides where to go next. The Demon Lord never picks the room for
    /// them, so this is the behaviour he has to predict and, later, manipulate.
    /// </summary>
    internal class ExpeditionAi
    {
        private static readonly Random _rnd = new Random();

        private GameInstance GameInstance { get; }

        public ExpeditionAi(GameInstance gameInstance)
        {
            GameInstance = gameInstance;
        }

        /// <summary>
        /// Picks the room the party breaks into on the floor they are standing on. What they
        /// came for beats everything else; without it on this floor they take the way up
        /// that looks cheapest, without being reliable about it.
        /// </summary>
        internal RoomChoice ChooseRoom()
        {
            var expedition = GameInstance.Expedition;
            var floor = GameInstance.Tower.Floors[expedition.CurrentFloorNumber - 1];

            // A floor gives up one room per expedition, so a room already fought through is
            // not on the table any more.
            var rooms = floor.Rooms.Where(room => !room.Cleared).ToList();
            if (rooms.Count == 0)
                throw new InvalidOperationException(
                    $"[ExpeditionAi] Floor {expedition.CurrentFloorNumber} has no room left for the party to enter"
                );

            var objectiveRoom = rooms.FirstOrDefault(room => expedition.Objective.IsHeldBy(room));
            if (objectiveRoom != null)
                return new RoomChoice(
                    objectiveRoom,
                    $"what they came for is behind that door: {expedition.Objective.Name.ToLowerInvariant()}"
                );

            var room = ChooseRandomlyBiasedTowardsLesserThreat(rooms);
            return new RoomChoice(room, "of the ways up, that one looks like the cheapest");
        }

        /// <summary>
        /// A party with nothing to aim for on this floor still has to go through one of the
        /// rooms, and would rather it were the quiet one. Weights mirror the threats the
        /// party can see around the range they span, so the calmest looking room is the most
        /// likely one and the worst looking room is the least - and the extra point keeps
        /// even the worst room possible, and keeps the weights from all being zero when
        /// every room looks the same.
        /// </summary>
        private Room ChooseRandomlyBiasedTowardsLesserThreat(List<Room> rooms)
        {
            var highestThreat = rooms.Max(room => room.PerceivedThreat);
            var lowestThreat = rooms.Min(room => room.PerceivedThreat);

            var weights = rooms
                .Select(room =>
                    (Weight: highestThreat + lowestThreat - room.PerceivedThreat + 1, Room: room)
                )
                .ToList();

            var randomWeight = _rnd.Next(weights.Sum(w => w.Weight));
            foreach (var (weight, room) in weights)
            {
                if (randomWeight < weight)
                    return room;

                randomWeight -= weight;
            }

            throw new InvalidOperationException("[ExpeditionAi] Error with choosing the room!");
        }

        /// <summary>
        /// The room the party walks into, and the reason they gave for it.
        /// </summary>
        internal class RoomChoice
        {
            public Room Room { get; }
            public string Reason { get; }

            public RoomChoice(Room room, string reason)
            {
                Room = room;
                Reason = reason;
            }
        }
    }
}
