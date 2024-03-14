using UnityEngine;

[RequireComponent(typeof(ReflectionProbe))]
public class ME_RealtimeReflection : MonoBehaviour
{
	private ReflectionProbe probe;

	private Transform camT;

	public bool CanUpdate = true;

	private void Awake()
	{
		probe = GetComponent<ReflectionProbe>();
		camT = Camera.main.transform;
	}

	private void Update()
	{
		Vector3 pos = camT.position;
		probe.transform.position = new Vector3(pos.x, pos.y * -1f, pos.z);
		if (CanUpdate)
		{
			probe.RenderProbe();
		}
	}
}
