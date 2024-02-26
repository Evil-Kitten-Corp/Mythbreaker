using System;
using Characters;
using UnityEngine;

[Serializable]
public class Hook
{
    public RaycastHit hitRay;

    public Vector3 cameraToHitDir;
    public Vector3 handToHitDir;

    public LineRenderer lineRenderer;

    public bool IsHook = false;
    public bool IsRope = false;

    public GameObject handObj;
    public Spring spring;
    public Vector3 currentRopePosition;
}