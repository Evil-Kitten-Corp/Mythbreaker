using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(ReflectionProbe))]
public class ME_Reflection : MonoBehaviour
{
	public RenderTexture tex;

	private ReflectionProbe reflectionProbe;

	private List<Light> dirLight;

	private List<float> lightIntencity;

	private void Awake()
	{
		Light[] array = FindObjectsOfType<Light>();
		dirLight = new List<Light>();
		lightIntencity = new List<float>();
		Light[] array2 = array;
		foreach (Light i in array2)
		{
			if (i.type == LightType.Directional)
			{
				dirLight.Add(i);
				lightIntencity.Add(i.intensity);
			}
		}
		reflectionProbe = GetComponent<ReflectionProbe>();
		tex = new RenderTexture(reflectionProbe.resolution, reflectionProbe.resolution, 0);
		((Texture)tex).dimension = TextureDimension.Cube;
		tex.useMipMap = true;
		Shader.SetGlobalTexture("ME_Reflection", (Texture)tex);
		reflectionProbe.RenderProbe(tex);
	}

	private void Update()
	{
		bool requireUpdate = false;
		for (int i = 0; i < dirLight.Count; i++)
		{
			if (Math.Abs(dirLight[i].intensity - lightIntencity[i]) > 0.001f)
			{
				requireUpdate = true;
				lightIntencity[i] = dirLight[i].intensity;
			}
		}
		if (requireUpdate)
		{
			reflectionProbe.RenderProbe(tex);
		}
	}
}
