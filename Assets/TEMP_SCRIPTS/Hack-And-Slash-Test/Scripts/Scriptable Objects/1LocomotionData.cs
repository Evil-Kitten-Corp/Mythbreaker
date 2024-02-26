// Decompiled with JetBrains decompiler
// Type: LocomotionData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
[CreateAssetMenu(fileName = "Locomotion Data", menuName = "Scriptable Object/Locomotion Data", order = 2147483647)]
public class LocomotionData : ScriptableObject
{
  [Header("[Locomotion Data - State Values]")]
  public eCharacterState CharacterState;
  public eMovementState MovementState = eMovementState.Grounded;
  public eCharacterMoveMode CharacterMoveMode;
  public eAllowedGait AllowedGait = eAllowedGait.Running;
  [Header("[Locomotion Data - Movement System]")]
  public float MaxWalkSpeed;
  public float MaxWalkSpeedCrouched;
  public float LocWalkSpeed;
  public float LocRunSpeed;
  public float LocSprintSpeed;
  [Header("[Locomotion Data - Grounded]")]
  public CurrentMovementSettings CurrentMovementSettings;
  public AnimationCurveData AnimationCurveData;
  public LayerMask GroundLayer;
  public float Gravity;
  public float WalkSpeed = 0.5f;
  public float RunSpeed = 1f;
  public Vector3 Velocity;
  public Vector3 RelativeAccelerationAmount;
  public float MaxAcceleration = 1f;
  public float MaxBrakingDeceleration = 1f;
  public float Gait;
  public bool IsGrounded = true;
  public bool IsSprint;
  public Vector3 LocRelativeVelocityDir;
  public Vector3 RelativeDirection;
  public VelocityBlend VelocityBlend;
  public float VelocityBlendLerpSpeed = 12f;
  public LeanAmount LeanAmount;
  public float LeanLerpSpeed = 4f;
  [Header("[Locomotion Data - Essential Information]")]
  public Vector3 Acceleration;
  public float AccelerationLerpSpeed = 5f;
  public bool IsMoving;
  public bool HasInput;
  public bool HasMovementInput;
  public Quaternion LastVelocityRotation;
  public Quaternion LastMovementInputRotation;
  public float Speed;
  public float LerpSpeed;
  public float InputAmount;
  public float MovementInputAmount;
  public float AimYawRate;
  [Header("[Locomotion Data - Caches Values]")]
  public Vector3 PreviousVelocity;
  public float PreviousAimYaw;
  [Header("[Locomotion Data - Jump]")]
  public float JumpForce;
  public bool IsJump;
  [Header("[Locomotion Data - Air]")]
  public float AirControl = 0.2f;
  public float AirRotationSpeed = 1f;
  public bool IsAir;
  [Header("[Locomotion Data - Rotation Values]")]
  public AnimationCurve YawOffsetFB;
  public AnimationCurve YawOffsetLR;
  public float RotationSpeed;
  public bool LockedRotation;

  protected void OnEnable() => this.OnReset();

  private void OnReset()
  {
    this.CharacterState = eCharacterState.Idle;
    this.CharacterMoveMode = eCharacterMoveMode.Directional;
    this.MovementState = eMovementState.Grounded;
    this.GroundLayer = (LayerMask) (1 << LayerMask.NameToLayer("Map"));
    this.Speed = 0.0f;
    this.Acceleration = Vector3.zero;
    this.RelativeAccelerationAmount = Vector3.zero;
    this.Velocity = Vector3.zero;
    this.PreviousVelocity = Vector3.zero;
    this.Gait = 0.0f;
    this.LeanAmount = new LeanAmount()
    {
      LR = 0.0f,
      FB = 0.0f
    };
    this.IsGrounded = true;
    this.IsSprint = false;
    this.IsJump = false;
    this.IsAir = false;
    this.LockedRotation = false;
  }

  public Vector3 GetVelocity(Character character) => character.CharacterController.velocity;

  public Vector3 GetCurrentAcceleration() => this.Acceleration;

  public float GetMaxAcceleration() => this.MaxAcceleration;

  public float GetMaxBrakingDeceleration() => this.MaxBrakingDeceleration;

  public VelocityBlend CalculateVelocityBlend(Character character)
  {
    this.LocRelativeVelocityDir = this.UnrotateVector(this.Velocity.normalized, character.transform.rotation);
    this.RelativeDirection = this.LocRelativeVelocityDir / (Mathf.Abs(this.LocRelativeVelocityDir.x) + Mathf.Abs(this.LocRelativeVelocityDir.y) + Mathf.Abs(this.LocRelativeVelocityDir.z));
    return new VelocityBlend()
    {
      F = Mathf.Clamp(this.RelativeDirection.z, 0.0f, 1f),
      B = Mathf.Abs(Mathf.Clamp(this.RelativeDirection.z, -1f, 0.0f)),
      L = Mathf.Abs(Mathf.Clamp(this.RelativeDirection.x, -1f, 0.0f)),
      R = Mathf.Clamp(this.RelativeDirection.x, 0.0f, 1f)
    };
  }

  public VelocityBlend InterpVelocityBlend(
    VelocityBlend current,
    VelocityBlend target,
    float interpSpeed,
    float deltaTime)
  {
    current.F = Mathf.Lerp(current.F, target.F, deltaTime * interpSpeed);
    current.B = Mathf.Lerp(current.B, target.B, deltaTime * interpSpeed);
    current.R = Mathf.Lerp(current.R, target.R, deltaTime * interpSpeed);
    current.L = Mathf.Lerp(current.L, target.L, deltaTime * interpSpeed);
    return current;
  }

  public Vector3 CalculateAcceleraction()
  {
    return (this.Velocity - this.PreviousVelocity) / Time.deltaTime;
  }

  public Vector3 CalculateRelativeAccelerationAmount(Character character)
  {
    if ((double) Vector3.Dot(this.Acceleration, this.Velocity) > 0.0)
    {
      Debug.LogError((object) "<color=green>가속</color>");
      return this.UnrotateVector(Vector3.ClampMagnitude(this.Acceleration, this.GetMaxAcceleration()) / this.GetMaxAcceleration(), character.transform.rotation);
    }
    Debug.LogError((object) "<color=red>감속</color>");
    return this.UnrotateVector(Vector3.ClampMagnitude(this.Acceleration, this.GetMaxBrakingDeceleration()) / this.GetMaxBrakingDeceleration(), character.transform.rotation);
  }

  public Vector3 UnrotateVector(Vector3 vector, Quaternion rotation)
  {
    return Quaternion.Inverse(rotation) * vector;
  }

  public Vector3 GetControlRotation()
  {
    if ((Object) Camera.main != (Object) null)
      return Camera.main.transform.eulerAngles;
    Debug.LogError((object) "Camera.main not found");
    return Vector3.zero;
  }
}
