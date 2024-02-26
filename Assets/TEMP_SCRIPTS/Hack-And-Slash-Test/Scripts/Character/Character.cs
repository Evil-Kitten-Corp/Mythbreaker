// Decompiled with JetBrains decompiler
// Type: AttachSocket
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public struct AttachSocket
{
  [Header("[Weapon Socket]")]
  public EAttachSocket attachSocket;
  public Transform socketTransform;

  public void AttachToObject(GameObject obj)
  {
    obj.transform.SetParent(this.socketTransform);
    obj.transform.localPosition = Vector3.zero;
    obj.transform.localRotation = Quaternion.identity;
  }
}
