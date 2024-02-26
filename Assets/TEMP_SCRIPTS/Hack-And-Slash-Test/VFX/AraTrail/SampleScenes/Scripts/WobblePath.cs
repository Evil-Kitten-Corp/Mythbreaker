// Decompiled with JetBrains decompiler
// Type: WobblePath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WobblePath : MonoBehaviour
{
  public float speed = 10f;
  public float amplitude = 1f;
  public Vector3 offset;

  private void Update()
  {
    this.transform.localPosition = this.offset + this.transform.up * Mathf.Sin(Time.time * this.speed) * this.amplitude;
  }
}
