using System.Collections.Generic;
using GameCore.Models.GameEvents;

namespace GameCore.Models.CombatActions
{
    public class Smite : CombatActionBase
    {
        public int HolyDamage { get; private set; }

        public Smite(int holyDamage)
            : base("Smite")
        {
            HolyDamage = holyDamage;
        }

        public override int GetDamage(Combatant actor, Combatant target) => HolyDamage;

        public override int GetProtection(Combatant actor, Combatant target) => 0;

        public override List<Combatant> GetValidTargets(
            Combatant actor,
            List<Combatant> combatants
        ) => GetEnemies(actor, combatants);

        public override IEnumerable<GameEventBase> Execute(
            Combatant actor,
            Combatant target,
            Encounter encounter
        )
        {
            var gameEvents = new List<GameEventBase>();
            gameEvents.Add(new SimpleGameEvent($"{actor.Class} calls down holy wrath!"));
            gameEvents.Add(new HpReducedGameEvent(target, HolyDamage));

            target.Hp -= HolyDamage;

            return gameEvents;
        }
    }
}
