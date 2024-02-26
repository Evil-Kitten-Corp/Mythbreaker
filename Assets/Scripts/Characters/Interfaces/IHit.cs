using ExtEvents;

namespace Characters.Interfaces
{
    public interface IHit
    {
        ExtEvent OnDamaged { get; }

        float GetHealth();
        int GetTeam();
        void TakeDamage(int amount);
    }
}