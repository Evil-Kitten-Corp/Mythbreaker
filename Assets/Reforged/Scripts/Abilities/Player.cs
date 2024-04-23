using UnityEngine;

namespace Abilities
{
    public class Player : MonoBehaviour
    {
        public void SetUpAbilities()
        {
            foreach (var p in GlobalPerksSettings.Instance.allPerks)
            {
                p.associated.enabled = p.owned;
            }
        }
    }
}