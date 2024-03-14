using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class ComboActions : IInputActionCollection2, IInputActionCollection, IEnumerable<InputAction>, IEnumerable, IDisposable
{
	public struct LocomotionActions
	{
		private ComboActions m_Wrapper;

		public InputAction Move => m_Wrapper.m_Locomotion_Move;

		public InputAction Sprint => m_Wrapper.m_Locomotion_Sprint;

		public InputAction Jump => m_Wrapper.m_Locomotion_Jump;

		public InputAction MoveMode => m_Wrapper.m_Locomotion_MoveMode;

		public InputAction SlowMotion => m_Wrapper.m_Locomotion_SlowMotion;

		public bool enabled => Get().enabled;

		public LocomotionActions(ComboActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Locomotion;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(LocomotionActions set)
		{
			return set.Get();
		}

		public void SetCallbacks(ILocomotionActions instance)
		{
			if (m_Wrapper.m_LocomotionActionsCallbackInterface != null)
			{
				Move.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove;
				Move.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove;
				Move.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove;
				Sprint.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnSprint;
				Sprint.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnSprint;
				Sprint.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnSprint;
				Jump.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump;
				Jump.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump;
				Jump.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump;
				MoveMode.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMoveMode;
				MoveMode.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMoveMode;
				MoveMode.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnMoveMode;
				SlowMotion.started -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnSlowMotion;
				SlowMotion.performed -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnSlowMotion;
				SlowMotion.canceled -= m_Wrapper.m_LocomotionActionsCallbackInterface.OnSlowMotion;
			}
			m_Wrapper.m_LocomotionActionsCallbackInterface = instance;
			if (instance != null)
			{
				Move.started += instance.OnMove;
				Move.performed += instance.OnMove;
				Move.canceled += instance.OnMove;
				Sprint.started += instance.OnSprint;
				Sprint.performed += instance.OnSprint;
				Sprint.canceled += instance.OnSprint;
				Jump.started += instance.OnJump;
				Jump.performed += instance.OnJump;
				Jump.canceled += instance.OnJump;
				MoveMode.started += instance.OnMoveMode;
				MoveMode.performed += instance.OnMoveMode;
				MoveMode.canceled += instance.OnMoveMode;
				SlowMotion.started += instance.OnSlowMotion;
				SlowMotion.performed += instance.OnSlowMotion;
				SlowMotion.canceled += instance.OnSlowMotion;
			}
		}
	}

	public struct CombatActions
	{
		private ComboActions m_Wrapper;

		public InputAction LightAttack => m_Wrapper.m_Combat_LightAttack;

		public InputAction StrongAttack => m_Wrapper.m_Combat_StrongAttack;

		public InputAction HoldAttack => m_Wrapper.m_Combat_HoldAttack;

		public InputAction Dodge => m_Wrapper.m_Combat_Dodge;

		public InputAction Finisher => m_Wrapper.m_Combat_Finisher;

		public bool enabled => Get().enabled;

		public CombatActions(ComboActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Combat;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(CombatActions set)
		{
			return set.Get();
		}

		public void SetCallbacks(ICombatActions instance)
		{
			if (m_Wrapper.m_CombatActionsCallbackInterface != null)
			{
				LightAttack.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack;
				LightAttack.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack;
				LightAttack.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack;
				StrongAttack.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack;
				StrongAttack.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack;
				StrongAttack.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack;
				HoldAttack.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnHoldAttack;
				HoldAttack.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnHoldAttack;
				HoldAttack.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnHoldAttack;
				Dodge.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnDodge;
				Dodge.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnDodge;
				Dodge.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnDodge;
				Finisher.started -= m_Wrapper.m_CombatActionsCallbackInterface.OnFinisher;
				Finisher.performed -= m_Wrapper.m_CombatActionsCallbackInterface.OnFinisher;
				Finisher.canceled -= m_Wrapper.m_CombatActionsCallbackInterface.OnFinisher;
			}
			m_Wrapper.m_CombatActionsCallbackInterface = instance;
			if (instance != null)
			{
				LightAttack.started += instance.OnLightAttack;
				LightAttack.performed += instance.OnLightAttack;
				LightAttack.canceled += instance.OnLightAttack;
				StrongAttack.started += instance.OnStrongAttack;
				StrongAttack.performed += instance.OnStrongAttack;
				StrongAttack.canceled += instance.OnStrongAttack;
				HoldAttack.started += instance.OnHoldAttack;
				HoldAttack.performed += instance.OnHoldAttack;
				HoldAttack.canceled += instance.OnHoldAttack;
				Dodge.started += instance.OnDodge;
				Dodge.performed += instance.OnDodge;
				Dodge.canceled += instance.OnDodge;
				Finisher.started += instance.OnFinisher;
				Finisher.performed += instance.OnFinisher;
				Finisher.canceled += instance.OnFinisher;
			}
		}
	}

	public struct CameraActions
	{
		private ComboActions m_Wrapper;

		public InputAction Look => m_Wrapper.m_Camera_Look;

		public bool enabled => Get().enabled;

		public CameraActions(ComboActions wrapper)
		{
			m_Wrapper = wrapper;
		}

		public InputActionMap Get()
		{
			return m_Wrapper.m_Camera;
		}

		public void Enable()
		{
			Get().Enable();
		}

		public void Disable()
		{
			Get().Disable();
		}

		public static implicit operator InputActionMap(CameraActions set)
		{
			return set.Get();
		}

		public void SetCallbacks(ICameraActions instance)
		{
			if (m_Wrapper.m_CameraActionsCallbackInterface != null)
			{
				Look.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnLook;
				Look.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnLook;
				Look.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnLook;
			}
			m_Wrapper.m_CameraActionsCallbackInterface = instance;
			if (instance != null)
			{
				Look.started += instance.OnLook;
				Look.performed += instance.OnLook;
				Look.canceled += instance.OnLook;
			}
		}
	}

	public interface ILocomotionActions
	{
		void OnMove(InputAction.CallbackContext context);

		void OnSprint(InputAction.CallbackContext context);

		void OnJump(InputAction.CallbackContext context);

		void OnMoveMode(InputAction.CallbackContext context);

		void OnSlowMotion(InputAction.CallbackContext context);
	}

	public interface ICombatActions
	{
		void OnLightAttack(InputAction.CallbackContext context);

		void OnStrongAttack(InputAction.CallbackContext context);

		void OnHoldAttack(InputAction.CallbackContext context);

		void OnDodge(InputAction.CallbackContext context);

		void OnFinisher(InputAction.CallbackContext context);
	}

	public interface ICameraActions
	{
		void OnLook(InputAction.CallbackContext context);
	}

	private readonly InputActionMap m_Locomotion;

	private ILocomotionActions m_LocomotionActionsCallbackInterface;

	private readonly InputAction m_Locomotion_Move;

	private readonly InputAction m_Locomotion_Sprint;

	private readonly InputAction m_Locomotion_Jump;

	private readonly InputAction m_Locomotion_MoveMode;

	private readonly InputAction m_Locomotion_SlowMotion;

	private readonly InputActionMap m_Combat;

	private ICombatActions m_CombatActionsCallbackInterface;

	private readonly InputAction m_Combat_LightAttack;

	private readonly InputAction m_Combat_StrongAttack;

	private readonly InputAction m_Combat_HoldAttack;

	private readonly InputAction m_Combat_Dodge;

	private readonly InputAction m_Combat_Finisher;

	private readonly InputActionMap m_Camera;

	private ICameraActions m_CameraActionsCallbackInterface;

	private readonly InputAction m_Camera_Look;

	private int m_PCSchemeIndex = -1;

	private int m_PS5SchemeIndex = -1;

	public InputActionAsset asset { get; }

	public InputBinding? bindingMask
	{
		get
		{
			return asset.bindingMask;
		}
		set
		{
			asset.bindingMask = value;
		}
	}

	public ReadOnlyArray<InputDevice>? devices
	{
		get
		{
			return asset.devices;
		}
		set
		{
			asset.devices = value;
		}
	}

	public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

	public IEnumerable<InputBinding> bindings => asset.bindings;

	public LocomotionActions Locomotion => new LocomotionActions(this);

	public CombatActions Combat => new CombatActions(this);

	public CameraActions Camera => new CameraActions(this);

	public InputControlScheme PCScheme
	{
		get
		{
			if (m_PCSchemeIndex == -1)
			{
				m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
			}
			return asset.controlSchemes[m_PCSchemeIndex];
		}
	}

	public InputControlScheme PS5Scheme
	{
		get
		{
			if (m_PS5SchemeIndex == -1)
			{
				m_PS5SchemeIndex = asset.FindControlSchemeIndex("PS5");
			}
			return asset.controlSchemes[m_PS5SchemeIndex];
		}
	}

	public ComboActions()
	{
		asset = InputActionAsset.FromJson("{\n    \"name\": \"Combo Actions\",\n    \"maps\": [\n        {\n            \"name\": \"Locomotion\",\n            \"id\": \"b3def5e6-6e75-4ed0-8628-588e1666f4a8\",\n            \"actions\": [\n                {\n                    \"name\": \"Move\",\n                    \"type\": \"Value\",\n                    \"id\": \"7e3b99c7-0de3-459a-a0e8-aea2dbc16be8\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Sprint\",\n                    \"type\": \"Button\",\n                    \"id\": \"6e399c00-4798-4402-87ab-581ee5aa7260\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Jump\",\n                    \"type\": \"Button\",\n                    \"id\": \"284632bf-23b4-4c15-9c21-4aaaab5a8287\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Move Mode\",\n                    \"type\": \"Button\",\n                    \"id\": \"e3edf50b-e0ac-481f-ae90-925950d32d7d\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Slow Motion\",\n                    \"type\": \"Button\",\n                    \"id\": \"6c14c970-4b5f-4f1f-b7e2-cb712101e1c8\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"2D Vector\",\n                    \"id\": \"08cb223d-8c39-422e-a29e-f2366830d7e0\",\n                    \"path\": \"2DVector(mode=1)\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"cf82fa17-8093-4bb4-a912-09f78b254e31\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"771f3206-d547-4e76-8ed5-cdb415586e51\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"23628870-8ebe-4fb8-b914-c7dcd152474a\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"5d2d48de-6647-4c4d-bb01-70b5d6e88570\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"2D Vector\",\n                    \"id\": \"be730e2f-94c8-44ed-9c3f-278ef0f925be\",\n                    \"path\": \"2DVector(mode=1)\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"04521375-58d8-4b68-acc3-3d7dd745b045\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"7fc7dec6-3022-4aeb-bdbf-0482ff89fcb3\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"3d8e758d-7ca5-42ad-9311-0c95d2d318f1\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"7045bf19-7d50-4976-b487-e00f165888de\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6db3f47e-a83e-4b3d-8144-7753ce45bd7b\",\n                    \"path\": \"<Keyboard>/leftShift\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Sprint\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"0ebacd4c-217a-4de5-9db4-5dd682e7096d\",\n                    \"path\": \"<DualSenseGamepadHID>/rightShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Sprint\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"e77de00c-1844-4108-a406-37f29e817aa5\",\n                    \"path\": \"<Keyboard>/space\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Jump\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"54236667-68fb-4b1b-8dfe-9e1045858f0b\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Jump\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"596a71b2-9431-41a2-a25f-43a54add4d82\",\n                    \"path\": \"<Keyboard>/tab\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move Mode\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"10e57300-cc67-4309-94e7-5352ea41e3dd\",\n                    \"path\": \"<DualSenseGamepadHID>/leftShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move Mode\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"9733dfa0-d011-44e3-9b80-676099e2715e\",\n                    \"path\": \"<DualSenseGamepadHID>/leftTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Slow Motion\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Combat\",\n            \"id\": \"65b8f3a9-fac5-4928-9640-d6bf6185834f\",\n            \"actions\": [\n                {\n                    \"name\": \"Light Attack\",\n                    \"type\": \"Button\",\n                    \"id\": \"a4b20aae-8b48-4d88-b3c3-2817a67a34f8\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Strong Attack\",\n                    \"type\": \"Button\",\n                    \"id\": \"364dcea3-95a8-4856-bdc1-8feeda491b7c\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Hold Attack\",\n                    \"type\": \"Button\",\n                    \"id\": \"b319bcaf-a386-4b25-9d8b-7c552fddd459\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Dodge\",\n                    \"type\": \"Button\",\n                    \"id\": \"94043a5d-f04f-4b42-8d70-36d83e24cfd7\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Finisher\",\n                    \"type\": \"Button\",\n                    \"id\": \"646859c9-470b-4ee5-ba94-5a42c8ce93e7\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"One Modifier\",\n                    \"id\": \"7eab6b34-661f-4dc7-b470-d06c52923446\",\n                    \"path\": \"OneModifier\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Light Attack\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"361ff217-5cda-495e-b700-8d3ae1756727\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Light Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c532e179-0b33-4810-ab20-c094192585a5\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonWest\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Light Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"One Modifier\",\n                    \"id\": \"40c5e407-0bb9-4408-9347-7bc21689f0c4\",\n                    \"path\": \"OneModifier\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Strong Attack\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"14315f50-c9b7-469a-a5c6-bd530bf295f3\",\n                    \"path\": \"<Mouse>/rightButton\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Strong Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d8132bb3-0e90-4304-84a5-bce77d2d050d\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonNorth\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Strong Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"f1ddd949-392f-4724-9559-501448e52a21\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"Hold(duration=0.35)\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Hold Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"bcd4876e-2ed0-4108-af7c-2463f2e91fec\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonWest\",\n                    \"interactions\": \"Hold(duration=0.35)\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Hold Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d6617e59-791b-43bd-baf8-762e6e29b38f\",\n                    \"path\": \"<Keyboard>/leftCtrl\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Dodge\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"0c287d00-f803-4666-8d0f-b50c77ebb7bc\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonEast\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Dodge\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"63277ff2-3d80-4e55-a775-97066229d86f\",\n                    \"path\": \"<Keyboard>/f\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Finisher\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"78faf6ce-caa0-443b-be9f-22f165124e4e\",\n                    \"path\": \"<DualSenseGamepadHID>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Finisher\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Camera\",\n            \"id\": \"5204e717-ed12-41cf-8bd8-30076ccacef3\",\n            \"actions\": [\n                {\n                    \"name\": \"Look\",\n                    \"type\": \"Value\",\n                    \"id\": \"f2df47ca-a56e-489a-a6bb-472ad4bb31f9\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"67f52271-efd8-4475-b014-8a5a99be6e41\",\n                    \"path\": \"<DualSenseGamepadHID>/rightStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"7c89536b-5633-4939-9e91-40a7627d3931\",\n                    \"path\": \"<Mouse>/delta\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        }\n    ],\n    \"controlSchemes\": [\n        {\n            \"name\": \"PC\",\n            \"bindingGroup\": \"PC\",\n            \"devices\": []\n        },\n        {\n            \"name\": \"PS5\",\n            \"bindingGroup\": \"PS5\",\n            \"devices\": [\n                {\n                    \"devicePath\": \"<DualSenseGamepadHID>\",\n                    \"isOptional\": false,\n                    \"isOR\": false\n                }\n            ]\n        }\n    ]\n}");
		m_Locomotion = asset.FindActionMap("Locomotion", throwIfNotFound: true);
		m_Locomotion_Move = m_Locomotion.FindAction("Move", throwIfNotFound: true);
		m_Locomotion_Sprint = m_Locomotion.FindAction("Sprint", throwIfNotFound: true);
		m_Locomotion_Jump = m_Locomotion.FindAction("Jump", throwIfNotFound: true);
		m_Locomotion_MoveMode = m_Locomotion.FindAction("Move Mode", throwIfNotFound: true);
		m_Locomotion_SlowMotion = m_Locomotion.FindAction("Slow Motion", throwIfNotFound: true);
		m_Combat = asset.FindActionMap("Combat", throwIfNotFound: true);
		m_Combat_LightAttack = m_Combat.FindAction("Light Attack", throwIfNotFound: true);
		m_Combat_StrongAttack = m_Combat.FindAction("Strong Attack", throwIfNotFound: true);
		m_Combat_HoldAttack = m_Combat.FindAction("Hold Attack", throwIfNotFound: true);
		m_Combat_Dodge = m_Combat.FindAction("Dodge", throwIfNotFound: true);
		m_Combat_Finisher = m_Combat.FindAction("Finisher", throwIfNotFound: true);
		m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
		m_Camera_Look = m_Camera.FindAction("Look", throwIfNotFound: true);
	}

	public void Dispose()
	{
		UnityEngine.Object.Destroy(asset);
	}

	public bool Contains(InputAction action)
	{
		return asset.Contains(action);
	}

	public IEnumerator<InputAction> GetEnumerator()
	{
		return asset.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Enable()
	{
		asset.Enable();
	}

	public void Disable()
	{
		asset.Disable();
	}

	public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
	{
		return asset.FindAction(actionNameOrId, throwIfNotFound);
	}

	public int FindBinding(InputBinding bindingMask, out InputAction action)
	{
		return asset.FindBinding(bindingMask, out action);
	}
}
