using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class LieutenantFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Lieutenant,
                Side = ConflictSide.DemonLord,
                MaxHp = 12,
                Hp = 12,
                PerceivedDanger = 26,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 5) },
            };
    }
}
