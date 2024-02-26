// Decompiled with JetBrains decompiler
// Type: ComboActions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

#nullable disable
public class ComboActions : 
  IInputActionCollection2,
  IInputActionCollection,
  IEnumerable<InputAction>,
  IEnumerable,
  IDisposable
{
  private readonly InputActionMap m_Locomotion;
  private ComboActions.ILocomotionActions m_LocomotionActionsCallbackInterface;
  private readonly InputAction m_Locomotion_Move;
  private readonly InputAction m_Locomotion_Sprint;
  private readonly InputAction m_Locomotion_Jump;
  private readonly InputAction m_Locomotion_MoveMode;
  private readonly InputAction m_Locomotion_SlowMotion;
  private readonly InputActionMap m_Combat;
  private ComboActions.ICombatActions m_CombatActionsCallbackInterface;
  private readonly InputAction m_Combat_LightAttack;
  private readonly InputAction m_Combat_StrongAttack;
  private readonly InputAction m_Combat_HoldAttack;
  private readonly InputAction m_Combat_Dodge;
  private readonly InputAction m_Combat_Finisher;
  private readonly InputActionMap m_Camera;
  private ComboActions.ICameraActions m_CameraActionsCallbackInterface;
  private readonly InputAction m_Camera_Look;
  private int m_PCSchemeIndex = -1;
  private int m_PS5SchemeIndex = -1;

  public InputActionAsset asset { get; }

  public ComboActions()
  {
    this.asset = InputActionAsset.FromJson("{\n    \"name\": \"Combo Actions\",\n    \"maps\": [\n        {\n            \"name\": \"Locomotion\",\n            \"id\": \"b3def5e6-6e75-4ed0-8628-588e1666f4a8\",\n            \"actions\": [\n                {\n                    \"name\": \"Move\",\n                    \"type\": \"Value\",\n                    \"id\": \"7e3b99c7-0de3-459a-a0e8-aea2dbc16be8\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                },\n                {\n                    \"name\": \"Sprint\",\n                    \"type\": \"Button\",\n                    \"id\": \"6e399c00-4798-4402-87ab-581ee5aa7260\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Jump\",\n                    \"type\": \"Button\",\n                    \"id\": \"284632bf-23b4-4c15-9c21-4aaaab5a8287\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Move Mode\",\n                    \"type\": \"Button\",\n                    \"id\": \"e3edf50b-e0ac-481f-ae90-925950d32d7d\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Slow Motion\",\n                    \"type\": \"Button\",\n                    \"id\": \"6c14c970-4b5f-4f1f-b7e2-cb712101e1c8\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"2D Vector\",\n                    \"id\": \"08cb223d-8c39-422e-a29e-f2366830d7e0\",\n                    \"path\": \"2DVector(mode=1)\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"cf82fa17-8093-4bb4-a912-09f78b254e31\",\n                    \"path\": \"<Keyboard>/w\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"771f3206-d547-4e76-8ed5-cdb415586e51\",\n                    \"path\": \"<Keyboard>/s\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"23628870-8ebe-4fb8-b914-c7dcd152474a\",\n                    \"path\": \"<Keyboard>/a\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"5d2d48de-6647-4c4d-bb01-70b5d6e88570\",\n                    \"path\": \"<Keyboard>/d\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"2D Vector\",\n                    \"id\": \"be730e2f-94c8-44ed-9c3f-278ef0f925be\",\n                    \"path\": \"2DVector(mode=1)\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Move\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"up\",\n                    \"id\": \"04521375-58d8-4b68-acc3-3d7dd745b045\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/up\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"down\",\n                    \"id\": \"7fc7dec6-3022-4aeb-bdbf-0482ff89fcb3\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/down\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"left\",\n                    \"id\": \"3d8e758d-7ca5-42ad-9311-0c95d2d318f1\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/left\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"right\",\n                    \"id\": \"7045bf19-7d50-4976-b487-e00f165888de\",\n                    \"path\": \"<DualSenseGamepadHID>/leftStick/right\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": true\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"6db3f47e-a83e-4b3d-8144-7753ce45bd7b\",\n                    \"path\": \"<Keyboard>/leftShift\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Sprint\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"0ebacd4c-217a-4de5-9db4-5dd682e7096d\",\n                    \"path\": \"<DualSenseGamepadHID>/rightShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Sprint\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"e77de00c-1844-4108-a406-37f29e817aa5\",\n                    \"path\": \"<Keyboard>/space\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Jump\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"54236667-68fb-4b1b-8dfe-9e1045858f0b\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonSouth\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Jump\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"596a71b2-9431-41a2-a25f-43a54add4d82\",\n                    \"path\": \"<Keyboard>/tab\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Move Mode\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"10e57300-cc67-4309-94e7-5352ea41e3dd\",\n                    \"path\": \"<DualSenseGamepadHID>/leftShoulder\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Move Mode\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"9733dfa0-d011-44e3-9b80-676099e2715e\",\n                    \"path\": \"<DualSenseGamepadHID>/leftTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Slow Motion\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Combat\",\n            \"id\": \"65b8f3a9-fac5-4928-9640-d6bf6185834f\",\n            \"actions\": [\n                {\n                    \"name\": \"Light Attack\",\n                    \"type\": \"Button\",\n                    \"id\": \"a4b20aae-8b48-4d88-b3c3-2817a67a34f8\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Strong Attack\",\n                    \"type\": \"Button\",\n                    \"id\": \"364dcea3-95a8-4856-bdc1-8feeda491b7c\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Hold Attack\",\n                    \"type\": \"Button\",\n                    \"id\": \"b319bcaf-a386-4b25-9d8b-7c552fddd459\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Dodge\",\n                    \"type\": \"Button\",\n                    \"id\": \"94043a5d-f04f-4b42-8d70-36d83e24cfd7\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                },\n                {\n                    \"name\": \"Finisher\",\n                    \"type\": \"Button\",\n                    \"id\": \"646859c9-470b-4ee5-ba94-5a42c8ce93e7\",\n                    \"expectedControlType\": \"Button\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": false\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"One Modifier\",\n                    \"id\": \"7eab6b34-661f-4dc7-b470-d06c52923446\",\n                    \"path\": \"OneModifier\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Light Attack\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"361ff217-5cda-495e-b700-8d3ae1756727\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Light Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"c532e179-0b33-4810-ab20-c094192585a5\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonWest\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Light Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"One Modifier\",\n                    \"id\": \"40c5e407-0bb9-4408-9347-7bc21689f0c4\",\n                    \"path\": \"OneModifier\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"\",\n                    \"action\": \"Strong Attack\",\n                    \"isComposite\": true,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"14315f50-c9b7-469a-a5c6-bd530bf295f3\",\n                    \"path\": \"<Mouse>/rightButton\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Strong Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d8132bb3-0e90-4304-84a5-bce77d2d050d\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonNorth\",\n                    \"interactions\": \"Tap\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Strong Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"f1ddd949-392f-4724-9559-501448e52a21\",\n                    \"path\": \"<Mouse>/leftButton\",\n                    \"interactions\": \"Hold(duration=0.35)\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Hold Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"bcd4876e-2ed0-4108-af7c-2463f2e91fec\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonWest\",\n                    \"interactions\": \"Hold(duration=0.35)\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Hold Attack\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"d6617e59-791b-43bd-baf8-762e6e29b38f\",\n                    \"path\": \"<Keyboard>/leftCtrl\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Dodge\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"0c287d00-f803-4666-8d0f-b50c77ebb7bc\",\n                    \"path\": \"<DualSenseGamepadHID>/buttonEast\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Dodge\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"63277ff2-3d80-4e55-a775-97066229d86f\",\n                    \"path\": \"<Keyboard>/f\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Finisher\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"78faf6ce-caa0-443b-be9f-22f165124e4e\",\n                    \"path\": \"<DualSenseGamepadHID>/rightTrigger\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Finisher\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        },\n        {\n            \"name\": \"Camera\",\n            \"id\": \"5204e717-ed12-41cf-8bd8-30076ccacef3\",\n            \"actions\": [\n                {\n                    \"name\": \"Look\",\n                    \"type\": \"Value\",\n                    \"id\": \"f2df47ca-a56e-489a-a6bb-472ad4bb31f9\",\n                    \"expectedControlType\": \"Vector2\",\n                    \"processors\": \"\",\n                    \"interactions\": \"\",\n                    \"initialStateCheck\": true\n                }\n            ],\n            \"bindings\": [\n                {\n                    \"name\": \"\",\n                    \"id\": \"67f52271-efd8-4475-b014-8a5a99be6e41\",\n                    \"path\": \"<DualSenseGamepadHID>/rightStick\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PS5\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                },\n                {\n                    \"name\": \"\",\n                    \"id\": \"7c89536b-5633-4939-9e91-40a7627d3931\",\n                    \"path\": \"<Mouse>/delta\",\n                    \"interactions\": \"\",\n                    \"processors\": \"\",\n                    \"groups\": \"PC\",\n                    \"action\": \"Look\",\n                    \"isComposite\": false,\n                    \"isPartOfComposite\": false\n                }\n            ]\n        }\n    ],\n    \"controlSchemes\": [\n        {\n            \"name\": \"PC\",\n            \"bindingGroup\": \"PC\",\n            \"devices\": []\n        },\n        {\n            \"name\": \"PS5\",\n            \"bindingGroup\": \"PS5\",\n            \"devices\": [\n                {\n                    \"devicePath\": \"<DualSenseGamepadHID>\",\n                    \"isOptional\": false,\n                    \"isOR\": false\n                }\n            ]\n        }\n    ]\n}");
    this.m_Locomotion = this.asset.FindActionMap(nameof (Locomotion), true);
    this.m_Locomotion_Move = this.m_Locomotion.FindAction("Move", true);
    this.m_Locomotion_Sprint = this.m_Locomotion.FindAction("Sprint", true);
    this.m_Locomotion_Jump = this.m_Locomotion.FindAction("Jump", true);
    this.m_Locomotion_MoveMode = this.m_Locomotion.FindAction("Move Mode", true);
    this.m_Locomotion_SlowMotion = this.m_Locomotion.FindAction("Slow Motion", true);
    this.m_Combat = this.asset.FindActionMap(nameof (Combat), true);
    this.m_Combat_LightAttack = this.m_Combat.FindAction("Light Attack", true);
    this.m_Combat_StrongAttack = this.m_Combat.FindAction("Strong Attack", true);
    this.m_Combat_HoldAttack = this.m_Combat.FindAction("Hold Attack", true);
    this.m_Combat_Dodge = this.m_Combat.FindAction("Dodge", true);
    this.m_Combat_Finisher = this.m_Combat.FindAction("Finisher", true);
    this.m_Camera = this.asset.FindActionMap(nameof (Camera), true);
    this.m_Camera_Look = this.m_Camera.FindAction("Look", true);
  }

  public void Dispose() => UnityEngine.Object.Destroy((UnityEngine.Object) this.asset);

  public InputBinding? bindingMask
  {
    get => this.asset.bindingMask;
    set => this.asset.bindingMask = value;
  }

  public ReadOnlyArray<InputDevice>? devices
  {
    get => this.asset.devices;
    set => this.asset.devices = value;
  }

  public ReadOnlyArray<InputControlScheme> controlSchemes => this.asset.controlSchemes;

  public bool Contains(InputAction action) => this.asset.Contains(action);

  public IEnumerator<InputAction> GetEnumerator() => this.asset.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public void Enable() => this.asset.Enable();

  public void Disable() => this.asset.Disable();

  public IEnumerable<InputBinding> bindings => this.asset.bindings;

  public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
  {
    return this.asset.FindAction(actionNameOrId, throwIfNotFound);
  }

  public int FindBinding(InputBinding bindingMask, out InputAction action)
  {
    return this.asset.FindBinding(bindingMask, out action);
  }

  public ComboActions.LocomotionActions Locomotion => new ComboActions.LocomotionActions(this);

  public ComboActions.CombatActions Combat => new ComboActions.CombatActions(this);

  public ComboActions.CameraActions Camera => new ComboActions.CameraActions(this);

  public InputControlScheme PCScheme
  {
    get
    {
      if (this.m_PCSchemeIndex == -1)
        this.m_PCSchemeIndex = this.asset.FindControlSchemeIndex("PC");
      return this.asset.controlSchemes[this.m_PCSchemeIndex];
    }
  }

  public InputControlScheme PS5Scheme
  {
    get
    {
      if (this.m_PS5SchemeIndex == -1)
        this.m_PS5SchemeIndex = this.asset.FindControlSchemeIndex("PS5");
      return this.asset.controlSchemes[this.m_PS5SchemeIndex];
    }
  }

  public struct LocomotionActions
  {
    private ComboActions m_Wrapper;

    public LocomotionActions(ComboActions wrapper) => this.m_Wrapper = wrapper;

    public InputAction Move => this.m_Wrapper.m_Locomotion_Move;

    public InputAction Sprint => this.m_Wrapper.m_Locomotion_Sprint;

    public InputAction Jump => this.m_Wrapper.m_Locomotion_Jump;

    public InputAction MoveMode => this.m_Wrapper.m_Locomotion_MoveMode;

    public InputAction SlowMotion => this.m_Wrapper.m_Locomotion_SlowMotion;

    public InputActionMap Get() => this.m_Wrapper.m_Locomotion;

    public void Enable() => this.Get().Enable();

    public void Disable() => this.Get().Disable();

    public bool enabled => this.Get().enabled;

    public static implicit operator InputActionMap(ComboActions.LocomotionActions set) => set.Get();

    public void SetCallbacks(ComboActions.ILocomotionActions instance)
    {
      if (this.m_Wrapper.m_LocomotionActionsCallbackInterface != null)
      {
        this.Move.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove);
        this.Move.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove);
        this.Move.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnMove);
        this.Sprint.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnSprint);
        this.Sprint.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnSprint);
        this.Sprint.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnSprint);
        this.Jump.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump);
        this.Jump.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump);
        this.Jump.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnJump);
        this.MoveMode.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnMoveMode);
        this.MoveMode.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnMoveMode);
        this.MoveMode.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnMoveMode);
        this.SlowMotion.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnSlowMotion);
        this.SlowMotion.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnSlowMotion);
        this.SlowMotion.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_LocomotionActionsCallbackInterface.OnSlowMotion);
      }
      this.m_Wrapper.m_LocomotionActionsCallbackInterface = instance;
      if (instance == null)
        return;
      this.Move.started += new Action<InputAction.CallbackContext>(instance.OnMove);
      this.Move.performed += new Action<InputAction.CallbackContext>(instance.OnMove);
      this.Move.canceled += new Action<InputAction.CallbackContext>(instance.OnMove);
      this.Sprint.started += new Action<InputAction.CallbackContext>(instance.OnSprint);
      this.Sprint.performed += new Action<InputAction.CallbackContext>(instance.OnSprint);
      this.Sprint.canceled += new Action<InputAction.CallbackContext>(instance.OnSprint);
      this.Jump.started += new Action<InputAction.CallbackContext>(instance.OnJump);
      this.Jump.performed += new Action<InputAction.CallbackContext>(instance.OnJump);
      this.Jump.canceled += new Action<InputAction.CallbackContext>(instance.OnJump);
      this.MoveMode.started += new Action<InputAction.CallbackContext>(instance.OnMoveMode);
      this.MoveMode.performed += new Action<InputAction.CallbackContext>(instance.OnMoveMode);
      this.MoveMode.canceled += new Action<InputAction.CallbackContext>(instance.OnMoveMode);
      this.SlowMotion.started += new Action<InputAction.CallbackContext>(instance.OnSlowMotion);
      this.SlowMotion.performed += new Action<InputAction.CallbackContext>(instance.OnSlowMotion);
      this.SlowMotion.canceled += new Action<InputAction.CallbackContext>(instance.OnSlowMotion);
    }
  }

  public struct CombatActions
  {
    private ComboActions m_Wrapper;

    public CombatActions(ComboActions wrapper) => this.m_Wrapper = wrapper;

    public InputAction LightAttack => this.m_Wrapper.m_Combat_LightAttack;

    public InputAction StrongAttack => this.m_Wrapper.m_Combat_StrongAttack;

    public InputAction HoldAttack => this.m_Wrapper.m_Combat_HoldAttack;

    public InputAction Dodge => this.m_Wrapper.m_Combat_Dodge;

    public InputAction Finisher => this.m_Wrapper.m_Combat_Finisher;

    public InputActionMap Get() => this.m_Wrapper.m_Combat;

    public void Enable() => this.Get().Enable();

    public void Disable() => this.Get().Disable();

    public bool enabled => this.Get().enabled;

    public static implicit operator InputActionMap(ComboActions.CombatActions set) => set.Get();

    public void SetCallbacks(ComboActions.ICombatActions instance)
    {
      if (this.m_Wrapper.m_CombatActionsCallbackInterface != null)
      {
        this.LightAttack.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack);
        this.LightAttack.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack);
        this.LightAttack.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnLightAttack);
        this.StrongAttack.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack);
        this.StrongAttack.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack);
        this.StrongAttack.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnStrongAttack);
        this.HoldAttack.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnHoldAttack);
        this.HoldAttack.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnHoldAttack);
        this.HoldAttack.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnHoldAttack);
        this.Dodge.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnDodge);
        this.Dodge.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnDodge);
        this.Dodge.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnDodge);
        this.Finisher.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnFinisher);
        this.Finisher.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnFinisher);
        this.Finisher.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CombatActionsCallbackInterface.OnFinisher);
      }
      this.m_Wrapper.m_CombatActionsCallbackInterface = instance;
      if (instance == null)
        return;
      this.LightAttack.started += new Action<InputAction.CallbackContext>(instance.OnLightAttack);
      this.LightAttack.performed += new Action<InputAction.CallbackContext>(instance.OnLightAttack);
      this.LightAttack.canceled += new Action<InputAction.CallbackContext>(instance.OnLightAttack);
      this.StrongAttack.started += new Action<InputAction.CallbackContext>(instance.OnStrongAttack);
      this.StrongAttack.performed += new Action<InputAction.CallbackContext>(instance.OnStrongAttack);
      this.StrongAttack.canceled += new Action<InputAction.CallbackContext>(instance.OnStrongAttack);
      this.HoldAttack.started += new Action<InputAction.CallbackContext>(instance.OnHoldAttack);
      this.HoldAttack.performed += new Action<InputAction.CallbackContext>(instance.OnHoldAttack);
      this.HoldAttack.canceled += new Action<InputAction.CallbackContext>(instance.OnHoldAttack);
      this.Dodge.started += new Action<InputAction.CallbackContext>(instance.OnDodge);
      this.Dodge.performed += new Action<InputAction.CallbackContext>(instance.OnDodge);
      this.Dodge.canceled += new Action<InputAction.CallbackContext>(instance.OnDodge);
      this.Finisher.started += new Action<InputAction.CallbackContext>(instance.OnFinisher);
      this.Finisher.performed += new Action<InputAction.CallbackContext>(instance.OnFinisher);
      this.Finisher.canceled += new Action<InputAction.CallbackContext>(instance.OnFinisher);
    }
  }

  public struct CameraActions
  {
    private ComboActions m_Wrapper;

    public CameraActions(ComboActions wrapper) => this.m_Wrapper = wrapper;

    public InputAction Look => this.m_Wrapper.m_Camera_Look;

    public InputActionMap Get() => this.m_Wrapper.m_Camera;

    public void Enable() => this.Get().Enable();

    public void Disable() => this.Get().Disable();

    public bool enabled => this.Get().enabled;

    public static implicit operator InputActionMap(ComboActions.CameraActions set) => set.Get();

    public void SetCallbacks(ComboActions.ICameraActions instance)
    {
      if (this.m_Wrapper.m_CameraActionsCallbackInterface != null)
      {
        this.Look.started -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CameraActionsCallbackInterface.OnLook);
        this.Look.performed -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CameraActionsCallbackInterface.OnLook);
        this.Look.canceled -= new Action<InputAction.CallbackContext>(this.m_Wrapper.m_CameraActionsCallbackInterface.OnLook);
      }
      this.m_Wrapper.m_CameraActionsCallbackInterface = instance;
      if (instance == null)
        return;
      this.Look.started += new Action<InputAction.CallbackContext>(instance.OnLook);
      this.Look.performed += new Action<InputAction.CallbackContext>(instance.OnLook);
      this.Look.canceled += new Action<InputAction.CallbackContext>(instance.OnLook);
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
}
