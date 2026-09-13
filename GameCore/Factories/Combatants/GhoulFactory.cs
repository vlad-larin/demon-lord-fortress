using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class GhoulFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Ghoul,
                Side = ConflictSide.DemonLord,
                MaxHp = 5,
                Hp = 5,
                PerceivedDanger = 7,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 2) },
            };
    }
}
