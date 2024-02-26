// Decompiled with JetBrains decompiler
// Type: ComboData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public struct ComboData
{
  [Header("[Combo Data]")]
  public string rowName;
  public string comboName;
  public List<EKeystroke> comboInputs;
  public List<AnimationClip> comboClips;
}
