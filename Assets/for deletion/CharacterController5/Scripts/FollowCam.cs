using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FollowCam : MonoBehaviour
{
    [FormerlySerializedAs("Target")] [SerializeField]
    Transform target;

    private void Update()
    {
        transform.LookAt(target);
    }
}
