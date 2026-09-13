namespace GameCore.Models.GameEvents
{
    public class HpReducedGameEvent : GameEventBase
    {
        public Combatant Actor { get; }
        public int InitialHp { get; }
        public int HpReduction { get; }
        public int FinalHp { get; }

        public HpReducedGameEvent(Combatant actor, int hpReduction)
            : base($"{actor.Class}'s HPs reduced by {hpReduction}!")
        {
            Actor = actor;
            InitialHp = actor.Hp;
            HpReduction = hpReduction;
            FinalHp = actor.Hp - HpReduction;
        }
    }
}
