using UnityEngine;

namespace Abilities
{
    public class PerkCollection : UIMenu
    {
        public GameObject perkPrefab;
        public Transform parentTransform;

        protected override void Initialise()
        {
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Destroy(parentTransform.GetChild(i));
            }
            
            foreach (var perk in GlobalPerksSettings.Instance.allPerks)
            {
                var p = Instantiate(perkPrefab, parentTransform);
                var pobj = p.GetComponent<PerkUI>();

                if (GlobalPerksSettings.Instance.discoveredPerks.Contains(perk.ability))
                {
                    pobj.SetState(true, perk.ability);
                }
                else
                {
                    pobj.SetState(false);
                }
            }
        }
    }
}