// Decompiled with JetBrains decompiler
// Type: Ara.ElectricalArc
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Ara
{
  [RequireComponent(typeof (AraTrail))]
  public class ElectricalArc : MonoBehaviour
  {
    private AraTrail trail;
    public Transform source;
    public Transform target;
    public int points = 20;
    public float burstInterval = 0.5f;
    public float burstRandom = 0.2f;
    public float speedRandom = 2f;
    public float positionRandom = 0.1f;
    private float accum;

    private void OnEnable()
    {
      this.trail = this.GetComponent<AraTrail>();
      this.trail.emit = false;
    }

    private void Update()
    {
      this.accum += Time.deltaTime;
      if ((double) this.accum < (double) this.burstInterval)
        return;
      this.ChangeArc();
      this.accum = -this.burstInterval * Random.value * this.burstRandom;
    }

    private void ChangeArc()
    {
      this.trail.points.Clear();
      if (!((Object) this.source != (Object) null) || !((Object) this.target != (Object) null))
        return;
      for (int index = 0; index < this.points; ++index)
      {
        float t = (float) index / (float) (this.points - 1);
        float num = Mathf.Sin(t * 3.14159274f);
        this.trail.points.Add(new AraTrail.Point(Vector3.Lerp(this.source.position, this.target.position, t) + Random.onUnitSphere * this.positionRandom * num, Random.onUnitSphere * this.speedRandom * num, Vector3.up, Vector3.forward, Color.white, 1f, 0.0f, this.burstInterval * 2f));
      }
    }
  }
}
