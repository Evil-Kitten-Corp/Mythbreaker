using UnityEngine;

public class TriggerWarehouse : MonoBehaviour
{
    public TriggeredObject triggeredObject;
    public TriggeredObject triggeredObject2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggeredObject.Trigger();
            triggeredObject2.Trigger();
        }
    }
}