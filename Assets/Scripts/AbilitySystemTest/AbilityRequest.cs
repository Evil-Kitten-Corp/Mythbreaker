namespace AbilitySystemTest
{
    public class AbilityRequest
    {
        public object ConcreteTarget;
        public int ManaToTake;
        public Ability ability;

        public AbilityRequest(Ability ability)
        {
            this.ability = ability;
        }

        public void Cancel()
        {
            
        }

        public void ReserveManaFromOwner()
        {
            ManaToTake = ability.mana;
        }
    }
}