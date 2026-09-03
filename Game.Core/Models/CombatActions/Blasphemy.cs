using System.Collections.Generic;
using System.Linq;
using GameCore.Extensions;
using GameCore.Models.Conditions;
using GameCore.Models.GameEvents;
using GameCore.Models.HeroPartyStrategies.Helpers;

namespace GameCore.Models.CombatActions
{
    public class Blasphemy : CombatActionBase
    {
        private static readonly CharacterClass[] HolyClasses = { CharacterClass.Paladin };

        public Blasphemy()
            : base("Blasphemy (taunt holy heroes)") { }

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
                    holyEnemy.Conditions.Add(new Taunted(actor));
                else
                    taunted.Retaunt(actor);

                // TEMPORARY: replace with condition processing during executions
                var retaliation = FindStrongestAttack(holyEnemy, actor);
                if (retaliation == null)
                {
                    gameEvents.Add(
                        new SimpleGameEvent(
                            $"{holyEnemy.Class} is enraged, but has no way to strike back"
                        )
                    );
                    continue;
                }

                var tauntedIntents = encounter
                    .Intents.Where(i => i.Actor == holyEnemy && !i.IsExecuted)
                    .ToList();
                if (tauntedIntents.Count == 0)
                    continue;

                foreach (var tauntedIntent in tauntedIntents)
                {
                    tauntedIntent.Action = retaliation;
                    tauntedIntent.Target = actor;
                }

                gameEvents.Add(
                    new SimpleGameEvent(
                        $"{holyEnemy.Class} abandons the plan to punish the {actor.Class}"
                    )
                );
            }
            return gameEvents;
        }

        private static CombatActionBase FindStrongestAttack(Combatant actor, Combatant target) =>
            AttackCalculator.FindStrongestAttack(actor, new Combatant[] { target }).Action;
    }
}
