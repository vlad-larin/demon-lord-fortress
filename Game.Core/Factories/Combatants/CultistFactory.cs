using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class CultistFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Cultist,
                Side = ConflictSide.DemonLord,
                MaxHp = 3,
                Hp = 3,
                PerceivedDanger = 5,
                Actions = new List<CombatActionBase>()
                {
                    new SimpleAttack(damage: 1),
                    new Blasphemy(),
                    new Ritual(1),
                },
            };
    }
}
