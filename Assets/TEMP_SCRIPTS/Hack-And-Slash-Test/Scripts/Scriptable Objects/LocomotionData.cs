// Decompiled with JetBrains decompiler
// Type: LeanAmount
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct LeanAmount
{
  public float LR;
  public float FB;

  public LeanAmount(float _LR, float _FB)
    : this()
  {
    this.LR = _LR;
    this.FB = _FB;
  }

  public void TweenLeanAmount(LeanAmount current, LeanAmount target, float duration)
  {
    DOTween.To((DOGetter<float>) (() => current.LR), (DOSetter<float>) (x => current.LR = x), target.LR, duration);
    DOTween.To((DOGetter<float>) (() => current.FB), (DOSetter<float>) (x => current.FB = x), target.FB, duration);
    Debug.LogError((object) new Vector2(current.LR, current.FB));
  }
}
