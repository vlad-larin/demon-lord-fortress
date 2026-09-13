using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class WizardFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Wizard,
                Side = ConflictSide.Heroes,
                MaxHp = 6,
                Hp = 6,
                Actions = new List<CombatActionBase>()
                {
                    new Blast(damage: 5),
                    new Ward(wardRounds: 5, durability: 5),
                    new Stun(stunRounds: 5),
                },
            };
    }
}
