// Decompiled with JetBrains decompiler
// Type: TimeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TimeManager : MonoSingleton<TimeManager>
{
  [Header("[Time Manager]")]
  public float SlowMotionTime;
  public bool IsSlowMotion;

  private void Update() => this.SlowMotionTimer();

  private void SlowMotionTimer()
  {
    if (!this.IsSlowMotion || (double) this.SlowMotionTime <= 0.0)
      return;
    this.SlowMotionTime -= Time.unscaledDeltaTime;
    if ((double) this.SlowMotionTime > 0.0)
      return;
    this.OffSlowMotion();
  }

  public void OnSlowMotion(float timeScale, float timer = 0.0f)
  {
    Time.timeScale = timeScale;
    Time.fixedDeltaTime = 0.02f * Time.timeScale;
    this.IsSlowMotion = true;
    this.SlowMotionTime = timer;
  }

  public void OffSlowMotion()
  {
    Time.timeScale = 1f;
    Time.fixedDeltaTime = 0.02f * Time.timeScale;
    this.IsSlowMotion = false;
  }
}
