// Decompiled with JetBrains decompiler
// Type: Slash_AxisRotateByTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class Slash_AxisRotateByTime : MonoBehaviour
{
  public Vector3 RotateAxis = new Vector3(0.0f, 0.0f, 0.0f);

  private void Start()
  {
  }

  private void Update() => this.transform.Rotate(this.RotateAxis * Time.deltaTime);
}
