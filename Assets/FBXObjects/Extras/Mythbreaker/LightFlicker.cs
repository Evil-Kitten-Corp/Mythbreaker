using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.0f;
    public float oscillationSpeed = 5.0f;

    private Light pointLight;

    void Start()
    {
        pointLight = GetComponent<Light>();
        if (pointLight == null)
        {
            Debug.LogError("LightFlicker script requires a Point Light component on the same GameObject.");
            enabled = false;
        }
    }

    void Update()
    {
        // Use Mathf.PingPong to create a rapid oscillation between minIntensity and maxIntensity
        float intensity = Mathf.PingPong(Time.time * oscillationSpeed, 1.0f);
        pointLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, intensity);
    }
}