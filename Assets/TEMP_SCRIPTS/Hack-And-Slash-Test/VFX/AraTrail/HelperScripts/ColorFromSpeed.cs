// Decompiled with JetBrains decompiler
// Type: Ara.ColorFromSpeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Ara
{
  [RequireComponent(typeof (AraTrail))]
  public class ColorFromSpeed : MonoBehaviour
  {
    private AraTrail trail;
    [Tooltip("Maps trail speed to color. Control how much speed is transferred to the trail by setting inertia > 0. The trail will be colorized even if physics are disabled. ")]
    public Gradient colorFromSpeed = new Gradient();
    [Tooltip("Min speed used to map speed to color.")]
    public float minSpeed;
    [Tooltip("Max speed used to map speed to color.")]
    public float maxSpeed = 5f;

    private void OnEnable()
    {
      this.trail = this.GetComponent<AraTrail>();
      this.trail.onUpdatePoints += new Action(this.SetColorFromSpeed);
    }

    private void OnDisable() => this.trail.onUpdatePoints -= new Action(this.SetColorFromSpeed);

    private void SetColorFromSpeed()
    {
      float num = Mathf.Max(1E-05f, this.maxSpeed - this.minSpeed);
      for (int index = 0; index < this.trail.points.Count; ++index)
      {
        AraTrail.Point point = this.trail.points[index];
        point.color = this.colorFromSpeed.Evaluate((point.velocity.magnitude - this.minSpeed) / num);
        this.trail.points[index] = point;
      }
    }
  }
}
