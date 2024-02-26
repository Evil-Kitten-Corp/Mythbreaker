using Characters.Combat.Info;

namespace Characters.Interfaces
{
    public interface IHeal
    {
        int GetTeam();
        void Heal(HealInfoBase healInfo);
    }
}