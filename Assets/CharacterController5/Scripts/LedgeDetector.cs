using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class LedgeDetector : MonoBehaviour
{
    private float distancePlayerToLedge;

    private Vector3 ledgePos;
    private Vector3 ledgeNormal;

    [SerializeField]
    private LayerMask hitMask = 1;

    [SerializeField]
    float grabbingDistance = 0.3f;

    private Transform playerFace;
    private Transform playerSpine;

    private bool Inrange = false;
    private bool hanging = false;
    GameObject ledgeDetector;
    GameObject ledgePosIndicator;

    private Vector3 raycastFloorPos;

    private Vector3 CombinedRaycastNormal;

    RaycastHit hit, hitDown;
    Animator anim;
    CharacterMovement charM;

    Vector3 wallhitPoint;
    Vector3 wallNormal;
    // Use this for initialization
    void Start()
    {
        charM = transform.GetComponent<CharacterMovement>();
        anim = GetComponentInChildren<Animator>();
        playerFace = anim.GetBoneTransform(HumanBodyBones.Head); // these are from the humanoid rig
        playerSpine = anim.GetBoneTransform(HumanBodyBones.Spine);
        SpawnLedgePosIndicator();
        SpawnLedgeDetector();
    }

    // Update is called once per frame
    void Update()
    {
        // draw ray to find walls, from the player chest
        Debug.DrawRay(playerSpine.position, transform.forward, Color.red);
        if (Physics.Raycast(playerSpine.position, transform.forward, out hit, 1, hitMask))
        {
            Debug.DrawRay(playerSpine.position, transform.forward, Color.green);
            wallhitPoint = hit.point;
            wallNormal = hit.normal;
            Inrange = true;
            charM.WallNormal = hit.normal;
        }
        else
        {
            Inrange = false;
            // drop down if there is nothing in front of us and we are hanging
            if (hanging)
            {
                StopHanging();
            }
        }

        // if in range of wall, shoot ray downward from the ledge detector
        if (Inrange)
        {

            Vector3 down = -ledgeDetector.transform.up;
            Debug.DrawRay(ledgeDetector.transform.position, down * 4f, Color.yellow);
            if (Physics.Raycast(ledgeDetector.transform.position, down, out hitDown, 4f, hitMask))
            {
                Debug.DrawRay(ledgeDetector.transform.position, down * 4f, Color.cyan);
                ledgeNormal = hitDown.normal;
                ledgePos = new Vector3(wallhitPoint.x, hitDown.point.y, wallhitPoint.z);
            }
            else
            {
                //reset the position
                if (ledgePos != Vector3.zero)
                {
                    ledgePos = Vector3.zero;
                }

            }
            // set the ledge indicator block
            ledgePosIndicator.SetActive(true);
            ledgePosIndicator.transform.position = ledgePos;

            // check distance from face to the ledge
            distancePlayerToLedge = Vector3.Distance(ledgePos, playerFace.transform.position);
            //, if its close enough, and the surface is flat on the ledge normal
            if (distancePlayerToLedge < grabbingDistance && ledgeNormal.y > 0.9f && ledgeNormal.y < 1.1f)
            {
                // if not already hanging, grab
                if (!hanging)
                {
                    hanging = true;
                    charM.GrabLedgePos(ledgePos);
                }
            }
        }

        if (hanging)
        {
            // check for places to climbup when hanging
            FindSuitablePlaceForClimbup();
        }
    }

    Vector3 FindSuitablePlaceForClimbup()
    {
        float raycastWidth = 0.2f;
        // check floor on all raycasts
        // average only for those who are not returning zero  
        int floorAverage = 1;
        CombinedRaycastNormal = FloorRaycasts(0, 0, 3f);
        floorAverage += (GetFloorAverage(raycastWidth, 0) + GetFloorAverage(-raycastWidth, 0) + GetFloorAverage(0, -raycastWidth) + GetFloorAverage(0, raycastWidth));
        // if all 5 raycast hit something, and the surface is flat, there is space to climb up.
        Vector3 AverageNormal = CombinedRaycastNormal / floorAverage;
        if (floorAverage == 5 && AverageNormal.y > 0.9f && AverageNormal.y < 1.1f && distancePlayerToLedge < grabbingDistance + 0.4f)
        {
            charM.safeForClimbUp = true;
            // update the ledgeposition so it's in the correct place when moving
            charM.LedgePos = ledgePos;
            ledgePosIndicator.transform.position = ledgePos;
        }
        else
        {
            charM.safeForClimbUp = false;
        }

        return CombinedRaycastNormal / floorAverage;
    }

    // only add to average floor position if its not Vector3.zero
    int GetFloorAverage(float offsetx, float offsetz)
    {

        if (FloorRaycasts(offsetx, offsetz, 2f) != Vector3.zero)
        {
            CombinedRaycastNormal += FloorRaycasts(offsetx, offsetz, 3f);
            return 1;
        }
        else { return 0; }
    }

    Vector3 FloorRaycasts(float offsetx, float offsetz, float raycastLength)
    {
        //  raycast from multiple points
        raycastFloorPos = ledgeDetector.transform.TransformPoint(0 + offsetx, -2f, 0 + offsetz);

        Debug.DrawRay(raycastFloorPos, Vector3.down * raycastLength, Color.red);
        if (Physics.Raycast(raycastFloorPos, Vector3.down, out hit, raycastLength, hitMask, QueryTriggerInteraction.Collide))
        {
            if (hit.normal.y > 0.9f && hit.normal.y < 1.1f)
                Debug.DrawRay(raycastFloorPos, Vector3.down * raycastLength, Color.green);
            return hit.normal;
        }
        else return Vector3.zero;
    }

    void StopHanging()
    {
        // only drop when not currently climbing up or we get stuck
        if (hanging && !anim.GetCurrentAnimatorStateInfo(0).IsName("Climbup"))
        {
            hanging = false;
            charM.CancelHanging();
        }
    }

    void SpawnLedgePosIndicator()
    {
        ledgePosIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ledgePosIndicator.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        ledgePosIndicator.GetComponent<Collider>().enabled = false;
    }

    void SpawnLedgeDetector()
    {
        ledgeDetector = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ledgeDetector.transform.SetParent(transform);
        ledgeDetector.transform.localPosition = new Vector3(0, 4.2f, 0.5f);
        ledgeDetector.GetComponent<Collider>().enabled = false;
    }

}