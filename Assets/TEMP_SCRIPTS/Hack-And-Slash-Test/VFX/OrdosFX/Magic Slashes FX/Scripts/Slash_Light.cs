// Decompiled with JetBrains decompiler
// Type: Slash_Light
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Slash_Light : MonoBehaviour
{
  public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1f, 1f);
  public Gradient LightColor = new Gradient();
  public float GraphTimeMultiplier = 1f;
  public float GraphIntensityMultiplier = 1f;
  public bool IsLoop;
  [HideInInspector]
  public bool canUpdate;
  private float startTime;
  private Color startColor;
  private Light lightSource;

  public void SetStartColor(Color color) => this.startColor = color;

  private void Awake()
  {
    this.lightSource = this.GetComponent<Light>();
    this.startColor = this.lightSource.color;
    this.lightSource.intensity = this.LightCurve.Evaluate(0.0f) * this.GraphIntensityMultiplier;
    this.lightSource.color = this.startColor * this.LightColor.Evaluate(0.0f);
    this.startTime = Time.time;
    this.canUpdate = true;
  }

  private void OnEnable()
  {
    this.startTime = Time.time;
    this.canUpdate = true;
    if (!((Object) this.lightSource != (Object) null))
      return;
    this.lightSource.intensity = this.LightCurve.Evaluate(0.0f) * this.GraphIntensityMultiplier;
    this.lightSource.color = this.startColor * this.LightColor.Evaluate(0.0f);
  }

  private void Update()
  {
    float num = Time.time - this.startTime;
    if (this.canUpdate)
    {
      this.lightSource.intensity = this.LightCurve.Evaluate(num / this.GraphTimeMultiplier) * this.GraphIntensityMultiplier;
      this.lightSource.color = this.startColor * this.LightColor.Evaluate(num / this.GraphTimeMultiplier);
    }
    if ((double) num < (double) this.GraphTimeMultiplier)
      return;
    if (this.IsLoop)
      this.startTime = Time.time;
    else
      this.canUpdate = false;
  }
}
