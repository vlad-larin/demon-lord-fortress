namespace GameCore.Models.GameEvents
{
    public class CombatantDiedGameEvent : GameEventBase
    {
        public Combatant Actor { get; }

        public CombatantDiedGameEvent(Combatant actor)
            : base($"{actor.DisplayName} dies!")
        {
            Actor = actor;
        }
    }
}
