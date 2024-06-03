using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisableGameObject();
        }
    }

    public void DisableGameObject()
    {
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Object to disable is not assigned.");
        }
    }
}
