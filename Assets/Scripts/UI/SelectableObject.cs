using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SelectableObject: Selectable
    {
        public TMP_Text text;

        [HideInInspector] public UnityEvent onSelectEvent;
        public bool isSelected;
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            isSelected = true;
            DoStateTransition(SelectionState.Highlighted, false);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            isSelected = false;
            DoStateTransition(SelectionState.Normal, false);
        }

        public void SetText(string txt)
        {
            text.text = txt;
        }
    }
}