using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemManager : MonoSingleton<InputSystemManager>
{
	[Header("[InputSystemManager - Input System]")]
	public Character Character;

	public CombatComponent CombatComponent;

	private ComboActions InputAction;

	[Header("[InputSystemManager - Optional]")]
	public ECursorMode CursorMode = ECursorMode.Invisible;

	public bool IsToggle;

	[Header("[InputSystemManager - Input Time]")]
	public float JumpDelayTime;

	private Coroutine C_Jump;

	public ComboActions GetInputAction => InputAction;

	protected override void OnAwake()
	{
		base.OnAwake();
		Initialize();
	}

	private void Update()
	{
		Sprint();
	}

	private void OnEnable()
	{
		InputAction.Locomotion.Enable();
		InputAction.Combat.Enable();
	}

	private void OnDisable()
	{
		InputAction.Locomotion.Disable();
		InputAction.Combat.Disable();
	}

	public void Initialize()
	{
		InputAction = new ComboActions();
		InputBinding();
		SetCursorMode(CursorMode);
	}

	private void InputBinding()
	{
		InputAction.Locomotion.Sprint.performed += Sprint;
		InputAction.Locomotion.Jump.performed += Jump;
		InputAction.Locomotion.SlowMotion.performed += SlowMotion;
		InputAction.Combat.Dodge.performed += Dodge;
		InputAction.Combat.LightAttack.performed += LightAttack;
		InputAction.Combat.StrongAttack.performed += StrongAttack;
		InputAction.Combat.HoldAttack.performed += HoldAttack;
		InputAction.Locomotion.SlowMotion.canceled += SlowMotion;
		InputAction.Combat.HoldAttack.canceled += HoldAttack;
	}

	public void SetCursorMode(ECursorMode mode)
	{
		switch (mode)
		{
		case ECursorMode.Visible:
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			break;
		case ECursorMode.Invisible:
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			break;
		}
		CursorMode = mode;
	}

	public void Sprint()
	{
		if (!IsToggle && Character.LocomotionData.IsGrounded)
		{
			Character.LocomotionData.IsSprint = InputAction.Locomotion.Sprint.phase == InputActionPhase.Performed;
			Character.CharacterAnim.SetBool("IsSprint", Character.LocomotionData.IsSprint);
		}
	}

	private IEnumerator Jump()
	{
		Character.LocomotionData.IsJump = true;
		Character.LocomotionData.IsGrounded = false;
		Character.CharacterAnim.SetBool("IsGrounded", false);
		Character.LocomotionData.MovementState = eMovementState.InAir;
		Character.FallingEvent();
		Character.LocomotionData.JumpForce = 5f;
		if (Character.CharacterOptional.jumpCount == 0)
		{
			Character.CharacterAnim.CrossFadeInFixedTime("Jump", 0.1f);
		}
		else if (Character.CharacterOptional.jumpCount == 1)
		{
			Character.CharacterAnim.CrossFadeInFixedTime("Double Jump", 0.25f);
		}
		Character.CharacterOptional.jumpCount++;
		float jumpTime = 0f;
		while (!Character.LocomotionData.IsGrounded)
		{
			jumpTime += Time.deltaTime;
			if (jumpTime > 0.2f)
			{
				Character.LocomotionData.IsJump = false;
			}
			Character.AirControl(Character.LocomotionData.JumpForce);
			yield return null;
		}
	}

	public void Sprint(InputAction.CallbackContext ctx)
	{
		if (IsToggle && Character.LocomotionData.IsGrounded && ctx.performed)
		{
			Character.LocomotionData.IsSprint = !Character.LocomotionData.IsSprint;
			Character.CharacterAnim.SetBool("IsSprint", Character.LocomotionData.IsSprint);
		}
	}

	public void Jump(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && Character.CharacterOptional.jumpCount < 2 && JumpDelayTime <= Time.time)
		{
			JumpDelayTime = Time.time + 0.1f;
			if (C_Jump != null)
			{
				StopCoroutine(C_Jump);
			}
			C_Jump = StartCoroutine(Jump());
			CombatComponent.ResetCombo();
		}
	}

	public void Dodge(InputAction.CallbackContext ctx)
	{
		if (Character.LocomotionData.IsGrounded && Character.CombatData.CombatType != ECombatType.Dodge && ctx.performed)
		{
			Character.CharacterAnim.SetFloat("Dodge_Direction", Vector3.SignedAngle(Character.transform.forward, Character.GetComponent<CharacterMovement>().GetDesiredMoveDirection, Vector3.up));
			switch (Character.LocomotionData.CharacterMoveMode)
			{
			case eCharacterMoveMode.Directional:
				Character.CharacterAnim.CrossFadeInFixedTime("Dodge_F", 0.1f);
				break;
			case eCharacterMoveMode.Strafe:
				Character.CharacterAnim.CrossFadeInFixedTime("Dodge_BT", 0.1f);
				break;
			}
			CombatComponent.ResetCombo();
		}
	}

	private void LightAttack(InputAction.CallbackContext ctx)
	{
		if (Character.LocomotionData.IsGrounded && Character.CombatData.CombatType != ECombatType.Dodge && ctx.performed)
		{
			CombatComponent.SetComboInput(EKeystroke.LightAttack);
		}
	}

	private void StrongAttack(InputAction.CallbackContext ctx)
	{
		if (Character.LocomotionData.IsGrounded && Character.CombatData.CombatType != ECombatType.Dodge && ctx.performed)
		{
			CombatComponent.SetComboInput(EKeystroke.StrongAttack);
		}
	}

	private void HoldAttack(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && CombatComponent.ComboState == EComboState.Stop)
		{
			Character.CharacterAnim.SetBool("IsHold", true);
			Character.CharacterAnim.CrossFadeInFixedTime("Hold Attack", 0.1f);
		}
		else if (ctx.canceled)
		{
			Character.CharacterAnim.SetBool("IsHold", false);
		}
	}

	private void SlowMotion(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			Time.timeScale = 0.1f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}
}
