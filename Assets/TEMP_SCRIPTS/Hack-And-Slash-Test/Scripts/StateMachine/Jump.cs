// Decompiled with JetBrains decompiler
// Type: Jump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using DG.Tweening;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class Jump : StateMachineBehaviour
{
  private Character Character;
  public AnimationCurve JumpCurve;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.Character = animator.GetComponent<Character>();
    this.Character.LocomotionData.IsJump = true;
    this.Character.LocomotionData.IsGrounded = false;
    this.Character.CharacterAnim.SetBool("IsGrounded", false);
    this.Character.LocomotionData.MovementState = eMovementState.InAir;
    this.Character.FallingEvent();
  }

  public override void OnStateUpdate(
    Animator animator,
    AnimatorStateInfo stateInfo,
    int layerIndex)
  {
    this.Character.AirControl(this.JumpCurve.Evaluate(stateInfo.normalizedTime) * this.Character.LocomotionData.JumpForce);
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.Character.LocomotionData.IsJump = false;
    this.Character.GetComponent<CharacterMovement>().AirMoveDirection = Vector3.zero;
    this.Character.transform.DORotate(new Vector3(0.0f, this.Character.transform.eulerAngles.y, 0.0f), 0.2f);
  }
}
