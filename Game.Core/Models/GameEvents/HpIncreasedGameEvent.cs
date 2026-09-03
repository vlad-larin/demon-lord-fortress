namespace GameCore.Models.GameEvents
{
    public class HpIncreasedGameEvent : GameEventBase
    {
        public Combatant Actor { get; }
        public int InitialHp { get; }
        public int HpIncrease { get; }
        public int FinalHp { get; }

        public HpIncreasedGameEvent(Combatant actor, int hpIncrease)
            : base($"{actor.Class}'s HPs increased by {hpIncrease}!")
        {
            Actor = actor;
            InitialHp = actor.Hp;
            HpIncrease = hpIncrease;
            FinalHp = actor.Hp + HpIncrease;
        }
    }
}
