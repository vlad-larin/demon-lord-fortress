using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class DemonLordFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.DemonLord,
                Side = ConflictSide.DemonLord,
                MaxHp = 22,
                Hp = 22,
                PerceivedDanger = 40,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 6) },
            };
    }
}
