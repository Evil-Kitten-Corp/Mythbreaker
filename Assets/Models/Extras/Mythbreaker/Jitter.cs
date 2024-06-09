using UnityEngine;

public class Jitter : MonoBehaviour
{
    public float maxJitterDistance = 0.2f;
    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the object
        originalPosition = transform.position;
    }

    void Update()
    {
        // Generate random offsets for x and y axes within the specified maxJitterDistance
        float xOffset = Random.Range(-maxJitterDistance, maxJitterDistance);
        float yOffset = Random.Range(-maxJitterDistance, maxJitterDistance);

        // Apply the random offsets to the object's position while keeping it within the original range
        Vector3 newPosition = originalPosition + new Vector3(xOffset, yOffset, 0f);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
    }
}