using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public CameraShaking camShaker;

    public void ShakeCamera()
    {
        camShaker.TriggerShake();
    }
}
