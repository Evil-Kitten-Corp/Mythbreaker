// Decompiled with JetBrains decompiler
// Type: SpawnEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class SpawnEffect : StateMachineBehaviour
{
  public GameObject effectPrefab;
  public int spawnCount = 1;
  public float destroyTime = 1f;
  public float startDelayTime;
  public float intervalTime = 0.2f;
  public List<Vector3> spawnPositions;
  public List<Vector3> spawnEulers;

  public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
  {
    if ((Object) this.effectPrefab == (Object) null)
    {
      Debug.LogError((object) "Prefab is null.");
    }
    else
    {
      if (!((Object) animator.GetComponent<Character>() != (Object) null))
        return;
      Character component = animator.GetComponent<Character>();
      component.StartCoroutine(this.Spawn(component));
    }
  }

  private IEnumerator Spawn(Character spawner)
  {
    yield return (object) new WaitForSeconds(this.startDelayTime);
    for (int i = 0; i < this.spawnCount; ++i)
    {
      Object.Destroy((Object) Object.Instantiate<GameObject>(this.effectPrefab, spawner.transform.position + spawner.transform.TransformDirection(this.spawnPositions[i]), Quaternion.LookRotation(spawner.transform.forward) * Quaternion.Euler(this.spawnEulers[i])), this.destroyTime);
      yield return (object) new WaitForSeconds(this.intervalTime);
    }
  }
}
