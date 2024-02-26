// Decompiled with JetBrains decompiler
// Type: Ara.TireTrack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Ara
{
  [RequireComponent(typeof (AraTrail))]
  public class TireTrack : MonoBehaviour
  {
    private AraTrail trail;
    public float offset = 0.05f;
    public float maxDist = 0.1f;

    private void OnEnable()
    {
      this.trail = this.GetComponent<AraTrail>();
      this.trail.onUpdatePoints += new Action(this.ProjectToGround);
    }

    private void OnDisable() => this.trail.onUpdatePoints -= new Action(this.ProjectToGround);

    private void ProjectToGround()
    {
      RaycastHit hitInfo;
      if (Physics.Raycast(new Ray(this.transform.position, -Vector3.up), out hitInfo, this.maxDist))
      {
        if (this.trail.emit && this.trail.points.Count > 0)
        {
          AraTrail.Point point = this.trail.points[this.trail.points.Count - 1];
          if (!point.discontinuous)
          {
            point.normal = hitInfo.normal;
            point.position = hitInfo.point + hitInfo.normal * this.offset;
            this.trail.points[this.trail.points.Count - 1] = point;
          }
        }
        this.trail.emit = true;
      }
      else
      {
        if (!this.trail.emit)
          return;
        this.trail.emit = false;
        if (this.trail.points.Count <= 0)
          return;
        this.trail.points.RemoveAt(this.trail.points.Count - 1);
      }
    }
  }
}
