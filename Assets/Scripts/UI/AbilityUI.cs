using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Abilities
{
    public class AbilityUI: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image icon;
        [HideInInspector] public AbilityInfo ability;

        [ReadOnly] public Transform parentAfterDrag;

        public void SetAbility(AbilityInfo ability)
        {
            this.ability = ability;
            icon.sprite = ability.icon;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0.5f);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            ability.Unequip();
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            icon.raycastTarget = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentAfterDrag);
            icon.raycastTarget = true;
        }
    }
}