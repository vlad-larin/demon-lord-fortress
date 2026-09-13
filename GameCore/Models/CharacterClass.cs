using GameCore.Attributes;

namespace GameCore.Models
{
    public enum CharacterClass
    {
        [DisplayName("Unknown")]
        Unknown,

        [DisplayName("Fighter")]
        Fighter,

        [DisplayName("Paladin")]
        Paladin,

        [DisplayName("Wizard")]
        Wizard,

        [DisplayName("Rogue")]
        Rogue,

        [DisplayName("Cultist")]
        Cultist,

        [DisplayName("Bone Knight")]
        BoneKnight,

        [DisplayName("Vampire")]
        Vampire,

        [DisplayName("Giant Rat")]
        GiantRat,

        [DisplayName("Acid Slime")]
        AcidSlime,

        [DisplayName("Ghoul")]
        Ghoul,

        [DisplayName("Gargoyle")]
        Gargoyle,

        [DisplayName("Bodyguard")]
        Bodyguard,

        [DisplayName("Lieutenant")]
        Lieutenant,

        [DisplayName("Demon Lord")]
        DemonLord,
    }
}
