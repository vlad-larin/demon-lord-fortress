using System.Collections.Generic;
using System.Linq;
using GameCore.Extensions;
using GameCore.Models.Conditions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Blasphemy : CombatActionBase
    {
        private static readonly CharacterClass[] HolyClasses = { CharacterClass.Paladin };

        public int TauntRounds { get; private set; }

        public Blasphemy(int tauntRounds)
            : base("Blasphemy (taunt holy heroes)")
        {
            TauntRounds = tauntRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => new List<Combatant> { actor };

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new SimpleGameEvent($"{actor.Class} shouts unspeakable blasphemies!"));

            var holyEnemies = GetEnemies(actor, encounter.Combatants)
                .Where(c => HolyClasses.Contains(c.Class))
                .ToList();

            foreach (var holyEnemy in holyEnemies)
            {
                var taunted = holyEnemy.GetCondition<Taunted>();
                if (taunted == null)
                    holyEnemy.Conditions.Add(new Taunted(actor, TauntRounds));
                else
                    taunted.Retaunt(actor, TauntRounds);

                gameEvents.Add(
                    new SimpleGameEvent($"{holyEnemy.Class} is enraged for {TauntRounds} rounds")
                );
            }
            return gameEvents;
        }
    }
}
