using UnityEngine;

[CreateAssetMenu(fileName = "Locomotion Data", menuName = "Scriptable Object/Locomotion Data", order = int.MaxValue)]
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

	protected void OnEnable()
	{
		OnReset();
	}

	private void OnReset()
	{
		CharacterState = eCharacterState.Idle;
		CharacterMoveMode = eCharacterMoveMode.Directional;
		MovementState = eMovementState.Grounded;
		GroundLayer = 1 << LayerMask.NameToLayer("Map");
		Speed = 0f;
		Acceleration = Vector3.zero;
		RelativeAccelerationAmount = Vector3.zero;
		Velocity = Vector3.zero;
		PreviousVelocity = Vector3.zero;
		Gait = 0f;
		LeanAmount = new LeanAmount
		{
			LR = 0f,
			FB = 0f
		};
		IsGrounded = true;
		IsSprint = false;
		IsJump = false;
		IsAir = false;
		LockedRotation = false;
	}

	public Vector3 GetVelocity(Character character)
	{
		return character.CharacterController.velocity;
	}

	public Vector3 GetCurrentAcceleration()
	{
		return Acceleration;
	}

	public float GetMaxAcceleration()
	{
		return MaxAcceleration;
	}

	public float GetMaxBrakingDeceleration()
	{
		return MaxBrakingDeceleration;
	}

	public VelocityBlend CalculateVelocityBlend(Character character)
	{
		LocRelativeVelocityDir = UnrotateVector(Velocity.normalized, character.transform.rotation);
		float sum = Mathf.Abs(LocRelativeVelocityDir.x) + Mathf.Abs(LocRelativeVelocityDir.y) + Mathf.Abs(LocRelativeVelocityDir.z);
		RelativeDirection = LocRelativeVelocityDir / sum;
		VelocityBlend result = default(VelocityBlend);
		result.F = Mathf.Clamp(RelativeDirection.z, 0f, 1f);
		result.B = Mathf.Abs(Mathf.Clamp(RelativeDirection.z, -1f, 0f));
		result.L = Mathf.Abs(Mathf.Clamp(RelativeDirection.x, -1f, 0f));
		result.R = Mathf.Clamp(RelativeDirection.x, 0f, 1f);
		return result;
	}

	public VelocityBlend InterpVelocityBlend(VelocityBlend current, VelocityBlend target, float interpSpeed, float deltaTime)
	{
		current.F = Mathf.Lerp(current.F, target.F, deltaTime * interpSpeed);
		current.B = Mathf.Lerp(current.B, target.B, deltaTime * interpSpeed);
		current.R = Mathf.Lerp(current.R, target.R, deltaTime * interpSpeed);
		current.L = Mathf.Lerp(current.L, target.L, deltaTime * interpSpeed);
		return current;
	}

	public Vector3 CalculateAcceleraction()
	{
		return (Velocity - PreviousVelocity) / Time.deltaTime;
	}

	public Vector3 CalculateRelativeAccelerationAmount(Character character)
	{
		if (Vector3.Dot(Acceleration, Velocity) > 0f)
		{
			Debug.LogError("<color=green>가속</color>");
			return UnrotateVector(Vector3.ClampMagnitude(Acceleration, GetMaxAcceleration()) / GetMaxAcceleration(), character.transform.rotation);
		}
		Debug.LogError("<color=red>감속</color>");
		return UnrotateVector(Vector3.ClampMagnitude(Acceleration, GetMaxBrakingDeceleration()) / GetMaxBrakingDeceleration(), character.transform.rotation);
	}

	public Vector3 UnrotateVector(Vector3 vector, Quaternion rotation)
	{
		return Quaternion.Inverse(rotation) * vector;
	}

	public Vector3 GetControlRotation()
	{
		if (Camera.main != null)
		{
			return Camera.main.transform.eulerAngles;
		}
		Debug.LogError("Camera.main not found");
		return Vector3.zero;
	}
}
