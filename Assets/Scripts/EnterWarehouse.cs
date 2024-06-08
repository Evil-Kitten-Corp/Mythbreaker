using UnityEngine;

public class TriggerWarehouse : MonoBehaviour
{
    public TriggeredObject triggeredObject;
    public TriggeredObject triggeredObject2;
    public GameObject objectToReenable; // New field for the GameObject to re-enable

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggeredObject.Trigger();
            triggeredObject2.Trigger();

            if (objectToReenable != null) // Check if the GameObject is assigned
            {
                objectToReenable.SetActive(true); // Re-enable the GameObject
            }
        }
    }
}
