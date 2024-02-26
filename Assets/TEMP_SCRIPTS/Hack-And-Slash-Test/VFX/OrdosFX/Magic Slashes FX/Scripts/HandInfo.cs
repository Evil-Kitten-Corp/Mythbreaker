// Decompiled with JetBrains decompiler
// Type: HandInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct HandInfo
{
  [Header("[Hand Info]")]
  public Transform HandTransform;
  public Transform HintTransform;
  public Vector3 OffsetPosition;
  public Vector3 OffsetNormal;
  [Range(0.0f, 1f)]
  public float Weight;
}
