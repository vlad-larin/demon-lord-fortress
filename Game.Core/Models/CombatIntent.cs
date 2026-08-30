using GameCore.Models.CombatActions;

namespace GameCore.Models
{
    public class CombatIntent
    {
        public Combatant Actor { get; set; }
        public CombatActionBase Action { get; set; }
        public Combatant Target { get; set; }

        public CombatIntent(Combatant actor, CombatActionBase action, Combatant target)
        {
            Actor = actor;
            @Action = action;
            Target = target;
        }
    }
}
