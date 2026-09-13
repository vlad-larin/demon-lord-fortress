using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class GiantRatFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.GiantRat,
                Side = ConflictSide.DemonLord,
                MaxHp = 2,
                Hp = 2,
                PerceivedDanger = 2,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 1) },
            };
    }
}
