using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class PaladinFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Paladin,
                Side = ConflictSide.Heroes,
                MaxHp = 12,
                Hp = 12,
                Actions = new List<CombatActionBase>()
                {
                    new Smite(holyDamage: 3),
                    new LayHands(heal: 5),
                    new Protect(protectRounds: 5),
                },
            };
    }
}
