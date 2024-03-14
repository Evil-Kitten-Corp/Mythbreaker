using UnityEngine;

[ExecuteInEditMode] [RequireComponent(typeof(ParticleSystem))]
public class Slash_GrvityPoint : MonoBehaviour
{
	public Transform Target;

	public float Force = 1f;

	public float StopDistance;

	private ParticleSystem ps;

	private ParticleSystem.Particle[] particles;

	private ParticleSystem.MainModule mainModule;

	private void Start()
	{
		ps = GetComponent<ParticleSystem>();
		mainModule = ps.main;
	}

	private void LateUpdate()
	{
		if (Target == null)
		{
			return;
		}
		int maxParticles = mainModule.maxParticles;
		if (particles == null || particles.Length < maxParticles)
		{
			particles = new ParticleSystem.Particle[maxParticles];
		}
		int particleCount = ps.GetParticles(particles);
		Vector3 targetTransformedPosition = Vector3.zero;
		if (mainModule.simulationSpace == ParticleSystemSimulationSpace.Local)
		{
			targetTransformedPosition = base.transform.InverseTransformPoint(Target.position);
		}
		if (mainModule.simulationSpace == ParticleSystemSimulationSpace.World)
		{
			targetTransformedPosition = Target.position;
		}
		float forceDeltaTime = Time.deltaTime * Force;
		for (int i = 0; i < particleCount; i++)
		{
			Vector3 distanceToParticle = targetTransformedPosition - particles[i].position;
			if (StopDistance > 0.001f && distanceToParticle.magnitude < StopDistance)
			{
				particles[i].velocity = Vector3.zero;
				continue;
			}
			Vector3 seekForce = Vector3.Normalize(distanceToParticle) * forceDeltaTime;
			particles[i].velocity += seekForce;
		}
		ps.SetParticles(particles, particleCount);
	}
}
