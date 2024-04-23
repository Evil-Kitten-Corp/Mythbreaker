using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class SelectableGroup : MonoBehaviour
    {
        [HideInInspector] public UnityEvent onChildInstantiate;
        public KeyCode interactionKey;
        private List<SelectableObject> _selectables;

        private void Start()
        {
            UpdateList();
            onChildInstantiate.AddListener(UpdateList);
        }

        private void UpdateList()
        {
            _selectables = gameObject.GetComponentsInChildren<SelectableObject>().ToList();
        
            if (_selectables.Count > 0)
            {
                _selectables[0].Select(); 
            }
        }

        private void Update()
        {
            if (_selectables.Count > 0 && GetSelected() != null)
            {
                SelectableObject selected = GetSelected();

                int selectedIndex = _selectables.IndexOf(selected);

                if (Input.GetAxis("Mouse ScrollWheel") < 0f && selectedIndex < _selectables.Count - 1)
                {
                    _selectables[selectedIndex + 1].Select();
                }

                if (Input.GetAxis("Mouse ScrollWheel") > 0f && selectedIndex > 0)
                {
                    _selectables[selectedIndex - 1].Select();
                }

                if (Input.GetKeyDown(interactionKey))
                {
                    selected.onSelectEvent?.Invoke();
                }
            }
        }

        private SelectableObject GetSelected()
        {
            return _selectables.FirstOrDefault(g => g.isSelected);
        }
    }
}
