// Decompiled with JetBrains decompiler
// Type: BakeTrail
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using Ara;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (AraTrail))]
public class BakeTrail : MonoBehaviour
{
  private AraTrail trail;

  private void Awake() => this.trail = this.GetComponent<AraTrail>();

  private void Update()
  {
    if (!Input.GetKeyDown(KeyCode.Space))
      return;
    this.Bake();
  }

  private void Bake()
  {
    MeshFilter meshFilter = this.gameObject.AddComponent<MeshFilter>();
    MeshRenderer meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
    if ((Object) meshFilter != (Object) null && (Object) meshRenderer != (Object) null)
    {
      meshFilter.mesh = Object.Instantiate<Mesh>(this.trail.mesh);
      meshRenderer.materials = this.trail.materials;
      Object.Destroy((Object) this);
      Object.Destroy((Object) this.trail);
    }
    else
      Debug.LogError((object) "[BakeTrail]: Could not bake the trail because the object already had a MeshRenderer.");
  }
}
