using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{

    Animator anim;

    Transform leftFoot;
    Transform rightFoot;

    Transform rightShoulder;
    Transform leftShoulder;

    Transform head;

    Vector3 leftFoot_pos;
    Vector3 rightFoot_pos;

    Quaternion leftFoot_rot;
    Quaternion rightFoot_rot;

    Vector3 leftHand_pos;
    Vector3 rightHand_pos;

    Quaternion leftHand_rot;
    Quaternion rightHand_rot;

    float leftFoot_Weight;
    float rightFoot_Weight;

    float leftHand_Weight;
    float rightHand_Weight;

    public Transform lookAtThis;

    public LayerMask hitMask;

    private void Start()
    {
        anim = GetComponent<Animator>();
        leftFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
        leftShoulder = anim.GetBoneTransform(HumanBodyBones.LeftShoulder);
        rightShoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder);
        head = anim.GetBoneTransform(HumanBodyBones.Head);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // foot IK
        leftFoot_Weight = anim.GetFloat("LeftFoot");
        rightFoot_Weight = anim.GetFloat("RightFoot");
        // find raycast positions
        FindFloorPositions(leftFoot, ref leftFoot_pos, ref leftFoot_rot, Vector3.up);
        FindFloorPositions(rightFoot, ref rightFoot_pos, ref rightFoot_rot, Vector3.up);

        // replace the weights with leftFoot_Weight, and rightFoot_Weight when you've set them in your animation's Curves
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFoot_Weight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFoot_Weight);

        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFoot_Weight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFoot_Weight);

        // set the position of the feet
        // set the rotation of the feet
        if (rightFoot_pos != Vector3.zero)
        {
            anim.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot_pos);
            anim.SetIKRotation(AvatarIKGoal.RightFoot, rightFoot_rot);
        }
        if(leftFoot_pos != Vector3.zero)
        {
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot_pos);
            anim.SetIKRotation(AvatarIKGoal.LeftFoot, leftFoot_rot);
        }
      
        // hand IK

        // find raycast positions
        FindFloorPositions(leftShoulder, ref leftHand_pos, ref leftHand_rot, -transform.forward);
        FindFloorPositions(rightShoulder, ref rightHand_pos, ref rightHand_rot, -transform.forward);

        // distance between hands and raycast hit
        float distanceRightArmObject = Vector3.Distance(rightShoulder.position, rightHand_pos);
        float distanceLeftArmObject = Vector3.Distance(leftShoulder.position, leftHand_pos);
        // blend weight based on the distance
        leftHand_Weight = Mathf.Clamp01(Mathf.Clamp01(1- distanceLeftArmObject) * 2) * anim.GetFloat("HandIK");
      
        rightHand_Weight = Mathf.Clamp01(Mathf.Clamp01(1 - distanceRightArmObject) * 2) * anim.GetFloat("HandIK");


        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHand_Weight);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHand_Weight);

        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHand_Weight);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHand_Weight);

        // set the position of the hand
        if(leftHand_pos != Vector3.zero)
        {
            anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand_pos);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHand_rot);
        }
       if(rightHand_pos != Vector3.zero)
        {
            anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand_pos);

            // set the rotation of the hand

            anim.SetIKRotation(AvatarIKGoal.RightHand, rightHand_rot);
        }
        
        // head IK
        if (lookAtThis != null)
        {
            // distance between face and object to look at
            float distanceFaceObject = Vector3.Distance(head.position, lookAtThis.position);

            anim.SetLookAtPosition(lookAtThis.position);
            // blend based on the distance
            anim.SetLookAtWeight(Mathf.Clamp01(2 - distanceFaceObject), Mathf.Clamp01(1 - distanceFaceObject));
        }
    }

    void FindFloorPositions(Transform t, ref Vector3 targetPosition, ref Quaternion targetRotation, Vector3 direction)
    {
        RaycastHit hit;
        Vector3 rayOrigin = t.position;
        // move the ray origin back a bit
        rayOrigin += direction * 0.3f;

        // raycast in the given direction
        Debug.DrawRay(rayOrigin, -direction, Color.green);
        if (Physics.Raycast(rayOrigin, -direction, out hit, 2, hitMask))
        {
            // the hit point is the position of the hand/foot
            targetPosition = hit.point;
            // then rotate based on the hit normal
            Quaternion rot = Quaternion.LookRotation(transform.forward);
            targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * rot;
        }
        else
        {
            targetPosition = Vector3.zero;
        }
    }


}