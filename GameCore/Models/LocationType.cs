using GameCore.Attributes;

namespace GameCore.Models
{
    public enum LocationType
    {
        [DisplayName("Unknown")]
        Unknown,

        [DisplayName("Tower")]
        Tower,

        [DisplayName("Capital")]
        Capital,

        [DisplayName("Town")]
        Town,

        [DisplayName("Village")]
        Village,

        [DisplayName("Port")]
        Port,

        [DisplayName("Keep")]
        Keep,

        [DisplayName("Shrine")]
        Shrine,

        [DisplayName("Mine")]
        Mine,

        [DisplayName("Forest")]
        Forest,

        [DisplayName("Swamp")]
        Swamp,

        [DisplayName("Ruins")]
        Ruins,
    }
}
