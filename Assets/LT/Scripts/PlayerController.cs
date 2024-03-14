using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CombatComponent))]
public class PlayerController : MonoBehaviour
{
	[Header("[Component]")]
	private Character Character;

	private CombatComponent CombatComponent;

	[Header("[Input System]")]
	private ComboActions ComboActions;

	public ComboActions GetComboActions => ComboActions;

	private void Awake()
	{
		Initialize();
	}

	private void OnEnable()
	{
		ComboActions.Locomotion.Enable();
		ComboActions.Combat.Enable();
	}

	private void OnDisable()
	{
		ComboActions.Locomotion.Disable();
		ComboActions.Combat.Disable();
	}

	private void Initialize()
	{
		Character = GetComponent<Character>();
		CombatComponent = GetComponent<CombatComponent>();
		ComboActions = new ComboActions();
		InputBinding();
	}

	private void InputBinding()
	{
		ComboActions.Locomotion.Sprint.performed += Sprint;
		ComboActions.Locomotion.Jump.performed += Jump;
		ComboActions.Combat.LightAttack.performed += LightAttack;
		ComboActions.Combat.StrongAttack.performed += StrongAttack;
		ComboActions.Combat.HoldAttack.performed += HoldAttack;
		ComboActions.Combat.Dodge.performed += Dodge;
		ComboActions.Locomotion.Sprint.canceled += Sprint;
		ComboActions.Combat.HoldAttack.canceled += HoldAttack;
	}

	private void Sprint(InputAction.CallbackContext ctx)
	{
	}

	private void Jump(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			if (Character.LocomotionData.IsGrounded)
			{
				Character.CharacterAnim.CrossFadeInFixedTime("Jump", 0.1f);
				Character.CharacterOptional.jumpCount++;
			}
			else if (Character.CharacterOptional.isDoubleJump && Character.CharacterOptional.jumpCount == 1)
			{
				Character.CharacterAnim.CrossFadeInFixedTime("Double Jump", 0.1f);
				Character.CharacterOptional.jumpCount++;
			}
			CombatComponent.ResetCombo();
		}
	}

	private void LightAttack(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
		{
			CombatComponent.SetComboInput(EKeystroke.LightAttack);
		}
	}

	private void StrongAttack(InputAction.CallbackContext ctx)
	{
		if (ctx.performed)
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

	private void Dodge(InputAction.CallbackContext ctx)
	{
		if (ctx.performed && Character.CombatData.CombatType != ECombatType.Dodge)
		{
			Character.CharacterAnim.CrossFadeInFixedTime("Dodge", 0.1f);
			CombatComponent.ResetCombo();
		}
	}
}
