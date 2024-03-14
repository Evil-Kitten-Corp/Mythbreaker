using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] [RequireComponent(typeof(ParticleSystem))]
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
		collisionEvents.Clear();
		collidedGameObjects.Clear();
		initiatorPS = GetComponent<ParticleSystem>();
		particles = new ParticleSystem.Particle[DecalParticles.main.maxParticles];
		if (InstantiateWhenZeroSpeed)
		{
			InvokeRepeating("CollisionDetect", 0f, 0.1f);
		}
	}

	private void OnDisable()
	{
		if (InstantiateWhenZeroSpeed)
		{
			CancelInvoke("CollisionDetect");
		}
	}

	private void CollisionDetect()
	{
		int aliveParticles = 0;
		if (InstantiateWhenZeroSpeed)
		{
			aliveParticles = DecalParticles.GetParticles(particles);
		}
		foreach (GameObject collidedGameObject in collidedGameObjects)
		{
			OnParticleCollisionManual(collidedGameObject, aliveParticles);
		}
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
		if (InstantiateWhenZeroSpeed)
		{
			if (!collidedGameObjects.Contains(other))
			{
				collidedGameObjects.Add(other);
			}
		}
		else
		{
			OnParticleCollisionManual(other);
		}
	}
}
