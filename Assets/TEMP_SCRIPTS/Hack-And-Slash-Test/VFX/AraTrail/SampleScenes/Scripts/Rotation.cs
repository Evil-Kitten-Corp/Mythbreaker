// Decompiled with JetBrains decompiler
// Type: AraSamples.Rotation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace AraSamples
{
  public class Rotation : MonoBehaviour
  {
    public float speed = 10f;
    public Vector3 axis;

    private void Update() => this.transform.Rotate(this.axis, this.speed * Time.deltaTime);
  }
}
