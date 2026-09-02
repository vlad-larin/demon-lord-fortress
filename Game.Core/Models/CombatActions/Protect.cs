using System.Collections.Generic;
using System.Linq;
using GameCore.Extensions;
using GameCore.Models.Conditions;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Protect : CombatActionBase
    {
        public int ProtectRounds { get; private set; }

        public Protect(int protectRounds)
            : base("Protect")
        {
            ProtectRounds = protectRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) =>
            target == actor ? 0 : actor.Hp / 2;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetAllies(actor, combatants);

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            var gameEvents = new List<GameEventBase>();

            if (target == null || target == actor)
            {
                gameEvents.Add(new SimpleGameEvent($"{actor.Class} braces for the attack"));
                return gameEvents;
            }

            gameEvents.Add(new SimpleGameEvent($"{actor.Class} steps in front of {target.Class}"));

            var protection = target.GetCondition<Protected>();
            if (protection == null)
                target.Conditions.Add(new Protected(actor, ProtectRounds));
            else
                protection.RenewProtection(actor, ProtectRounds);

            // TEMPORARY: replace with condition processing during executions. Damage reduction
            // cannot be applied yet, so protecting means soaking the blows instead: incoming
            // attacks aimed at the ward are pointed at the protector.
            var incomingAttacks = encounter
                .Intents.Where(i =>
                    i.Target == target
                    && !i.IsExecuted
                    && i.Actor.Side != actor.Side
                    && i.Action.GetDamage(i.Actor, target) > 0
                )
                .ToList();

            foreach (var incomingAttack in incomingAttacks)
            {
                incomingAttack.Target = actor;
                gameEvents.Add(
                    new SimpleGameEvent(
                        $"{incomingAttack.Actor.Class} has to hit {actor.Class} instead"
                    )
                );
            }

            return gameEvents;
        }
    }
}
