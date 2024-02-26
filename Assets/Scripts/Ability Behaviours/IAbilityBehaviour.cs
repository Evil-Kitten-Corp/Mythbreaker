using System.Collections;

namespace Ability_Behaviours
{
    public interface IAbilityBehaviour
    {
        public IEnumerator Apply();
        public void Unapply();
    }
}