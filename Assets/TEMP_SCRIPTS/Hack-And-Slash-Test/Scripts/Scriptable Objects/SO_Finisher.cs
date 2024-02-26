// Decompiled with JetBrains decompiler
// Type: FinisherData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
[Serializable]
public struct FinisherData
{
  [Header("[Finisher Data]")]
  [Tooltip("Attacker is Clip")]
  public AnimationClip executionClip;
  [Tooltip("Victim is Clip")]
  public AnimationClip executedClip;

  public IEnumerator PlayFinisher(Character attacker, Character victim, float duration)
  {
    float currentTime = 0f;
    Vector3 direction = (victim.transform.position - attacker.transform.position).normalized;
    direction.y = 0f;
    attacker.GetComponent<Animator>().CrossFadeInFixedTime(executionClip.name, 0.1f);
    victim.GetComponent<Animator>().CrossFadeInFixedTime(executedClip.name, 0.1f);
    while (currentTime < duration)
    {
      currentTime += Time.deltaTime;
      attacker.transform.position = Vector3.Lerp(attacker.transform.position, victim.transform.position + -direction, currentTime / duration);
      attacker.transform.rotation = Quaternion.Slerp(attacker.transform.rotation, Quaternion.LookRotation(direction), currentTime / (duration * 0.5f));
      victim.transform.rotation = Quaternion.Slerp(victim.transform.rotation, Quaternion.LookRotation(-direction), currentTime / (duration * 0.5f));
      yield return null;
    }
  }
}
