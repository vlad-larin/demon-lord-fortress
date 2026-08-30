using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class FighterFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Fighter,
                Side = ConflictSide.Heroes,
                MaxHp = 16,
                Hp = 16,
                Actions = new List<CombatActionBase>()
                {
                    new SimpleAttack(damage: 4),
                    new HeadfirstAttack(damage: 7, exposureRounds: 3),
                    new Challenge(riposteCount: 2, riposteDamage: 4),
                },
            };
    }
}
