namespace GameCore.Models.CombatActions
{
    public class Blasphemy : CombatActionBase
    {
        public Blasphemy()
            : base("Blasphemy (taunt holy heroes)") { }

        public override int GetDamage(Combatant actor, Combatant target) => 0;

        public override int GetProtection(Combatant actor, Combatant target) => 0;
    }
}
