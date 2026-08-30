using System.Collections.Generic;
using GameCore.Models;
using GameCore.Models.CombatActions;

namespace GameCore.Factories.Combatants
{
    public static class RogueFactory
    {
        public static Combatant BuildCombatant() =>
            new Combatant
            {
                Class = CharacterClass.Rogue,
                Side = ConflictSide.Heroes,
                MaxHp = 8,
                Hp = 8,
                Actions = new List<CombatActionBase>()
                {
                    new SneakyStrike(damage: 3),
                    new Expose(exposeRounds: 5),
                    new Distract(distractRounds: 5),
                },
            };
    }
}
