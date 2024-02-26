using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character
{
  [RequireComponent(typeof (AnimationEvent))]
  public class Character : MonoBehaviour
  {
    [Header("[Character - Component]")]
    public Animator CharacterAnim;
    public AnimationEvent AnimationEvent;
    public CapsuleCollider CharacterCollider;
    public CharacterController CharacterController;
    public CharacterMovement CharacterMovement;
    [Header("[Character - Data]")]
    public LocomotionData LocomotionData;
    [SerializeField]
    private string CharacterTag;
    public float Angle;
    public float Ratio = 1f;
    [Header("[Character - Combat]")]
    public CombatData CombatData;
    public List<AttachSocket> attachSockets;
    [Header("[Character - Weapon]")]
    public List<AttachWeapon> CurrentWeapon;
    [Header("[Character Option]")]
    public CharacterOptional CharacterOptional;
    [Header("[Character - Coroutine]")]
    private Coroutine C_CheckFalling;
    private Coroutine C_LookAt;
    private Coroutine C_Height;
    private Coroutine C_CheckWall;

    public bool IsPlayer => this.CharacterTag.Equals("Player");

    private void Awake()
    {
      this.CharacterAnim = this.GetComponent<Animator>();
      this.AnimationEvent = this.GetComponent<AnimationEvent>();
      this.CharacterCollider = this.GetComponent<CapsuleCollider>();
      this.CharacterController = this.GetComponent<CharacterController>();
      this.CharacterMovement = this.GetComponent<CharacterMovement>();
    }

    private void Update()
    {
      this.SetGravity();
      this.CheckGround();
      this.SetAnimationState(this.LocomotionData.CharacterMoveMode);
    }

    public virtual void SetAnimationState(eCharacterMoveMode mode)
    {
      this.Angle = Mathf.Lerp(this.Angle, Vector3.SignedAngle(this.transform.forward, this.CharacterMovement.GetDesiredMoveDirection, Vector3.up), Time.deltaTime * 4f);
      this.CharacterAnim.SetFloat("Angle", this.Angle);
      this.Ratio = Mathf.Lerp(this.Ratio, Mathf.Clamp01((float) ((180.0 - (double) Mathf.Abs(this.CharacterAnim.GetFloat("Angle"))) / 180.0)), Time.deltaTime * 4f);
      this.CharacterAnim.SetFloat("Ratio", this.Ratio);
      switch (mode)
      {
        case eCharacterMoveMode.Directional:
          this.CharacterAnim.SetFloat("Speed", this.CharacterMovement.GetStateSpeed());
          this.CharacterAnim.SetFloat("Direction", 0.0f);
          this.LocomotionData.Gait = Mathf.Lerp(this.LocomotionData.Gait, this.LocomotionData.AnimationCurveData.RunCurve.Evaluate(this.CharacterMovement.GetStateSpeed()), Time.deltaTime);
          break;
        case eCharacterMoveMode.Strafe:
          this.CharacterAnim.SetFloat("Speed", this.CharacterMovement.GetStateSpeed());
          this.CharacterAnim.SetFloat("Direction", MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().x, 0.25f, Time.deltaTime);
          this.LocomotionData.Gait = Mathf.Lerp(this.LocomotionData.Gait, this.LocomotionData.AnimationCurveData.RunCurve.Evaluate(this.CharacterMovement.GetStateSpeed()), Time.deltaTime);
          break;
      }
      this.CharacterAnim.SetFloat("Lean FB", this.LocomotionData.LeanAmount.FB);
      this.CharacterAnim.SetFloat("Lean LR", this.LocomotionData.LeanAmount.LR);
    }

    public virtual void CheckGround()
    {
      if (this.LocomotionData.IsJump)
        return;
      this.LocomotionData.IsGrounded = Physics.CheckSphere(this.transform.position, this.CharacterCollider.radius, this.LocomotionData.GroundLayer.value);
      this.CharacterAnim.SetBool("IsGrounded", this.LocomotionData.IsGrounded);
      this.LocomotionData.MovementState = this.LocomotionData.IsGrounded ? eMovementState.Grounded : eMovementState.InAir;
    }

    public virtual void SetGravity()
    {
      switch (this.LocomotionData.MovementState)
      {
        case eMovementState.Grounded:
          this.LocomotionData.JumpForce = 0.0f;
          int num1 = (int) this.CharacterController.Move(new Vector3(0.0f, this.LocomotionData.Gravity, 0.0f) * Time.deltaTime);
          break;
        case eMovementState.InAir:
          this.LocomotionData.JumpForce += this.LocomotionData.Gravity * Time.deltaTime;
          int num2 = (int) this.CharacterController.Move(new Vector3(0.0f, this.LocomotionData.JumpForce, 0.0f) * Time.deltaTime);
          break;
      }
    }

    public virtual void AirControl(float jumpForce)
    {
      this.CharacterMovement.AirMoveDirection = this.CharacterMovement.GetDesiredMoveDirection * this.LocomotionData.AirControl;
      this.CharacterMovement.AirMoveDirection = this.CharacterMovement.ClampVector(this.CharacterMovement.AirMoveDirection, -3f, 3f);
      switch (this.LocomotionData.CharacterMoveMode)
      {
        case eCharacterMoveMode.Directional:
        case eCharacterMoveMode.Strafe:
          int num = (int) this.CharacterController.Move(new Vector3(this.CharacterMovement.AirMoveDirection.x, jumpForce, this.CharacterMovement.AirMoveDirection.z) * Time.deltaTime);
          break;
      }
    }

    private IEnumerator CheckFalling()
    {
      float fallingTime = 0.0f;
      while (!this.LocomotionData.IsGrounded)
      {
        fallingTime += Time.deltaTime;
        this.CharacterAnim.SetFloat("Falling Time", fallingTime);
        yield return (object) null;
      }
      yield return (object) new WaitForEndOfFrame();
      fallingTime = 0.0f;
      this.CharacterAnim.SetFloat("Falling Time", fallingTime);
      this.CharacterOptional.jumpCount = 0;
    }

    private IEnumerator LookAt(GameObject target, float duration, float lerpSpeed)
    {
      Character character = this;
      Vector3 direction = Util.GetDirection(target.transform.position, character.transform.position);
      float currentLerpTime = 0.0f;
      float lerpTime = duration;
      while ((double) currentLerpTime < (double) lerpTime)
      {
        currentLerpTime += Time.deltaTime * lerpSpeed;
        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(-direction), currentLerpTime / lerpTime);
        yield return (object) null;
      }
    }

    private IEnumerator Height(float height)
    {
      Character character = this;
      character.LocomotionData.IsJump = true;
      character.LocomotionData.IsGrounded = false;
      character.CharacterAnim.SetBool("IsGrounded", false);
      character.LocomotionData.MovementState = eMovementState.InAir;
      character.LocomotionData.JumpForce = height;
      character.FallingEvent();
      float inAirTime = 0.0f;
      while (!character.LocomotionData.IsGrounded)
      {
        inAirTime += Time.deltaTime;
        if ((double) inAirTime > 0.20000000298023224 && character.LocomotionData.IsJump)
          character.LocomotionData.IsJump = false;
        Vector3 vector3 = -character.transform.forward * 3f;
        int num = (int) character.CharacterController.Move(new Vector3(vector3.x, height, vector3.z) * Time.deltaTime);
        yield return (object) null;
      }
    }

    private IEnumerator CheckWall(Character hitActor)
    {
      bool isWall = false;
      float elapsedTime = 0.0f;
      while (!isWall && (double) elapsedTime < 0.5)
      {
        elapsedTime += Time.deltaTime;
        RaycastHit hitInfo;
        if (Physics.CapsuleCast(hitActor.transform.position, hitActor.transform.position + hitActor.transform.TransformDirection(0.0f, hitActor.CharacterController.height, 0.0f), hitActor.CharacterController.radius, -hitActor.transform.forward * 1.5f, out hitInfo, hitActor.CharacterController.radius, 1 << LayerMask.NameToLayer("Default")))
        {
          hitActor.CharacterAnim.CrossFadeInFixedTime("Wall Hit Down", 0.1f);
          hitActor.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
          isWall = true;
        }
        yield return (object) null;
      }
    }

    public void FallingEvent()
    {
      if (this.C_CheckFalling != null)
        this.StopCoroutine(this.C_CheckFalling);
      this.C_CheckFalling = this.StartCoroutine(this.CheckFalling());
    }

    public void SetCharacterTag(string tag) => this.CharacterTag = tag;

    public string GetCharacterTag() => this.CharacterTag;

    public void SetIgnoreCollider(Collider collider, bool ignore)
    {
      Physics.IgnoreCollision((Collider) this.CharacterController, collider, ignore);
      Physics.IgnoreCollision((Collider) this.CharacterCollider, collider, ignore);
    }

    public void SetLookAt(GameObject target, float duration, float lerpSpeed)
    {
      if (this.C_LookAt != null)
        this.StopCoroutine(this.C_LookAt);
      this.C_LookAt = this.StartCoroutine(this.LookAt(target, duration, lerpSpeed));
    }

    public void TakeDamage(Character causer, float damageAmount, UnityAction damageEvent = null)
    {
      switch (this.LocomotionData.MovementState)
      {
        case eMovementState.Grounded:
          switch (causer.CombatData.AttackType)
          {
            case EAttackType.LightAttack:
              this.CharacterAnim.CrossFadeInFixedTime(string.Format("Hit_{0}", (object) causer.CombatData.AttackDireciton), 0.1f);
              break;
            case EAttackType.StrongAttack:
              this.CharacterAnim.CrossFadeInFixedTime(string.Format("Strong Hit_{0}", (object) causer.CombatData.AttackDireciton), 0.1f);
              if (causer.CombatData.AttackDireciton == EAttackDirection.Down)
              {
                this.SetHeight(5f);
                break;
              }
              break;
          }
          this.WallHit(this);
          break;
        case eMovementState.InAir:
          this.CharacterAnim.CrossFadeInFixedTime("Air Hit", 0.1f);
          this.SetHeight(2f);
          break;
      }
      if (damageEvent == null)
        return;
      damageEvent();
    }

    public void SetHeight(float height)
    {
      if (this.C_Height != null)
        this.StopCoroutine(this.C_Height);
      this.C_Height = this.StartCoroutine(this.Height(height));
    }

    private void WallHit(Character hitActor)
    {
      if (this.C_CheckWall != null)
        this.StopCoroutine(this.C_CheckWall);
      this.C_CheckWall = this.StartCoroutine(this.CheckWall(hitActor));
    }
  }
}