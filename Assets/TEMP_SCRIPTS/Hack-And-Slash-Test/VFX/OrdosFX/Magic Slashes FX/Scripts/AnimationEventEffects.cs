// Decompiled with JetBrains decompiler
// Type: AnimationEventEffects
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class AnimationEventEffects : MonoBehaviour
{
  public AnimationEventEffects.EffectInfo[] Effects;

  private void Start()
  {
  }

  private void InstantiateEffect(int EffectNumber)
  {
    if (this.Effects == null || this.Effects.Length <= EffectNumber)
      Debug.LogError((object) "Incorrect effect number or effect is null");
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Effects[EffectNumber].Effect, this.Effects[EffectNumber].StartPositionRotation.position, this.Effects[EffectNumber].StartPositionRotation.rotation);
    if (this.Effects[EffectNumber].UseLocalPosition)
    {
      gameObject.transform.parent = this.Effects[EffectNumber].StartPositionRotation.transform;
      gameObject.transform.localPosition = Vector3.zero;
      gameObject.transform.localRotation = new Quaternion();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, this.Effects[EffectNumber].DestroyAfter);
  }

  [Serializable]
  public class EffectInfo
  {
    public GameObject Effect;
    public Transform StartPositionRotation;
    public float DestroyAfter = 10f;
    public bool UseLocalPosition = true;
  }
}
