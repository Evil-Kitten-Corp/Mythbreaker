// Decompiled with JetBrains decompiler
// Type: SetCombatType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class SetCombatType : StateMachineBehaviour
{
  private Character character;
  [SerializeField]
  private ECombatType enterCombatType;
  [SerializeField]
  private ECombatType exitCombatType;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.character = animator.GetComponent<Character>();
    this.character.CombatData.CombatType = this.enterCombatType;
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.character.CombatData.CombatType = this.exitCombatType;
  }
}
