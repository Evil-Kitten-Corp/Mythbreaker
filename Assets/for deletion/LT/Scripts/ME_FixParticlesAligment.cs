using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ME_FixParticlesAligment : MonoBehaviour
{
	private void Start()
	{
		GetComponent<ParticleSystem>().GetComponent<ParticleSystemRenderer>().alignment = ParticleSystemRenderSpace.World;
	}
}
