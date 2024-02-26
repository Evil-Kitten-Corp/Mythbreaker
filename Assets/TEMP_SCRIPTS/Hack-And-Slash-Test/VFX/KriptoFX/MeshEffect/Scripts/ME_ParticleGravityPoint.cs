// Decompiled with JetBrains decompiler
// Type: ME_ParticleGravityPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ME_ParticleGravityPoint : MonoBehaviour
{
  public Transform target;
  public float Force = 1f;
  public bool DistanceRelative;
  private ParticleSystem ps;
  private ParticleSystem.Particle[] particles;
  private ParticleSystem.MainModule mainModule;
  private Vector3 prevPos;

  private void Start()
  {
    this.ps = this.GetComponent<ParticleSystem>();
    this.mainModule = this.ps.main;
  }

  private void LateUpdate()
  {
    int maxParticles = this.mainModule.maxParticles;
    if (this.particles == null || this.particles.Length < maxParticles)
      this.particles = new ParticleSystem.Particle[maxParticles];
    int particles = this.ps.GetParticles(this.particles);
    Vector3 vector3_1 = Vector3.zero;
    if (this.mainModule.simulationSpace == ParticleSystemSimulationSpace.Local)
      vector3_1 = this.transform.InverseTransformPoint(this.target.position);
    if (this.mainModule.simulationSpace == ParticleSystemSimulationSpace.World)
      vector3_1 = this.target.position;
    float num = Time.deltaTime * this.Force;
    if (this.DistanceRelative)
      num *= Mathf.Abs((this.prevPos - vector3_1).magnitude);
    for (int index = 0; index < particles; ++index)
    {
      Vector3 vector3_2 = Vector3.Normalize(vector3_1 - this.particles[index].position);
      if (this.DistanceRelative)
        vector3_2 = Vector3.Normalize(vector3_1 - this.prevPos);
      Vector3 vector3_3 = vector3_2 * num;
      this.particles[index].velocity += vector3_3;
    }
    this.ps.SetParticles(this.particles, particles);
    this.prevPos = vector3_1;
  }
}
