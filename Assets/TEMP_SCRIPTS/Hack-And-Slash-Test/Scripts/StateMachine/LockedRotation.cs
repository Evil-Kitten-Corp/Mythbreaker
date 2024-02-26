// Decompiled with JetBrains decompiler
// Type: LockedRotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class LockedRotation : StateMachineBehaviour
{
  private Character owner;

  public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
  {
    this.owner = animator.GetComponent<Character>();
    this.owner.LocomotionData.LockedRotation = true;
  }

  public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
  {
    this.owner.LocomotionData.LockedRotation = false;
  }
}
