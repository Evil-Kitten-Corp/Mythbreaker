// Decompiled with JetBrains decompiler
// Type: HeadTracking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using DG.Tweening;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;
using UnityEngine.Animations.Rigging;

#nullable disable
public class HeadTracking : MonoBehaviour
{
  private Character GetOwner;
  [Header("[Head Tracking]")]
  
  public Rig HeadRig;
  public Transform AimTargetTransform;
  public LayerMask TargetLayer;
  public float TrackingRadius = 10f;
  public float RetargetSpeed = 1f;
  public float WeightSpeed = 3f;
  public float MaxAngle = 90f;
  public float CurrentRigWeight;
  public Vector3 TargetPosition;
  private float RadiusSqr;
  private Vector3 OriginPos;
  [Header("[Debug]")]
  public bool IsDrawDebug;

  private void Start() => this.Initiate();

  private void Update() => this.Tracking();

  private void OnDrawGizmos()
  {
    if (!this.IsDrawDebug)
      return;
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(this.AimTargetTransform.position, 0.1f);
  }

  private void Initiate()
  {
    this.GetOwner = this.GetComponent<Character>();
    this.RadiusSqr = this.TrackingRadius * this.TrackingRadius;
    this.OriginPos = this.AimTargetTransform.position;
  }

  private void Tracking()
  {
    Transform transform1 = (Transform) null;
    Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, this.TrackingRadius, (int) this.TargetLayer);
    float num1 = float.PositiveInfinity;
    Transform transform2 = (Transform) null;
    foreach (Collider collider in colliderArray)
    {
      if ((bool) (Object) collider.GetComponentInParent<Character>() && (Object) collider.GetComponentInParent<Character>() != (Object) this.GetOwner)
      {
        float num2 = Vector3.Distance(collider.transform.position, this.transform.position);
        Vector3 to = collider.transform.position - this.transform.position;
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          transform2 = collider.transform;
        }
        transform1 = (double) to.sqrMagnitude >= (double) this.RadiusSqr ? (Transform) null : ((double) Vector3.Angle(this.transform.forward, to) >= (double) this.MaxAngle ? (Transform) null : transform2);
      }
    }
    if ((Object) transform1 != (Object) null && colliderArray.Length != 0)
    {
      this.TargetPosition = transform1.GetComponentInParent<Animator>().GetBoneTransform(HumanBodyBones.Head).position;
      this.CurrentRigWeight = 1f;
    }
    else
    {
      this.TargetPosition = this.GetOwner.transform.position + this.GetOwner.transform.TransformDirection(0.0f, 1.6f, 2f);
      this.CurrentRigWeight = 0.0f;
    }
    this.AimTargetTransform.DOMove(this.TargetPosition, this.RetargetSpeed);
    this.HeadRig.weight = Mathf.Lerp(this.HeadRig.weight, this.CurrentRigWeight, Time.deltaTime * this.WeightSpeed);
  }

  private void Tracking_Aiming()
  {
    this.AimTargetTransform.DOMove(this.transform.position + Camera.main.transform.forward * 50f, this.RetargetSpeed);
    this.HeadRig.weight = Mathf.Lerp(this.HeadRig.weight, 1f, Time.deltaTime * this.WeightSpeed);
  }

  private bool CheckTarget(Transform target)
  {
    if ((Object) target == (Object) null)
      return false;
    Vector3 position1 = this.GetOwner.CharacterAnim.GetBoneTransform(HumanBodyBones.Head).position;
    Vector3 position2 = target.GetComponentInParent<Character>().CharacterAnim.GetBoneTransform(HumanBodyBones.Chest).position;
    if (Physics.Raycast(position1, this.GetTargetDirection(position1, position2, false), out RaycastHit _, this.TrackingRadius, this.GetOwner.LocomotionData.GroundLayer.value))
    {
      Debug.DrawRay(position1, this.GetTargetDirection(position1, position2, false) * this.GetTargetDistance(target), Color.yellow);
      return false;
    }
    Debug.DrawRay(position1, this.GetTargetDirection(position1, position2, false) * this.GetTargetDistance(target), Color.red);
    return true;
  }

  private float GetTargetDistance(Transform target)
  {
    return Vector3.Distance(this.transform.position, target.position);
  }

  private Vector3 GetTargetDirection(Vector3 startPosition, Vector3 endPosition, bool isIgnoreY = true)
  {
    Vector3 vector3 = startPosition - endPosition;
    if (isIgnoreY)
      vector3.y = 0.0f;
    return -vector3.normalized;
  }
}
