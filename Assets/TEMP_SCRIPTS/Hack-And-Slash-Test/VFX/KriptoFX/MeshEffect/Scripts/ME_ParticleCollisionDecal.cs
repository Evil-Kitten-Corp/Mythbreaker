// Decompiled with JetBrains decompiler
// Type: ME_ParticleCollisionDecal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class ME_ParticleCollisionDecal : MonoBehaviour
{
  public ParticleSystem DecalParticles;
  public bool IsBilboard;
  public bool InstantiateWhenZeroSpeed;
  public float MaxGroundAngleDeviation = 45f;
  public float MinDistanceBetweenDecals = 0.1f;
  public float MinDistanceBetweenSurface = 0.03f;
  private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
  private ParticleSystem.Particle[] particles;
  private ParticleSystem initiatorPS;
  private List<GameObject> collidedGameObjects = new List<GameObject>();

  private void OnEnable()
  {
    this.collisionEvents.Clear();
    this.collidedGameObjects.Clear();
    this.initiatorPS = this.GetComponent<ParticleSystem>();
    this.particles = new ParticleSystem.Particle[this.DecalParticles.main.maxParticles];
    if (!this.InstantiateWhenZeroSpeed)
      return;
    this.InvokeRepeating("CollisionDetect", 0.0f, 0.1f);
  }

  private void OnDisable()
  {
    if (!this.InstantiateWhenZeroSpeed)
      return;
    this.CancelInvoke("CollisionDetect");
  }

  private void CollisionDetect()
  {
    int aliveParticles = 0;
    if (this.InstantiateWhenZeroSpeed)
      aliveParticles = this.DecalParticles.GetParticles(this.particles);
    foreach (GameObject collidedGameObject in this.collidedGameObjects)
      this.OnParticleCollisionManual(collidedGameObject, aliveParticles);
  }

  private void OnParticleCollisionManual(GameObject other, int aliveParticles = -1)
  {
    collisionEvents.Clear();
    int aliveEvents = initiatorPS.GetCollisionEvents(other, collisionEvents);
    for (int i = 0; i < aliveEvents; i++)
    {
      if (Vector3.Angle(collisionEvents[i].normal, Vector3.up) > MaxGroundAngleDeviation)
      {
        continue;
      }
      if (InstantiateWhenZeroSpeed)
      {
        if (collisionEvents[i].velocity.sqrMagnitude > 0.1f)
        {
          continue;
        }
        bool isNearDistance = false;
        for (int j = 0; j < aliveParticles; j++)
        {
          if (Vector3.Distance(collisionEvents[i].intersection, particles[j].position) < MinDistanceBetweenDecals)
          {
            isNearDistance = true;
          }
        }
        if (isNearDistance)
        {
          continue;
        }
      }
      ParticleSystem.EmitParams emiter = default(ParticleSystem.EmitParams);
      emiter.position = collisionEvents[i].intersection + collisionEvents[i].normal * MinDistanceBetweenSurface;
      Vector3 rotation = Quaternion.LookRotation(-collisionEvents[i].normal).eulerAngles;
      rotation.z = Random.Range(0, 360);
      emiter.rotation3D = rotation;
      DecalParticles.Emit(emiter, 1);
    }
  }

  private void OnParticleCollision(GameObject other)
  {
    if (this.InstantiateWhenZeroSpeed)
    {
      if (this.collidedGameObjects.Contains(other))
        return;
      this.collidedGameObjects.Add(other);
    }
    else
      this.OnParticleCollisionManual(other);
  }
}
