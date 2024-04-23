using System;

namespace AbilitySystemTest
{
    public interface IAbilityTemplate
    {
        public void Activate()
        {
            Prepare();
            Cast();
            Cooldown();
        }

        public void Prepare();
        public void Cast();
        public bool Cooldown();
    }
    
    public interface IAbilityAffected
    {
        void Activate();
        void Attach(IObserver observer);
        void Detach(IObserver observer);
    }

    public interface IObserver
    {
        void Update();
    }
    
    class UIEntity : IObserver
    {
        private readonly string name;

        public UIEntity(string name)
        {
            this.name = name;
        }

        public void Update()
        {
            Console.WriteLine($"{name} activated.");
        }
    }

    public interface IAbilityState
    {
        bool Activate();
        bool Cooldown();
    }
    
    class AbilityReadyState : IAbilityState
    {
        private readonly Ability ability;

        public AbilityReadyState(Ability ability)
        {
            this.ability = ability;
        }

        public bool Activate()
        {
            Console.WriteLine("Casting the ability!");
            ability.SetState(new AbilityCastingState(ability));
            return true;
        }

        public bool Cooldown()
        {
            Console.WriteLine("Ability is already ready.");
            return false;
        }
    }

    class AbilityCastingState : IAbilityState
    {
        private readonly Ability ability;

        public AbilityCastingState(Ability ability)
        {
            this.ability = ability;
        }

        public bool Activate()
        {
            Console.WriteLine("Ability is already casting.");
            return false;
        }

        public bool Cooldown()
        {
            Console.WriteLine("Ability cannot cooldown while casting.");
            return false;
        }
    }

    class AbilityCooldownState : IAbilityState
    {
        private readonly Ability ability;

        public AbilityCooldownState(Ability ability)
        {
            this.ability = ability;
        }

        public bool Activate()
        {
            Console.WriteLine("Ability is on cooldown.");
            return false;
        }

        public bool Cooldown()
        {
            Console.WriteLine("Ability is already on cooldown.");
            return true;
        }
    }
}