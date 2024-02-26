// Decompiled with JetBrains decompiler
// Type: ME_ParticleTrails
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable disable
public class ME_ParticleTrails : MonoBehaviour
{
  public GameObject TrailPrefab;
  private ParticleSystem ps;
  private ParticleSystem.Particle[] particles;
  private Dictionary<uint, GameObject> hashTrails = new Dictionary<uint, GameObject>();
  private Dictionary<uint, GameObject> newHashTrails = new Dictionary<uint, GameObject>();
  private List<GameObject> currentGO = new List<GameObject>();

  private void Start()
  {
    this.ps = this.GetComponent<ParticleSystem>();
    this.particles = new ParticleSystem.Particle[this.ps.main.maxParticles];
  }

  private void OnEnable() => this.InvokeRepeating("ClearEmptyHashes", 1f, 1f);

  private void OnDisable()
  {
    this.Clear();
    this.CancelInvoke("ClearEmptyHashes");
  }

  public void Clear()
  {
    foreach (UnityEngine.Object @object in this.currentGO)
      UnityEngine.Object.Destroy(@object);
    this.currentGO.Clear();
  }

  private void Update() => this.UpdateTrail();

  private void UpdateTrail()
  {
    this.newHashTrails.Clear();
    int particles = this.ps.GetParticles(this.particles);
    for (int index = 0; index < particles; ++index)
    {
      if (!this.hashTrails.ContainsKey(this.particles[index].randomSeed))
      {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TrailPrefab, this.transform.position, new Quaternion());
        gameObject.transform.parent = this.transform;
        this.currentGO.Add(gameObject);
        this.newHashTrails.Add(this.particles[index].randomSeed, gameObject);
        gameObject.GetComponent<LineRenderer>().widthMultiplier *= this.particles[index].startSize;
      }
      else
      {
        GameObject hashTrail = this.hashTrails[this.particles[index].randomSeed];
        if ((UnityEngine.Object) hashTrail != (UnityEngine.Object) null)
        {
          LineRenderer component = hashTrail.GetComponent<LineRenderer>();
          component.startColor *= (Color) this.particles[index].GetCurrentColor(this.ps);
          component.endColor *= (Color) this.particles[index].GetCurrentColor(this.ps);
          ParticleSystem.MainModule main = this.ps.main;
          if (main.simulationSpace == ParticleSystemSimulationSpace.World)
            hashTrail.transform.position = this.particles[index].position;
          main = this.ps.main;
          if (main.simulationSpace == ParticleSystemSimulationSpace.Local)
            hashTrail.transform.position = this.ps.transform.TransformPoint(this.particles[index].position);
          this.newHashTrails.Add(this.particles[index].randomSeed, hashTrail);
        }
        this.hashTrails.Remove(this.particles[index].randomSeed);
      }
    }
    foreach (KeyValuePair<uint, GameObject> hashTrail in this.hashTrails)
    {
      if ((UnityEngine.Object) hashTrail.Value != (UnityEngine.Object) null)
        hashTrail.Value.GetComponent<ME_TrailRendererNoise>().IsActive = false;
    }
    this.AddRange<uint, GameObject>(this.hashTrails, this.newHashTrails);
  }

  public void AddRange<T, S>(Dictionary<T, S> source, Dictionary<T, S> collection)
  {
    if (collection == null)
      return;
    foreach (KeyValuePair<T, S> keyValuePair in collection)
    {
      if (!source.ContainsKey(keyValuePair.Key))
        source.Add(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private void ClearEmptyHashes()
  {
    this.hashTrails = this.hashTrails.Where<KeyValuePair<uint, GameObject>>((Func<KeyValuePair<uint, GameObject>, bool>) (h => (UnityEngine.Object) h.Value != (UnityEngine.Object) null)).ToDictionary<KeyValuePair<uint, GameObject>, uint, GameObject>((Func<KeyValuePair<uint, GameObject>, uint>) (h => h.Key), (Func<KeyValuePair<uint, GameObject>, GameObject>) (h => h.Value));
  }
}
