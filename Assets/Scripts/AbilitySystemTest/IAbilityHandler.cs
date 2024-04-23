namespace AbilitySystemTest
{
    public interface IAbilityHandler
    {
        void SetNext(IAbilityHandler handler);
        void HandleRequest(AbilityRequest abilityRequest);
    }
}