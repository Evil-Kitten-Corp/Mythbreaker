using System;
using System.Collections.Generic;

namespace AbilitySystemTest
{
    public class Ability: IAbilityTemplate, IAbilityAffected
    {
        private IAbilityState _currentState;
        private readonly List<IObserver> observers = new();

        public int mana;
        public int cooldown;
        public int range;

        public Ability()
        {
            _currentState = new AbilityReadyState(this);
        }

        public void SetState(IAbilityState state)
        {
            _currentState = state;
        }

        void IAbilityTemplate.Prepare()
        {
            
        }

        void IAbilityTemplate.Cast()
        {
            if (!_currentState.Activate() && !_currentState.Cooldown())
            {
                
            }
        }

        public bool Cooldown()
        {
            if (_currentState.Cooldown())
            {
                
            }

            return _currentState.Cooldown();
        }

        public void Activate()
        {
            if (_currentState.Activate())
            {
                Notify();
            }
        }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }
        
        private void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }
    }
}