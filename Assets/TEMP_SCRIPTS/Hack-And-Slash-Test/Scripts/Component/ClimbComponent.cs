// Decompiled with JetBrains decompiler
// Type: ClimbComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class ClimbComponent : MonoBehaviour
{
  [Header("[Component]")]
  private Character Character;
  [Header("[Climb Component]")]
  public eClimbMoveDirection ClimbMoveDirection;
  public float CheckDistance = 2f;
  public float CheckAngle = 140f;
  public float MaxHeight = 3f;
  public float MinHeight = 1.5f;
  public float MoveSpeed = 0.5f;
  public bool CheckClimbing;
  public bool IsClimbing;
  public Vector3 ClimbPosition;
  public Vector3 ClimbOffset;
  [Header("[Raycast]")]
  public LayerMask ClimbLayer;
  public RaycastHit HitInfo;
  [Header("[Hand IK]")]
  public bool UseHandIK;
  public bool RightHand;
  public HandIK HandIK;
  public Vector3 RightHandPosition;
  public Vector3 LeftHandPosition;
  public float IKSpeed = 0.4f;
  public float MoveDistance = 0.5f;
  public AnimationCurve MoveCurve;
  [Header("[Coroutine]")]
  private Coroutine C_CheckGround;
  [Header("[Tween]")]
  private DG.Tweening.Sequence MoveSequence;
  [Header("[Draw Gizmos]")]
  public bool IsDrawGizmos;
  public float GizmosRadius = 0.15f;

  private void Awake() => this.Character = this.GetComponent<Character>();

  private void LateUpdate()
  {
    this.CheckClimb();
    this.OnUpdateClimb();
    this.OnClimbEnd();
    this.SetClimbMoveDirection();
  }

  private void OnDrawGizmos()
  {
    if (!this.IsDrawGizmos)
      return;
    Gizmos.color = this.CheckClimbing ? Color.green : Color.red;
    Gizmos.DrawLine(this.transform.position + this.transform.TransformDirection(0.0f, 1.5f, 0.0f), this.transform.position + this.transform.TransformDirection(0.0f, 1.5f, 2f));
    Gizmos.DrawSphere(this.ClimbPosition, this.GizmosRadius);
    if (!this.UseHandIK)
      return;
    Gizmos.color = this.RightHand ? Color.green : Color.red;
    Gizmos.DrawSphere(new Vector3(this.RightHandPosition.x, this.ClimbPosition.y + this.HandIK.RightHand.OffsetPosition.y, this.RightHandPosition.z), this.GizmosRadius * 0.5f);
    Gizmos.color = !this.RightHand ? Color.green : Color.red;
    Gizmos.DrawSphere(new Vector3(this.LeftHandPosition.x, this.ClimbPosition.y + this.HandIK.LeftHand.OffsetPosition.y, this.LeftHandPosition.z), this.GizmosRadius * 0.5f);
  }

  private void CheckClimb()
  {
    Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, this.CheckDistance, this.ClimbLayer.value);
    float num1 = float.PositiveInfinity;
    foreach (Collider collider in colliderArray)
    {
      float num2 = Vector3.Distance(this.transform.position, collider.transform.position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        this.ClimbPosition = collider.ClosestPoint(new Vector3(this.transform.position.x, collider.bounds.max.y, this.transform.position.z));
        if ((double) Vector3.Angle(this.transform.forward, (this.ClimbPosition - this.transform.position).normalized) < (double) this.CheckAngle * 0.5 && (double) this.GetClimbHeight(this.ClimbPosition) <= (double) this.MaxHeight && (double) this.GetClimbHeight(this.ClimbPosition) > (double) this.MinHeight)
        {
          this.CheckClimbing = true;
          this.OnClimbBegin();
        }
        else
          this.CheckClimbing = false;
      }
    }
  }

  public void OnClimbBegin()
  {
    if (CheckClimbing && !IsClimbing && !Character.LocomotionData.IsGrounded)
    {
      IsClimbing = true;
      UseHandIK = true;
      Character.CharacterController.enabled = false;
      Character.LocomotionData.CharacterMoveMode = eCharacterMoveMode.Strafe;
      Character.LocomotionData.GroundLayer = 1 << LayerMask.NameToLayer("Nothing");
      base.transform.DOMove(ClimbPosition + base.transform.TransformDirection(ClimbOffset), 0.5f).OnComplete(delegate
      {
        Character.CharacterAnim.CrossFadeInFixedTime("Climb_Begin", 0.1f);
      });
      Vector3 direction = ClimbPosition - base.transform.position;
      direction.y = 0f;
      base.transform.DORotateQuaternion(Quaternion.LookRotation(direction.normalized), 0.1f);
      RightHandPosition = ClimbPosition + base.transform.TransformDirection(HandIK.RightHand.OffsetPosition);
      LeftHandPosition = ClimbPosition + base.transform.TransformDirection(HandIK.LeftHand.OffsetPosition);
    }
  }

  public void OnUpdateClimb()
  {
    if (IsClimbing)
    {
      ClimbOffset.x = MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().x * MoveSpeed;
      ClimbMoveDirection = ((ClimbOffset.x != 0f) ? ((ClimbOffset.x > 0.1f) ? eClimbMoveDirection.Right : eClimbMoveDirection.Left) : eClimbMoveDirection.None);
      base.transform.DOMoveX(ClimbPosition.x + base.transform.TransformDirection(ClimbOffset).x, 1f);
      base.transform.DOMoveY(ClimbPosition.y + base.transform.TransformDirection(ClimbOffset).y, 0f);
      base.transform.DOMoveZ(ClimbPosition.z + base.transform.TransformDirection(ClimbOffset).z, 1f);
      Vector3 direction = ClimbPosition - base.transform.position;
      direction.y = 0f;
      base.transform.DORotateQuaternion(Quaternion.LookRotation(direction.normalized), 0.1f);
    }
  }

  public void OnClimbEnd()
  {
    if (!this.IsClimbing)
      return;
    if ((double) MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().y > 0.0 && MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Jump.triggered)
    {
      this.Character.CharacterAnim.CrossFadeInFixedTime("Climb_Up", 0.1f);
      this.IsClimbing = false;
      this.Character.LocomotionData.CharacterMoveMode = eCharacterMoveMode.Directional;
      this.transform.DOMove(this.ClimbPosition, 0.3f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        this.UseHandIK = false;
        this.Character.LocomotionData.GroundLayer = (LayerMask) (1 << LayerMask.NameToLayer("Map"));
        this.Character.CharacterController.enabled = true;
        this.ClimbPosition = Vector3.zero;
        this.ClimbOffset.x = 0.0f;
      }));
    }
    if ((double) MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().y >= 0.0 || !MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Jump.triggered)
      return;
    this.Character.CharacterAnim.CrossFadeInFixedTime("Climb_Down", 0.1f);
    this.UseHandIK = false;
    this.Character.LocomotionData.CharacterMoveMode = eCharacterMoveMode.Directional;
    this.Character.LocomotionData.GroundLayer = (LayerMask) (1 << LayerMask.NameToLayer("Map"));
    this.Character.CharacterController.enabled = true;
  }

  private void SetClimbMoveDirection()
  {
    if (!this.IsClimbing)
      return;
    switch (this.ClimbMoveDirection)
    {
      case eClimbMoveDirection.None:
        DOTween.To((DOGetter<Vector3>) (() => this.RightHandPosition), (DOSetter<Vector3>) (x => this.RightHandPosition = x), this.ClimbPosition + this.transform.TransformDirection(this.HandIK.RightHand.OffsetPosition), this.IKSpeed).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.MoveCurve);
        DOTween.To((DOGetter<Vector3>) (() => this.LeftHandPosition), (DOSetter<Vector3>) (x => this.LeftHandPosition = x), this.ClimbPosition + this.transform.TransformDirection(this.HandIK.LeftHand.OffsetPosition), this.IKSpeed).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.MoveCurve);
        break;
      case eClimbMoveDirection.Right:
        Vector3 vector3_1 = new Vector3(this.MoveDistance, 0.0f, 0.0f);
        if ((double) Vector3.Distance(this.ClimbPosition, this.Character.CharacterAnim.GetBoneTransform(HumanBodyBones.RightHand).position) > 0.20000000298023224 && this.RightHand)
          DOTween.To((DOGetter<Vector3>) (() => this.RightHandPosition), (DOSetter<Vector3>) (x => this.RightHandPosition = x), this.ClimbPosition + this.transform.TransformDirection(this.HandIK.RightHand.OffsetPosition + vector3_1), this.IKSpeed).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.RightHand = false)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.MoveCurve);
        if ((double) Vector3.Distance(this.ClimbPosition, this.Character.CharacterAnim.GetBoneTransform(HumanBodyBones.LeftHand).position) <= 0.20000000298023224 || this.RightHand)
          break;
        DOTween.To((DOGetter<Vector3>) (() => this.LeftHandPosition), (DOSetter<Vector3>) (x => this.LeftHandPosition = x), this.ClimbPosition + this.transform.TransformDirection(this.HandIK.LeftHand.OffsetPosition + vector3_1), this.IKSpeed).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.RightHand = true)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.MoveCurve);
        break;
      case eClimbMoveDirection.Left:
        Vector3 vector3_2 = new Vector3(-this.MoveDistance, 0.0f, 0.0f);
        if ((double) Vector3.Distance(this.ClimbPosition, this.Character.CharacterAnim.GetBoneTransform(HumanBodyBones.RightHand).position) > 0.20000000298023224 && this.RightHand)
          DOTween.To((DOGetter<Vector3>) (() => this.RightHandPosition), (DOSetter<Vector3>) (x => this.RightHandPosition = x), this.ClimbPosition + this.transform.TransformDirection(this.HandIK.RightHand.OffsetPosition + vector3_2), this.IKSpeed).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.RightHand = false)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.MoveCurve);
        if ((double) Vector3.Distance(this.ClimbPosition, this.Character.CharacterAnim.GetBoneTransform(HumanBodyBones.LeftHand).position) <= 0.20000000298023224 || this.RightHand)
          break;
        DOTween.To((DOGetter<Vector3>) (() => this.LeftHandPosition), (DOSetter<Vector3>) (x => this.LeftHandPosition = x), this.ClimbPosition + this.transform.TransformDirection(this.HandIK.LeftHand.OffsetPosition + vector3_2), this.IKSpeed).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => this.RightHand = true)).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(this.MoveCurve);
        break;
    }
  }

  public float GetClimbHeight(Vector3 height) => Mathf.Abs(this.transform.position.y - height.y);

  private void OnAnimatorIK(int layerIndex)
  {
    if (!this.UseHandIK)
      return;
    this.Character.CharacterAnim.SetIKPosition(AvatarIKGoal.RightHand, new Vector3(this.RightHandPosition.x, this.ClimbPosition.y + this.HandIK.RightHand.OffsetPosition.y, this.RightHandPosition.z));
    this.Character.CharacterAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, this.HandIK.RightHand.Weight);
    this.Character.CharacterAnim.SetIKHintPosition(AvatarIKHint.RightElbow, this.HandIK.RightHand.HintTransform.position);
    this.Character.CharacterAnim.SetIKHintPositionWeight(AvatarIKHint.RightElbow, this.HandIK.RightHand.Weight);
    this.Character.CharacterAnim.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(this.transform.forward) * Quaternion.Euler(this.HandIK.RightHand.OffsetNormal));
    this.Character.CharacterAnim.SetIKRotationWeight(AvatarIKGoal.RightHand, this.HandIK.RightHand.Weight);
    this.Character.CharacterAnim.SetIKPosition(AvatarIKGoal.LeftHand, new Vector3(this.LeftHandPosition.x, this.ClimbPosition.y + this.HandIK.LeftHand.OffsetPosition.y, this.LeftHandPosition.z));
    this.Character.CharacterAnim.SetIKPositionWeight(AvatarIKGoal.LeftHand, this.HandIK.LeftHand.Weight);
    this.Character.CharacterAnim.SetIKHintPosition(AvatarIKHint.LeftElbow, this.HandIK.LeftHand.HintTransform.position);
    this.Character.CharacterAnim.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, this.HandIK.LeftHand.Weight);
    this.Character.CharacterAnim.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(this.transform.forward) * Quaternion.Euler(-this.HandIK.LeftHand.OffsetNormal));
    this.Character.CharacterAnim.SetIKRotationWeight(AvatarIKGoal.LeftHand, this.HandIK.LeftHand.Weight);
  }

  private IEnumerator CheckGround()
  {
    yield return new WaitWhile(() => !Character.LocomotionData.IsGrounded);
    IsClimbing = false;
    ClimbPosition = Vector3.zero;
    ClimbOffset.x = 0f;
  }

  public void Anim_OnClimbEnd()
  {
    if (this.C_CheckGround != null)
      this.StopCoroutine(this.C_CheckGround);
    this.C_CheckGround = this.StartCoroutine(this.CheckGround());
  }
}
