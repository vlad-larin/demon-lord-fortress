using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class GargoyleFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Gargoyle,
                Side = ConflictSide.DemonLord,
                MaxHp = 9,
                Hp = 9,
                PerceivedDanger = 12,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 3) },
            };
    }
}
