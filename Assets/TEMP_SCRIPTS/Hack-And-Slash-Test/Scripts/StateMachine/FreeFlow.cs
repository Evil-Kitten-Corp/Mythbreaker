// Decompiled with JetBrains decompiler
// Type: FreeFlow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class FreeFlow : StateMachineBehaviour
{
  [SerializeField]
  private Character owner;
  [SerializeField]
  public EFlowType flowType;
  public bool isUpdated = true;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    this.owner = animator.gameObject.GetComponent<Character>();
    if ((bool) (Object) this.owner.GetComponent<FreeFlowComponent>())
    {
      if (!this.isUpdated)
        this.owner.GetComponent<FreeFlowComponent>().OnFreeFlow();
      else
        this.owner.GetComponent<FreeFlowComponent>().UpdateTarget();
    }
    else
      Debug.LogError((object) "Free Flow Component is null.");
  }

  public override void OnStateUpdate(
    Animator animator,
    AnimatorStateInfo stateInfo,
    int layerIndex)
  {
    if ((bool) (Object) this.owner.GetComponent<FreeFlowComponent>() && this.isUpdated)
      this.owner.GetComponent<FreeFlowComponent>().UpdateFreeFlow(this.flowType);
    else
      Debug.LogError((object) "Free Flow Component is null.");
  }

  public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if (!(bool) (Object) this.owner.GetComponent<FreeFlowComponent>() || !this.isUpdated)
      return;
    this.owner.GetComponent<FreeFlowComponent>().OnReset();
  }
}
