// Decompiled with JetBrains decompiler
// Type: Balrond3PersonMovements.Balrond3pCameraFollow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Balrond3PersonMovements
{
  public class Balrond3pCameraFollow : MonoBehaviour
  {
    [Header("Target to follow")]
    public Transform target;
    [Header("Target's height")]
    public float setTargetHeight;
    [Header("Distance")]
    public float maxDistance = 2f;
    public float minDistance = 1f;
    [Header("Zoom speed")]
    public float smooth = 10f;

    private void Start() => this.transform.position = this.target.position;

    private void Update()
    {
      this.transform.position = this.target.position;
      this.transform.position += new Vector3(0.0f, this.setTargetHeight, 0.0f);
    }
  }
}
