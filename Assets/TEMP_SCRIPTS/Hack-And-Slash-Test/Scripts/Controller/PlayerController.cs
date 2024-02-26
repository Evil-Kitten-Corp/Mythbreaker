// Decompiled with JetBrains decompiler
// Type: PlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
public class PlayerController : MonoBehaviour
{
  [Header("[Component]")]
  private Character Character;
  private CombatComponent CombatComponent;
  [Header("[Input System]")]
  private ComboActions ComboActions;

  public ComboActions GetComboActions => this.ComboActions;

  private void Awake() => this.Initialize();

  private void OnEnable()
  {
    this.ComboActions.Locomotion.Enable();
    this.ComboActions.Combat.Enable();
  }

  private void OnDisable()
  {
    this.ComboActions.Locomotion.Disable();
    this.ComboActions.Combat.Disable();
  }

  private void Initialize()
  {
    this.Character = this.GetComponent<Character>();
    this.CombatComponent = this.GetComponent<CombatComponent>();
    this.ComboActions = new ComboActions();
    this.InputBinding();
  }

  private void InputBinding()
  {
    this.ComboActions.Locomotion.Sprint.performed += new Action<InputAction.CallbackContext>(this.Sprint);
    this.ComboActions.Locomotion.Jump.performed += new Action<InputAction.CallbackContext>(this.Jump);
    this.ComboActions.Combat.LightAttack.performed += new Action<InputAction.CallbackContext>(this.LightAttack);
    this.ComboActions.Combat.StrongAttack.performed += new Action<InputAction.CallbackContext>(this.StrongAttack);
    this.ComboActions.Combat.HoldAttack.performed += new Action<InputAction.CallbackContext>(this.HoldAttack);
    this.ComboActions.Combat.Dodge.performed += new Action<InputAction.CallbackContext>(this.Dodge);
    this.ComboActions.Locomotion.Sprint.canceled += new Action<InputAction.CallbackContext>(this.Sprint);
    this.ComboActions.Combat.HoldAttack.canceled += new Action<InputAction.CallbackContext>(this.HoldAttack);
  }

  private void Sprint(InputAction.CallbackContext ctx)
  {
  }

  private void Jump(InputAction.CallbackContext ctx)
  {
    if (!ctx.performed)
      return;
    if (this.Character.LocomotionData.IsGrounded)
    {
      this.Character.CharacterAnim.CrossFadeInFixedTime(nameof (Jump), 0.1f);
      ++this.Character.CharacterOptional.jumpCount;
    }
    else if (this.Character.CharacterOptional.isDoubleJump && this.Character.CharacterOptional.jumpCount == 1)
    {
      this.Character.CharacterAnim.CrossFadeInFixedTime("Double Jump", 0.1f);
      ++this.Character.CharacterOptional.jumpCount;
    }
    this.CombatComponent.ResetCombo();
  }

  private void LightAttack(InputAction.CallbackContext ctx)
  {
    if (!ctx.performed)
      return;
    this.CombatComponent.SetComboInput(EKeystroke.LightAttack);
  }

  private void StrongAttack(InputAction.CallbackContext ctx)
  {
    if (!ctx.performed)
      return;
    this.CombatComponent.SetComboInput(EKeystroke.StrongAttack);
  }

  private void HoldAttack(InputAction.CallbackContext ctx)
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

  private void Dodge(InputAction.CallbackContext ctx)
  {
    if (!ctx.performed || this.Character.CombatData.CombatType == ECombatType.Dodge)
      return;
    this.Character.CharacterAnim.CrossFadeInFixedTime(nameof (Dodge), 0.1f);
    this.CombatComponent.ResetCombo();
  }
}
