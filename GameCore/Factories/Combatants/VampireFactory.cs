using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class VampireFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Vampire,
                Side = ConflictSide.DemonLord,
                MaxHp = 8,
                Hp = 8,
                PerceivedDanger = 12,
                Actions = new List<CombatActionBase>() { new Bite(damage: 3) },
            };
    }
}
