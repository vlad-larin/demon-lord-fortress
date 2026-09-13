using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class BoneKnightFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.BoneKnight,
                Side = ConflictSide.DemonLord,
                MaxHp = 7,
                Hp = 7,
                PerceivedDanger = 10,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 3) },
            };
    }
}
