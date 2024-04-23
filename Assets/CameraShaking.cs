using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaking : MonoBehaviour
{
    private Vector3 _mainCameraOriginalPosition;
    private Quaternion _mainCameraOriginalRotation; 
    private float _mainCameraShakeStrength;
     
    public float intensity = 0.02f; // intensity of CameraShake
    public float decayTime = 0.001f; // how long it takes camera to return to normal
    [HideInInspector] public bool shakeCamera;

    public void Update()
    {
        if (shakeCamera)
        {
            Shake();
            shakeCamera = false;
        }

        if (_mainCameraShakeStrength > 0)
        {
            transform.position = _mainCameraOriginalPosition + Random.insideUnitSphere * _mainCameraShakeStrength;
            transform.rotation = new Quaternion
            (
                _mainCameraOriginalRotation.x + Random.Range(-_mainCameraShakeStrength, _mainCameraShakeStrength) * .2f,
                _mainCameraOriginalRotation.y + Random.Range(-_mainCameraShakeStrength, _mainCameraShakeStrength) * .2f,
                _mainCameraOriginalRotation.z + Random.Range(-_mainCameraShakeStrength, _mainCameraShakeStrength) * .2f,
                _mainCameraOriginalRotation.w + Random.Range(-_mainCameraShakeStrength, _mainCameraShakeStrength) * .2f
            );
            _mainCameraShakeStrength -= decayTime;
        }
    }
    
    public void TriggerShake() => shakeCamera = true;

    private void Shake()
    {
        _mainCameraOriginalPosition = transform.position;
        _mainCameraOriginalRotation = transform.rotation;
        _mainCameraShakeStrength = intensity;
        Debug.Log("Camera shaking!");
    } 
}
