using UnityEngine;
using UnityEngine.EventSystems;

namespace Abilities
{
    public class AbilitySlot: MonoBehaviour, IDropHandler
    {
        public KeyCode associatedKey;
        public AbilityView abilityInSlot;
        
        public AbilityController Controller => abilityInSlot.abilityController;


        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent<AbilityView>(out var view))
            {
                if (abilityInSlot == null)
                {
                    SwapAbility(view);
                }
                else
                {
                    AbilitySlot other = eventData.pointerDrag.GetComponentInParent<AbilitySlot>();
                    
                    if (other != null)
                    {
                        AbilityView current = abilityInSlot;
                        
                        SwapAbility(other.abilityInSlot);
                        other.SwapAbility(current);
                    }
                    else if (eventData.pointerDrag.TryGetComponent<AbilityView>(out var view2))
                    {
                        abilityInSlot.ReturnToInventory();
                        SwapAbility(view2);
                    }
                }
            }
        }

        public void SwapAbility(AbilityView newAbility)
        {
            abilityInSlot = newAbility;
                
            //newAbility.GetComponent<RectTransform>().anchoredPosition =
            //    GetComponent<RectTransform>().anchoredPosition;
                
            newAbility.transform.parent = transform;
            newAbility.transform.SetAsFirstSibling();
            newAbility.transform.localPosition = Vector3.zero;

            Controller.abilityKey = associatedKey;
        }
    }
}