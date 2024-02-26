// Decompiled with JetBrains decompiler
// Type: CharacterState
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct CharacterState
{
  [Header("[Character State]")]
  public LayerMask groundLayer;
  public float gravity;
  public float jumpForce;
  public float airForce;
  public float duration;
  public float rotationRate;
  public float ratio;
  public float angle;
  public bool isGrounded;
  public bool isJump;
  public bool lockedRotation;
}
