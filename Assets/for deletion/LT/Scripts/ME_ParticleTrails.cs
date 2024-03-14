using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
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
		ps = GetComponent<ParticleSystem>();
		particles = new ParticleSystem.Particle[ps.main.maxParticles];
	}

	private void OnEnable()
	{
		InvokeRepeating("ClearEmptyHashes", 1f, 1f);
	}

	private void OnDisable()
	{
		Clear();
		CancelInvoke("ClearEmptyHashes");
	}

	public void Clear()
	{
		foreach (GameObject item in currentGO)
		{
			Object.Destroy(item);
		}
		currentGO.Clear();
	}

	private void Update()
	{
		UpdateTrail();
	}

	private void UpdateTrail()
	{
		newHashTrails.Clear();
		int count = ps.GetParticles(particles);
		for (int i = 0; i < count; i++)
		{
			if (!hashTrails.ContainsKey(particles[i].randomSeed))
			{
				GameObject go2 = Object.Instantiate(TrailPrefab, base.transform.position, default(Quaternion));
				go2.transform.parent = base.transform;
				currentGO.Add(go2);
				newHashTrails.Add(particles[i].randomSeed, go2);
				LineRenderer component = go2.GetComponent<LineRenderer>();
				component.widthMultiplier *= particles[i].startSize;
				continue;
			}
			GameObject go = hashTrails[particles[i].randomSeed];
			if (go != null)
			{
				LineRenderer component2 = go.GetComponent<LineRenderer>();
				component2.startColor *= particles[i].GetCurrentColor(ps);
				component2.endColor *= particles[i].GetCurrentColor(ps);
				if (ps.main.simulationSpace == ParticleSystemSimulationSpace.World)
				{
					go.transform.position = particles[i].position;
				}
				if (ps.main.simulationSpace == ParticleSystemSimulationSpace.Local)
				{
					go.transform.position = ps.transform.TransformPoint(particles[i].position);
				}
				newHashTrails.Add(particles[i].randomSeed, go);
			}
			hashTrails.Remove(particles[i].randomSeed);
		}
		foreach (KeyValuePair<uint, GameObject> hashTrail in hashTrails)
		{
			if (hashTrail.Value != null)
			{
				hashTrail.Value.GetComponent<ME_TrailRendererNoise>().IsActive = false;
			}
		}
		AddRange(hashTrails, newHashTrails);
	}

	public void AddRange<T, S>(Dictionary<T, S> source, Dictionary<T, S> collection)
	{
		if (collection == null)
		{
			return;
		}
		foreach (KeyValuePair<T, S> item in collection)
		{
			if (!source.ContainsKey(item.Key))
			{
				source.Add(item.Key, item.Value);
			}
		}
	}

	private void ClearEmptyHashes()
	{
		hashTrails = hashTrails.Where((KeyValuePair<uint, GameObject> h) => h.Value != null).ToDictionary((KeyValuePair<uint, GameObject> h) => h.Key, (KeyValuePair<uint, GameObject> h) => h.Value);
	}
}
