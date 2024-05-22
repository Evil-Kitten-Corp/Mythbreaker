using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public class IKFootSolver : MonoBehaviour
{
    public Transform body;
    public float footSpacing;
    public LayerMask terrainLayer;

    private Vector3 newPosition;
    private Vector3 oldPosition;
    public float lerp = 0f;
    public float stepDistance = 0.1f;
    public float stepHeight = 0.1f;
    public float speed = 1f;
    private Vector3 currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = body.position; 

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);
        if (Physics.Raycast(ray,out RaycastHit info, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance)
            {
                lerp = 0;
                newPosition = info.point;
            }
        }
        if (lerp < 1)
        {
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = footPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }
}
