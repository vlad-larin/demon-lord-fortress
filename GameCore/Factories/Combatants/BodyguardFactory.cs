using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class BodyguardFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Bodyguard,
                Side = ConflictSide.DemonLord,
                MaxHp = 16,
                Hp = 16,
                PerceivedDanger = 22,
                Actions = new List<CombatActionBase>() { new SimpleAttack(damage: 4) },
            };
    }
}
