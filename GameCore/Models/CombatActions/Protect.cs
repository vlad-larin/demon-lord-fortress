using System.Collections.Generic;
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

            gameEvents.Add(
                new SimpleGameEvent(
                    $"{actor.Class} steps in front of {target.Class} for {ProtectRounds} rounds"
                )
            );

            var protection = target.GetCondition<Protected>();
            if (protection == null)
                target.Conditions.Add(new Protected(actor, ProtectRounds));
            else
                protection.RenewProtection(actor, ProtectRounds);

            return gameEvents;
        }
    }
}
