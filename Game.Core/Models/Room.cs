using System.Collections.Generic;

namespace GameCore.Models
{
    public class Room
    {
        public string Type { get; set; }
        public int Capacity { get; set; }
        public List<Monster> Guardians { get; set; }
        public List<RoomProperty> Properties { get; set; }
        public bool Cleared { get; set; }
    }
}
