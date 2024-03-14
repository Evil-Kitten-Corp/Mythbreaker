using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
	[Header("[ReadOnly Variable]")]
	public readonly float idleSpeed;

	public readonly float walkSpeed = 0.5f;

	public readonly float sprintSpeed = 1f;

	[Header("[Component]")]
	[HideInInspector]
	public Animator characterAnim;

	[HideInInspector]
	public Rigidbody characterRig;

	[HideInInspector]
	public Collider characterCollider;

	[Header("[Character Base]")]
	public CharacterState characterState;

	public CharacterOptional characterOptional;

	public AnimationCurves animationCurves;

	private void Start()
	{
		OnStart();
	}

	private void Update()
	{
		OnUpdate();
	}

	private void FixedUpdate()
	{
		OnFixedUpdate();
	}

	protected virtual void OnStart()
	{
		OnInitialize();
	}

	protected virtual void OnUpdate()
	{
	}

	protected virtual void OnFixedUpdate()
	{
	}

	protected virtual void OnInitialize()
	{
		characterAnim = GetComponent<Animator>();
		characterRig = GetComponent<Rigidbody>();
		characterCollider = GetComponent<Collider>();
	}

	protected abstract void SetGravity();

	protected abstract void CheckGround();

	protected abstract void AirControl();
}
