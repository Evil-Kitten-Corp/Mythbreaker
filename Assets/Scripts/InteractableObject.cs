using UI;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public string selectionPrompt;
    public UnityEvent onInteract;

    private SelectableGroup _selectableGroup;
    private GlobalDefs _defs;
    private GameObject _loadedInstance;

    private void Start()
    {
        _defs = Resources.Load<GlobalDefs>("globalDefs");
        _selectableGroup = GameObject.FindWithTag("SelectableGroup").GetComponent<SelectableGroup>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _loadedInstance = Instantiate(_defs.interactPrefab, _selectableGroup.transform);
            _selectableGroup.onChildInstantiate.Invoke();
            _loadedInstance.GetComponent<SelectableObject>().SetText(selectionPrompt);
            _loadedInstance.GetComponent<SelectableObject>().onSelectEvent = onInteract;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(_loadedInstance);
        }
    }
}