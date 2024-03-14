using UnityEngine;

[ExecuteInEditMode] [RequireComponent(typeof(ParticleSystem))]
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
		ps = GetComponent<ParticleSystem>();
		mainModule = ps.main;
	}

	private void LateUpdate()
	{
		int maxParticles = mainModule.maxParticles;
		if (particles == null || particles.Length < maxParticles)
		{
			particles = new ParticleSystem.Particle[maxParticles];
		}
		int particleCount = ps.GetParticles(particles);
		Vector3 targetTransformedPosition = Vector3.zero;
		if (mainModule.simulationSpace == ParticleSystemSimulationSpace.Local)
		{
			targetTransformedPosition = base.transform.InverseTransformPoint(target.position);
		}
		if (mainModule.simulationSpace == ParticleSystemSimulationSpace.World)
		{
			targetTransformedPosition = target.position;
		}
		float forceDeltaTime = Time.deltaTime * Force;
		if (DistanceRelative)
		{
			forceDeltaTime *= Mathf.Abs((prevPos - targetTransformedPosition).magnitude);
		}
		for (int i = 0; i < particleCount; i++)
		{
			Vector3 directionToTarget = Vector3.Normalize(targetTransformedPosition - particles[i].position);
			if (DistanceRelative)
			{
				directionToTarget = Vector3.Normalize(targetTransformedPosition - prevPos);
			}
			Vector3 seekForce = directionToTarget * forceDeltaTime;
			particles[i].velocity += seekForce;
		}
		ps.SetParticles(particles, particleCount);
		prevPos = targetTransformedPosition;
	}
}
