// Decompiled with JetBrains decompiler
// Type: Balrond3PersonMovements.Balrond3personCameraCollision
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Balrond3PersonMovements
{
  public class Balrond3personCameraCollision : MonoBehaviour
  {
    private Vector3 dollyDir;
    private Vector3 dollyDirAdjusted;
    private Balrond3pCameraFollow follow;
    private Balrond3pMainCamera cam;

    private void Awake()
    {
      this.follow = this.transform.parent.parent.GetComponent<Balrond3pCameraFollow>();
      this.cam = this.transform.parent.GetComponent<Balrond3pMainCamera>();
      this.dollyDir = this.transform.parent.localPosition;
    }

    private void FixedUpdate()
    {
      RaycastHit hitInfo;
      if (Physics.Linecast(this.transform.parent.localPosition, this.transform.parent.TransformPoint(this.dollyDir * this.follow.maxDistance), out hitInfo))
      {
        if ((double) this.transform.localPosition.z > (double) this.follow.minDistance || hitInfo.transform.name.Equals(this.follow.target.transform.gameObject.name) || hitInfo.transform.gameObject.name.Equals(this.transform.gameObject.name) || hitInfo.transform.gameObject.name.Equals(this.cam.transform.gameObject.name))
          return;
        this.transform.localPosition += new Vector3(0.0f, 0.0f, this.follow.smooth * Time.deltaTime);
      }
      else
      {
        if (-(double) this.follow.maxDistance >= (double) this.transform.localPosition.z)
          return;
        this.transform.localPosition -= new Vector3(0.0f, 0.0f, this.follow.smooth * Time.deltaTime);
      }
    }
  }
}
