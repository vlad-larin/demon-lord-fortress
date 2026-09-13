using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class AcidSlimeFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.AcidSlime,
                Side = ConflictSide.DemonLord,
                MaxHp = 4,
                Hp = 4,
                PerceivedDanger = 5,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 2) },
            };
    }
}
