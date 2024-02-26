// Decompiled with JetBrains decompiler
// Type: ME_LightCurves
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class ME_LightCurves : MonoBehaviour
{
  public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  public float GraphTimeMultiplier = 1f;
  public float GraphIntensityMultiplier = 1f;
  public bool IsLoop;
  private bool canUpdate;
  private float startTime;
  private Light lightSource;

  private void Awake()
  {
    this.lightSource = this.GetComponent<Light>();
    this.lightSource.intensity = this.LightCurve.Evaluate(0.0f);
  }

  private void OnEnable()
  {
    this.startTime = Time.time;
    this.canUpdate = true;
  }

  private void Update()
  {
    float num = Time.time - this.startTime;
    if (this.canUpdate)
      this.lightSource.intensity = this.LightCurve.Evaluate(num / this.GraphTimeMultiplier) * this.GraphIntensityMultiplier;
    if ((double) num < (double) this.GraphTimeMultiplier)
      return;
    if (this.IsLoop)
      this.startTime = Time.time;
    else
      this.canUpdate = false;
  }
}
