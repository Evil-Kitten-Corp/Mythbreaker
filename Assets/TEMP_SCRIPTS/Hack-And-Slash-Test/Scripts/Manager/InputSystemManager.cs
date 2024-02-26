// Decompiled with JetBrains decompiler
// Type: InputSystemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
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

  public ComboActions GetInputAction => this.InputAction;

  protected override void OnAwake()
  {
    base.OnAwake();
    this.Initialize();
  }

  private void Update() => this.Sprint();

  private void OnEnable()
  {
    this.InputAction.Locomotion.Enable();
    this.InputAction.Combat.Enable();
  }

  private void OnDisable()
  {
    this.InputAction.Locomotion.Disable();
    this.InputAction.Combat.Disable();
  }

  public void Initialize()
  {
    this.InputAction = new ComboActions();
    this.InputBinding();
    this.SetCursorMode(this.CursorMode);
  }

  private void InputBinding()
  {
    this.InputAction.Locomotion.Sprint.performed += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.Sprint);
    this.InputAction.Locomotion.Jump.performed += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.Jump);
    this.InputAction.Locomotion.SlowMotion.performed += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.SlowMotion);
    this.InputAction.Combat.Dodge.performed += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.Dodge);
    this.InputAction.Combat.LightAttack.performed += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.LightAttack);
    this.InputAction.Combat.StrongAttack.performed += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.StrongAttack);
    this.InputAction.Combat.HoldAttack.performed += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.HoldAttack);
    this.InputAction.Locomotion.SlowMotion.canceled += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.SlowMotion);
    this.InputAction.Combat.HoldAttack.canceled += new Action<UnityEngine.InputSystem.InputAction.CallbackContext>(this.HoldAttack);
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
    this.CursorMode = mode;
  }

  public void Sprint()
  {
    if (this.IsToggle || !this.Character.LocomotionData.IsGrounded)
      return;
    this.Character.LocomotionData.IsSprint = this.InputAction.Locomotion.Sprint.phase == InputActionPhase.Performed;
    this.Character.CharacterAnim.SetBool("IsSprint", this.Character.LocomotionData.IsSprint);
  }

  private IEnumerator Jump()
  {
    this.Character.LocomotionData.IsJump = true;
    this.Character.LocomotionData.IsGrounded = false;
    this.Character.CharacterAnim.SetBool("IsGrounded", false);
    this.Character.LocomotionData.MovementState = eMovementState.InAir;
    this.Character.FallingEvent();
    this.Character.LocomotionData.JumpForce = 5f;
    if (this.Character.CharacterOptional.jumpCount == 0)
      this.Character.CharacterAnim.CrossFadeInFixedTime(nameof (Jump), 0.1f);
    else if (this.Character.CharacterOptional.jumpCount == 1)
      this.Character.CharacterAnim.CrossFadeInFixedTime("Double Jump", 0.25f);
    ++this.Character.CharacterOptional.jumpCount;
    float jumpTime = 0.0f;
    while (!this.Character.LocomotionData.IsGrounded)
    {
      jumpTime += Time.deltaTime;
      if ((double) jumpTime > 0.20000000298023224)
        this.Character.LocomotionData.IsJump = false;
      this.Character.AirControl(this.Character.LocomotionData.JumpForce);
      yield return (object) null;
    }
  }

  public void Sprint(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
  {
    if (!this.IsToggle || !this.Character.LocomotionData.IsGrounded || !ctx.performed)
      return;
    this.Character.LocomotionData.IsSprint = !this.Character.LocomotionData.IsSprint;
    this.Character.CharacterAnim.SetBool("IsSprint", this.Character.LocomotionData.IsSprint);
  }

  public void Jump(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
  {
    if (!ctx.performed || this.Character.CharacterOptional.jumpCount >= 2 || (double) this.JumpDelayTime > (double) Time.time)
      return;
    this.JumpDelayTime = Time.time + 0.1f;
    if (this.C_Jump != null)
      this.StopCoroutine(this.C_Jump);
    this.C_Jump = this.StartCoroutine(this.Jump());
    this.CombatComponent.ResetCombo();
  }

  public void Dodge(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
  {
    if (!this.Character.LocomotionData.IsGrounded || this.Character.CombatData.CombatType == ECombatType.Dodge || !ctx.performed)
      return;
    this.Character.CharacterAnim.SetFloat("Dodge_Direction", Vector3.SignedAngle(this.Character.transform.forward, this.Character.GetComponent<CharacterMovement>().GetDesiredMoveDirection, Vector3.up));
    switch (this.Character.LocomotionData.CharacterMoveMode)
    {
      case eCharacterMoveMode.Directional:
        this.Character.CharacterAnim.CrossFadeInFixedTime("Dodge_F", 0.1f);
        break;
      case eCharacterMoveMode.Strafe:
        this.Character.CharacterAnim.CrossFadeInFixedTime("Dodge_BT", 0.1f);
        break;
    }
    this.CombatComponent.ResetCombo();
  }

  private void LightAttack(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
  {
    if (!this.Character.LocomotionData.IsGrounded || this.Character.CombatData.CombatType == ECombatType.Dodge || !ctx.performed)
      return;
    this.CombatComponent.SetComboInput(EKeystroke.LightAttack);
  }

  private void StrongAttack(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
  {
    if (!this.Character.LocomotionData.IsGrounded || this.Character.CombatData.CombatType == ECombatType.Dodge || !ctx.performed)
      return;
    this.CombatComponent.SetComboInput(EKeystroke.StrongAttack);
  }

  private void HoldAttack(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
  {
    if (ctx.performed && this.CombatComponent.ComboState == EComboState.Stop)
    {
      this.Character.CharacterAnim.SetBool("IsHold", true);
      this.Character.CharacterAnim.CrossFadeInFixedTime("Hold Attack", 0.1f);
    }
    else
    {
      if (!ctx.canceled)
        return;
      this.Character.CharacterAnim.SetBool("IsHold", false);
    }
  }

  private void SlowMotion(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
  {
    if (ctx.performed)
      Time.timeScale = 0.1f;
    else
      Time.timeScale = 1f;
  }
}
