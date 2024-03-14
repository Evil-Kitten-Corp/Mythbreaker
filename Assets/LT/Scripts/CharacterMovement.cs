using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	[Header("[Character Movement]")]
	public Character Character;

	[SerializeField]
	private Vector3 InputVelocity;

	[SerializeField]
	private Vector3 InputDirection;

	[SerializeField]
	private Vector3 DesiredMoveDirection;

	[SerializeField]
	private Vector3 StrafeMoveDirection;

	public Vector3 AirMoveDirection;

	[Header("[Character Movement - Turn]")]
	public bool IsCheckTurn;

	public float Angle;

	public float TurnAngle = 150f;

	private float TurnDelayTime;

	[Header("[Character Movement - Coroutine]")]
	private Coroutine MoveTurnCoroutine;

	[Header("[Draw Gizmos]")]
	public bool IsDrawGizmos;

	public Vector3 GetInputDirection => InputDirection;

	public Vector3 GetDesiredMoveDirection => DesiredMoveDirection;

	public Vector3 GetStrafeMoveDirection => StrafeMoveDirection;

	public bool IsTurn => Vector3.Angle(base.transform.forward, GetDesiredMoveDirection) >= TurnAngle;

	private void Update()
	{
		SetEssentialValues();
		CacheValues();
		UpdateCharacterInfo();
	}

	private void OnDrawGizmos()
	{
		if (IsDrawGizmos)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(base.transform.position + base.transform.TransformDirection(0f, 1f, 0f), InputDirection.normalized * 5f * Character.CharacterAnim.GetFloat("Speed"));
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(base.transform.position + base.transform.TransformDirection(0f, 1f, 0f), DesiredMoveDirection.normalized * 5f * Character.CharacterAnim.GetFloat("Speed"));
		}
	}

	private void SetEssentialValues()
	{
		if (Character.LocomotionData.IsGrounded)
		{
			Character.LocomotionData.Acceleration = Vector3.Lerp(Character.LocomotionData.Acceleration, 
				Character.LocomotionData.CalculateAcceleraction(), 
				Time.deltaTime * Character.LocomotionData.AccelerationLerpSpeed);
			
			Character.LocomotionData.Speed = new Vector3(Character.LocomotionData.Velocity.x,
				0f, Character.LocomotionData.Velocity.z).sqrMagnitude;
			
			Character.LocomotionData.LerpSpeed = Mathf.Lerp(Character.LocomotionData.LerpSpeed, 
				Character.LocomotionData.Speed, Time.deltaTime);
			
			Character.LocomotionData.IsMoving = Character.LocomotionData.Speed > 0.1f;
			
			Character.CharacterAnim.SetBool("IsMove", Character.LocomotionData.IsMoving);
			
			if (Character.LocomotionData.IsMoving)
			{
				Character.LocomotionData.LastVelocityRotation = Quaternion.LookRotation(
					new Vector3(0f, 0f, Character.LocomotionData.Velocity.z), Vector3.up);
			}
			
			Character.LocomotionData.InputAmount = InputVelocity.sqrMagnitude;
			
			Character.LocomotionData.HasInput = Character.LocomotionData.InputAmount > 0f;
			
			Character.LocomotionData.MovementInputAmount = Character.LocomotionData.GetCurrentAcceleration()
				.sqrMagnitude / Character.LocomotionData.GetMaxAcceleration();
			
			Character.LocomotionData.HasMovementInput = Character.LocomotionData.MovementInputAmount > 0f;
			
			if (Character.LocomotionData.HasMovementInput)
			{
				Character.LocomotionData.LastMovementInputRotation = Quaternion.LookRotation(new 
					Vector3(0f, 0f, Character.LocomotionData.GetCurrentAcceleration().z), Vector3.up);
			}
			
			Character.LocomotionData.AimYawRate = Mathf.Abs((
				Character.LocomotionData.GetControlRotation().z - Character.LocomotionData.PreviousAimYaw) / Time.deltaTime);
		}
	}

	private void CacheValues()
	{
		Character.LocomotionData.PreviousVelocity = Character.LocomotionData.Velocity;
	}

	private void SetMovementState()
	{
		switch (Character.LocomotionData.MovementState)
		{
		case eMovementState.Grounded:
			UpdateMovementValues();
			UpdateRotationValues();
			MoveTurn();
			break;
		case eMovementState.InAir:
			UpdateMovementValues();
			UpdateRotationValues();
			break;
		}
	}

	private void UpdateCharacterInfo()
	{
		Vector2 inputValue = MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>();
		Vector3 move = new Vector3(inputValue.x, 0f, inputValue.y);
		InputVelocity = move;
		SetMovementState();
	}

	private void UpdateMovementValues()
	{
		Character.LocomotionData.VelocityBlend = Character.LocomotionData.InterpVelocityBlend(
			Character.LocomotionData.VelocityBlend, Character.LocomotionData.CalculateVelocityBlend(Character),
			Character.LocomotionData.VelocityBlendLerpSpeed, Time.deltaTime);
		Vector3 forward = Camera.main.transform.forward;
		Vector3 right = Camera.main.transform.right;
		forward.y = 0f;
		right.y = 0f;
		forward.Normalize();
		right.Normalize();
		InputDirection = forward * InputVelocity.z + right * InputVelocity.x;
		InputDirection.Normalize();
		DesiredMoveDirection = Vector3.Lerp(DesiredMoveDirection, forward * InputVelocity.z + right * 
			InputVelocity.x, Time.deltaTime * Character.LocomotionData.RotationSpeed);
		DesiredMoveDirection.Normalize();
		StrafeMoveDirection = Vector3.Lerp(StrafeMoveDirection, new Vector3(DesiredMoveDirection.x * 
			base.transform.right.x + DesiredMoveDirection.z * base.transform.right.z, 0f, DesiredMoveDirection.x * 
			base.transform.forward.x + DesiredMoveDirection.z * base.transform.forward.z), Time.deltaTime * 
			Character.LocomotionData.RotationSpeed);
		if (Character.LocomotionData.LockedRotation)
		{
			return;
		}
		Character.CharacterController.Move(DesiredMoveDirection * (((!Character.CharacterAnim.applyRootMotion) ? 
			GetStateSpeed() : 1f) * Character.Ratio * Time.deltaTime));
		if (Character.LocomotionData.IsGrounded)
		{
			if (!Character.LocomotionData.HasInput)
			{
				if (Character.LocomotionData.CharacterMoveMode == eCharacterMoveMode.Directional && !IsTurn)
				{
					eCharacterState characterState = Character.LocomotionData.CharacterState;
					if (characterState != eCharacterState.Walk)
					{
						_ = 2;
					}
				}
				Character.LocomotionData.CharacterState = eCharacterState.Idle;
				Character.LocomotionData.IsSprint = false;
				Character.CharacterAnim.SetBool("IsSprint", false);
			}
			else if (!Character.LocomotionData.IsSprint)
			{
				Character.LocomotionData.CharacterState = eCharacterState.Walk;
			}
			else
			{
				Character.LocomotionData.CharacterState = eCharacterState.Sprint;
			}
		}
		Character.LocomotionData.Velocity = Character.LocomotionData.GetVelocity(Character);
		Character.LocomotionData.RelativeAccelerationAmount = Character.LocomotionData.CalculateRelativeAccelerationAmount(Character);
		Character.LocomotionData.LeanAmount.LR = Mathf.Lerp(Character.LocomotionData.LeanAmount.LR, 
			Mathf.Clamp(Character.LocomotionData.RelativeAccelerationAmount.x * 
			            Character.LocomotionData.Gait, -1f, 1f), Time.deltaTime * Character.LocomotionData.LeanLerpSpeed);
		Character.LocomotionData.LeanAmount.FB = Mathf.Lerp(Character.LocomotionData.LeanAmount.FB, 
			Mathf.Clamp(Character.LocomotionData.RelativeAccelerationAmount.z * 
			            Character.LocomotionData.Gait, -1f, 1f), Time.deltaTime * Character.LocomotionData.LeanLerpSpeed);
	}

	private void UpdateRotationValues()
	{
		if (Character.LocomotionData.LockedRotation)
		{
			return;
		}
		switch (Character.LocomotionData.CharacterMoveMode)
		{
		case eCharacterMoveMode.Directional:
			if (Character.LocomotionData.IsGrounded)
			{
				if (Character.LocomotionData.IsMoving)
				{
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, 
						Quaternion.LookRotation(DesiredMoveDirection), Time.deltaTime * Character.LocomotionData.RotationSpeed);
				}
			}
			else if (Character.LocomotionData.IsMoving)
			{
				base.transform.rotation = Quaternion.Slerp(base.transform.rotation, 
					Quaternion.LookRotation(DesiredMoveDirection), Time.deltaTime * Character.LocomotionData.AirRotationSpeed);
			}
			break;
		case eCharacterMoveMode.Strafe:
			if (Character.LocomotionData.IsGrounded)
			{
				StrafeTurn_Update();
			}
			break;
		}
	}

	private IEnumerator CheckTurn()
	{
		IsCheckTurn = true;
		yield return (object)new WaitWhile((Func<bool>)(() => Character.LocomotionData.InputAmount < 0.5f));
		float elapsedTime = 0.25f;
		while (elapsedTime > 0f && Character.LocomotionData.IsGrounded && !Character.LocomotionData.LockedRotation)
		{
			elapsedTime -= Time.deltaTime;
			if (IsTurn && TurnDelayTime <= Time.time)
			{
				TurnDelayTime = Time.time + 0.15f;
				Character.CharacterAnim.CrossFadeInFixedTime("Turn_Blend", 0.25f);
				Character.CharacterAnim.SetFloat("Direction", Vector3.SignedAngle(
					Character.transform.forward, GetDesiredMoveDirection, Vector3.up));
				Character.LocomotionData.IsSprint = true;
				Character.CharacterAnim.SetBool("IsSprint", true);
				elapsedTime = 0f;
			}
			yield return new WaitForFixedUpdate();
		}
		yield return (object)new WaitWhile((Func<bool>)(() => Character.LocomotionData.InputAmount >= 0.5f));
		IsCheckTurn = false;
	}

	private void MoveTurn()
	{
		if (!IsCheckTurn && Character.LocomotionData.IsGrounded && Character.CharacterAnim.GetFloat("Speed") > 
		    0.5f && Character.LocomotionData.CharacterMoveMode == eCharacterMoveMode.Directional)
		{
			MoveTurnCoroutine = StartCoroutine(CheckTurn());
		}
	}

	private void StrafeTurn()
	{
		float angle = Vector3.SignedAngle(base.transform.forward, Camera.main.transform.forward, Vector3.up);
		Character.CharacterAnim.SetFloat("Angle", angle);
		if (angle >= 45f)
		{
			if (TurnDelayTime <= Time.time)
			{
				TurnDelayTime = Time.time + 0.25f;
				Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_L", 0.1f);
				Vector3 forward2 = Camera.main.transform.forward;
				forward2.y = 0f;
				Character.transform.DORotateQuaternion(Quaternion.LookRotation(forward2), 0.4f).SetEase(Ease.Linear);
			}
		}
		else if (angle <= -45f && TurnDelayTime <= Time.time)
		{
			TurnDelayTime = Time.time + 0.25f;
			Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_R", 0.1f);
			Vector3 forward = Camera.main.transform.forward;
			forward.y = 0f;
			Character.transform.DORotateQuaternion(Quaternion.LookRotation(forward), 0.4f).SetEase(Ease.Linear);
		}
	}

	private void StrafeTurn_Update()
	{
		if (Character.LocomotionData.IsMoving)
		{
			return;
		}
		float angle = Vector3.SignedAngle(base.transform.forward, Camera.main.transform.forward, Vector3.up);
		Character.CharacterAnim.SetFloat("Angle", angle);
		if (angle >= 45f)
		{
			if (TurnDelayTime <= Time.time)
			{
				TurnDelayTime = Time.time + 0.35f;
				Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_R", 0.2f);
				Vector3 forward2 = Camera.main.transform.forward;
				forward2.y = 0f;
				forward2.Normalize();
				Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation, 
					Quaternion.LookRotation(forward2), Time.deltaTime * Character.LocomotionData.RotationSpeed);
			}
		}
		else if (angle <= -45f && TurnDelayTime <= Time.time)
		{
			TurnDelayTime = Time.time + 0.35f;
			Character.CharacterAnim.CrossFadeInFixedTime("Strafe Turn_L", 0.2f);
			Vector3 forward = Camera.main.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation, 
				Quaternion.LookRotation(forward), Time.deltaTime * Character.LocomotionData.RotationSpeed);
		}
	}

	public Vector3 ClampVector(Vector3 vector, float minValue, float maxValue)
	{
		vector.x = Mathf.Clamp(vector.x, minValue, maxValue);
		vector.y = Mathf.Clamp(vector.y, minValue, maxValue);
		vector.z = Mathf.Clamp(vector.z, minValue, maxValue);
		return vector;
	}

	public float GetStateSpeed()
	{
		switch (Character.LocomotionData.CharacterState)
		{
		case eCharacterState.Idle:
			Character.LocomotionData.CurrentMovementSettings.CurrentSpeed = Mathf.Lerp(
				Character.LocomotionData.CurrentMovementSettings.CurrentSpeed, 0f, Time.deltaTime);
			break;
		case eCharacterState.Walk:
			Character.LocomotionData.CurrentMovementSettings.CurrentSpeed = Mathf.Lerp(
				Character.LocomotionData.CurrentMovementSettings.CurrentSpeed, Character.LocomotionData.AnimationCurveData.WalkCurve.Evaluate(Character.LocomotionData.CurrentMovementSettings.WalkSpeed), Time.deltaTime);
			break;
		case eCharacterState.Run:
			Character.LocomotionData.CurrentMovementSettings.CurrentSpeed = Mathf.Lerp(
				Character.LocomotionData.CurrentMovementSettings.CurrentSpeed, 
				Character.LocomotionData.AnimationCurveData.RunCurve.Evaluate(
					Character.LocomotionData.CurrentMovementSettings.RunSpeed), Time.deltaTime);
			break;
		case eCharacterState.Sprint:
			Character.LocomotionData.CurrentMovementSettings.CurrentSpeed = Mathf.Lerp(
				Character.LocomotionData.CurrentMovementSettings.CurrentSpeed, 
				Character.LocomotionData.AnimationCurveData.RunCurve.Evaluate(
					Character.LocomotionData.CurrentMovementSettings.SprintSpeed), Time.deltaTime);
			break;
		}
		return Character.LocomotionData.CurrentMovementSettings.CurrentSpeed * Character.Ratio;
	}
}
