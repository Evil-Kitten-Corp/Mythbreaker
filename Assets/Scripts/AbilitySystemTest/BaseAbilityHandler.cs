using System;

namespace AbilitySystemTest
{
    abstract class BaseAbilityHandler : IAbilityHandler
    {
        protected IAbilityHandler _nextHandler;

        public void SetNext(IAbilityHandler handler)
        {
            _nextHandler = handler;
        }

        public void HandleRequest(AbilityRequest abilityRequest)
        {
            if (CanHandle(abilityRequest))
            {
                Handle(abilityRequest);
            }
            else if (_nextHandler != null)
            {
                _nextHandler.HandleRequest(abilityRequest);
            }
            else
            {
                Console.WriteLine("No handler available to process the request.");
                CancelRequest(abilityRequest);
            }
        }

        private void CancelRequest(AbilityRequest abilityRequest)
        {
            
        }

        protected abstract bool CanHandle(AbilityRequest abilityRequest);
        protected abstract void Handle(AbilityRequest abilityRequest);
    }

    class AbilityRequirementHandler : BaseAbilityHandler
    {
        protected override bool CanHandle(AbilityRequest abilityRequest)
        {
            if (!abilityRequest.ability.Cooldown()) return true;
            if (abilityRequest.ability.mana > 0) return true;
            
            _nextHandler = null;
            return false;
        }

        protected override void Handle(AbilityRequest abilityRequest)
        {
            abilityRequest.ReserveManaFromOwner();
        }
    }
    
    class AbilityTargetHandler: BaseAbilityHandler
    {
        protected override bool CanHandle(AbilityRequest abilityRequest)
        {
            return abilityRequest.ConcreteTarget == null;
        }

        protected override void Handle(AbilityRequest abilityRequest)
        {
            
        }
    }
}