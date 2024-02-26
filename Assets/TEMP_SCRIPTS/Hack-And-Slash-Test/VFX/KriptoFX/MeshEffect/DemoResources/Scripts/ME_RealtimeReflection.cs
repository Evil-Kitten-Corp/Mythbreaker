// Decompiled with JetBrains decompiler
// Type: ME_RealtimeReflection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ME_RealtimeReflection : MonoBehaviour
{
  private ReflectionProbe probe;
  private Transform camT;
  public bool CanUpdate = true;

  private void Awake()
  {
    this.probe = this.GetComponent<ReflectionProbe>();
    this.camT = Camera.main.transform;
  }

  private void Update()
  {
    Vector3 position = this.camT.position;
    this.probe.transform.position = new Vector3(position.x, position.y * -1f, position.z);
    if (!this.CanUpdate)
      return;
    this.probe.RenderProbe();
  }
}
