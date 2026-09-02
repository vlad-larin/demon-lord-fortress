using System.Collections.Generic;
using System.Linq;
using GameCore.Models.GameEvents;

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

            var holyEnemies = encounter
                .Combatants.Where(c => c.Side != actor.Side && HolyClasses.Contains(c.Class))
                .ToList();

            foreach (var holyEnemy in holyEnemies)
            {
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

                // Rewriting the intents of the taunted hero in place is enough to break their
                // plan. Intents that were already resolved this round are rewritten too, but
                // that is harmless: the list is rebuilt every planning phase.
                var tauntedIntents = encounter.Intents.Where(i => i.Actor == holyEnemy).ToList();
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
            actor
                .Actions.Where(action => action.GetDamage(actor, target) > 0)
                .OrderByDescending(action => action.GetDamage(actor, target))
                .FirstOrDefault();
    }
}
