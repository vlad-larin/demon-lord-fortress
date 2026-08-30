namespace GameCore.Models.CombatActions
{
    public class Stun : CombatActionBase
    {
        public int StunRounds { get; private set; }

        public Stun(int stunRounds)
            : base("Stun")
        {
            StunRounds = stunRounds;
        }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;
    }
}
