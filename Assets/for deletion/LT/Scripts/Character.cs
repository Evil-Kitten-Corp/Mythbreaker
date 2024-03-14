using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AnimationEvent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CharacterMovement))]
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

	public bool IsPlayer => CharacterTag.Equals("Player");

	private void Awake()
	{
		CharacterAnim = GetComponent<Animator>();
		AnimationEvent = GetComponent<AnimationEvent>();
		CharacterCollider = GetComponent<CapsuleCollider>();
		CharacterController = GetComponent<CharacterController>();
		CharacterMovement = GetComponent<CharacterMovement>();
	}

	private void Update()
	{
		SetGravity();
		CheckGround();
		SetAnimationState(LocomotionData.CharacterMoveMode);
	}

	public virtual void SetAnimationState(eCharacterMoveMode mode)
	{
		Angle = Mathf.Lerp(Angle, Vector3.SignedAngle(base.transform.forward, CharacterMovement.GetDesiredMoveDirection, Vector3.up), Time.deltaTime * 4f);
		CharacterAnim.SetFloat("Angle", Angle);
		Ratio = Mathf.Lerp(Ratio, Mathf.Clamp01((180f - Mathf.Abs(CharacterAnim.GetFloat("Angle"))) / 180f), Time.deltaTime * 4f);
		CharacterAnim.SetFloat("Ratio", Ratio);
		switch (mode)
		{
		case eCharacterMoveMode.Directional:
			CharacterAnim.SetFloat("Speed", CharacterMovement.GetStateSpeed());
			CharacterAnim.SetFloat("Direction", 0f);
			LocomotionData.Gait = Mathf.Lerp(LocomotionData.Gait, LocomotionData.AnimationCurveData.RunCurve.Evaluate(CharacterMovement.GetStateSpeed()), Time.deltaTime);
			break;
		case eCharacterMoveMode.Strafe:
			CharacterAnim.SetFloat("Speed", CharacterMovement.GetStateSpeed());
			CharacterAnim.SetFloat("Direction", MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().x, 0.25f, Time.deltaTime);
			LocomotionData.Gait = Mathf.Lerp(LocomotionData.Gait, LocomotionData.AnimationCurveData.RunCurve.Evaluate(CharacterMovement.GetStateSpeed()), Time.deltaTime);
			break;
		}
		CharacterAnim.SetFloat("Lean FB", LocomotionData.LeanAmount.FB);
		CharacterAnim.SetFloat("Lean LR", LocomotionData.LeanAmount.LR);
	}

	public virtual void CheckGround()
	{
		if (!LocomotionData.IsJump)
		{
			LocomotionData.IsGrounded = Physics.CheckSphere(base.transform.position, CharacterCollider.radius, LocomotionData.GroundLayer.value);
			CharacterAnim.SetBool("IsGrounded", LocomotionData.IsGrounded);
			LocomotionData.MovementState = (LocomotionData.IsGrounded ? eMovementState.Grounded : eMovementState.InAir);
		}
	}

	public virtual void SetGravity()
	{
		switch (LocomotionData.MovementState)
		{
		case eMovementState.Grounded:
			LocomotionData.JumpForce = 0f;
			CharacterController.Move(new Vector3(0f, LocomotionData.Gravity, 0f) * Time.deltaTime);
			break;
		case eMovementState.InAir:
			LocomotionData.JumpForce += LocomotionData.Gravity * Time.deltaTime;
			CharacterController.Move(new Vector3(0f, LocomotionData.JumpForce, 0f) * Time.deltaTime);
			break;
		}
	}

	public virtual void AirControl(float jumpForce)
	{
		CharacterMovement.AirMoveDirection = CharacterMovement.GetDesiredMoveDirection * LocomotionData.AirControl;
		CharacterMovement.AirMoveDirection = CharacterMovement.ClampVector(CharacterMovement.AirMoveDirection, -3f, 3f);
		eCharacterMoveMode characterMoveMode = LocomotionData.CharacterMoveMode;
		if ((uint)characterMoveMode <= 1u)
		{
			CharacterController.Move(new Vector3(CharacterMovement.AirMoveDirection.x, jumpForce, CharacterMovement.AirMoveDirection.z) * Time.deltaTime);
		}
	}

	private IEnumerator CheckFalling()
	{
		float fallingTime2 = 0f;
		while (!LocomotionData.IsGrounded)
		{
			fallingTime2 += Time.deltaTime;
			CharacterAnim.SetFloat("Falling Time", fallingTime2);
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		fallingTime2 = 0f;
		CharacterAnim.SetFloat("Falling Time", fallingTime2);
		CharacterOptional.jumpCount = 0;
	}

	private IEnumerator LookAt(GameObject target, float duration, float lerpSpeed)
	{
		Vector3 direction = Util.GetDirection(target.transform.position, base.transform.position);
		float currentLerpTime = 0f;
		while (currentLerpTime < duration)
		{
			currentLerpTime += Time.deltaTime * lerpSpeed;
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(-direction), currentLerpTime / duration);
			yield return null;
		}
	}

	private IEnumerator Height(float height)
	{
		LocomotionData.IsJump = true;
		LocomotionData.IsGrounded = false;
		CharacterAnim.SetBool("IsGrounded", false);
		LocomotionData.MovementState = eMovementState.InAir;
		LocomotionData.JumpForce = height;
		FallingEvent();
		float inAirTime = 0f;
		while (!LocomotionData.IsGrounded)
		{
			inAirTime += Time.deltaTime;
			if (inAirTime > 0.2f && LocomotionData.IsJump)
			{
				LocomotionData.IsJump = false;
			}
			Vector3 backward = -base.transform.forward * 3f;
			CharacterController.Move(new Vector3(backward.x, height, backward.z) * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator CheckWall(Character hitActor)
	{
		bool isWall = false;
		float elapsedTime = 0f;
		while (!isWall && elapsedTime < 0.5f)
		{
			elapsedTime += Time.deltaTime;
			Vector3 position = hitActor.transform.position;
			Vector3 point2 = hitActor.transform.position + hitActor.transform.TransformDirection(0f, hitActor.CharacterController.height, 0f);
			if (Physics.CapsuleCast(position, point2, hitActor.CharacterController.radius, -hitActor.transform.forward * 1.5f, out var hitInfo, hitActor.CharacterController.radius, 1 << LayerMask.NameToLayer("Default")))
			{
				hitActor.CharacterAnim.CrossFadeInFixedTime("Wall Hit Down", 0.1f);
				hitActor.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
				isWall = true;
			}
			yield return null;
		}
	}

	public void FallingEvent()
	{
		if (C_CheckFalling != null)
		{
			StopCoroutine(C_CheckFalling);
		}
		C_CheckFalling = StartCoroutine(CheckFalling());
	}

	public void SetCharacterTag(string tag)
	{
		CharacterTag = tag;
	}

	public string GetCharacterTag()
	{
		return CharacterTag;
	}

	public void SetIgnoreCollider(Collider collider, bool ignore)
	{
		Physics.IgnoreCollision(CharacterController, collider, ignore);
		Physics.IgnoreCollision(CharacterCollider, collider, ignore);
	}

	public void SetLookAt(GameObject target, float duration, float lerpSpeed)
	{
		if (C_LookAt != null)
		{
			StopCoroutine(C_LookAt);
		}
		C_LookAt = StartCoroutine(LookAt(target, duration, lerpSpeed));
	}

	public void TakeDamage(Character causer, float damageAmount, UnityAction damageEvent = null)
	{
		switch (LocomotionData.MovementState)
		{
		case eMovementState.Grounded:
			switch (causer.CombatData.AttackType)
			{
			case EAttackType.LightAttack:
				CharacterAnim.CrossFadeInFixedTime($"Hit_{causer.CombatData.AttackDireciton}", 0.1f);
				break;
			case EAttackType.StrongAttack:
				CharacterAnim.CrossFadeInFixedTime($"Strong Hit_{causer.CombatData.AttackDireciton}", 0.1f);
				if (causer.CombatData.AttackDireciton == EAttackDirection.Down)
				{
					SetHeight(5f);
				}
				break;
			}
			WallHit(this);
			break;
		case eMovementState.InAir:
			CharacterAnim.CrossFadeInFixedTime("Air Hit", 0.1f);
			SetHeight(2f);
			break;
		}
		damageEvent?.Invoke();
	}

	public void SetHeight(float height)
	{
		if (C_Height != null)
		{
			StopCoroutine(C_Height);
		}
		C_Height = StartCoroutine(Height(height));
	}

	private void WallHit(Character hitActor)
	{
		if (C_CheckWall != null)
		{
			StopCoroutine(C_CheckWall);
		}
		C_CheckWall = StartCoroutine(CheckWall(hitActor));
	}
}
