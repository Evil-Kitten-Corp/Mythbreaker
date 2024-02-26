// Decompiled with JetBrains decompiler
// Type: Slash_WroomWroom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Slash_WroomWroom : MonoBehaviour
{
  public float Speed = 1f;
  public GameObject Target;

  private void OnEnable() => this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

  private void Update()
  {
    this.transform.position = Vector3.MoveTowards(this.transform.position, this.Target.transform.position, this.Speed * Time.deltaTime);
  }
}
