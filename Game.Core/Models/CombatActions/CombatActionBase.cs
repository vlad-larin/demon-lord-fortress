namespace GameCore.Models.CombatActions
{
    public abstract class CombatActionBase
    {
        public string Name { get; }

        protected CombatActionBase(string name)
        {
            Name = name;
        }

        public abstract int GetDamage(Combatant actor, Combatant target);
        public abstract int GetProtection(Combatant actor, Combatant target);
    }
}
