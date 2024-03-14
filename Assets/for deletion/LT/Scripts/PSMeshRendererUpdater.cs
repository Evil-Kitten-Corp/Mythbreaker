using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class PSMeshRendererUpdater : MonoBehaviour
{
	private class ParticleStartInfo
	{
		public ParticleSystem.MinMaxCurve StartSize;

		public ParticleSystem.MinMaxCurve StartSpeed;
	}

	public GameObject MeshObject;

	public float StartScaleMultiplier = 1f;

	public Color Color = Color.black;

	private const string materialName = "MeshEffect";

	private List<Material[]> rendererMaterials = new List<Material[]>();

	private List<Material[]> skinnedMaterials = new List<Material[]>();

	public bool IsActive = true;

	public float FadeTime = 1.5f;

	private bool currentActiveStatus;

	private bool needUpdateAlpha;

	private Color oldColor = Color.black;

	private float currentAlphaTime;

	private string[] colorProperties = new string[9] { "_TintColor", "_Color", "_EmissionColor", "_BorderColor", "_ReflectColor", "_RimColor", "_MainColor", "_CoreColor", "_FresnelColor" };

	private float alpha;

	private float prevAlpha;

	private Dictionary<string, float> startAlphaColors;

	private bool previousActiveStatus;

	private bool needUpdate;

	private bool needLastUpdate;

	private Dictionary<ParticleSystem, ParticleStartInfo> startParticleParameters;

	private void OnEnable()
	{
		alpha = 0f;
		prevAlpha = 0f;
		IsActive = true;
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (startAlphaColors == null)
		{
			InitStartAlphaColors();
		}
		if (IsActive && alpha < 1f)
		{
			alpha += Time.deltaTime / FadeTime;
		}
		if (!IsActive && alpha > 0f)
		{
			alpha -= Time.deltaTime / FadeTime;
		}
		if (alpha > 0f && alpha < 1f)
		{
			needUpdate = true;
		}
		else
		{
			needUpdate = false;
			alpha = Mathf.Clamp01(alpha);
			if (Mathf.Abs(prevAlpha - alpha) >= Mathf.Epsilon)
			{
				UpdateVisibleStatus();
			}
		}
		prevAlpha = alpha;
		if (needUpdate)
		{
			UpdateVisibleStatus();
		}
		if (Color != oldColor)
		{
			oldColor = Color;
			UpdateColor(Color);
		}
	}

	private void InitStartAlphaColors()
	{
		startAlphaColors = new Dictionary<string, float>();
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer rend in componentsInChildren)
		{
			Material[] mats = rend.materials;
			for (int i = 0; i < mats.Length; i++)
			{
				if (mats[i].name.Contains("MeshEffect"))
				{
					GetStartAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i]);
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		foreach (SkinnedMeshRenderer rend2 in componentsInChildren2)
		{
			Material[] mats2 = ((Renderer)rend2).materials;
			for (int j = 0; j < mats2.Length; j++)
			{
				if (mats2[j].name.Contains("MeshEffect"))
				{
					GetStartAlphaByProperties(rend2.GetHashCode().ToString(), j, mats2[j]);
				}
			}
		}
		Light[] lights = GetComponentsInChildren<Light>(includeInactive: true);
		for (int m = 0; m < lights.Length; m++)
		{
			ME_LightCurves lightCurve = lights[m].GetComponent<ME_LightCurves>();
			float intencity = 1f;
			if (lightCurve != null)
			{
				intencity = lightCurve.GraphIntensityMultiplier;
			}
			startAlphaColors.Add(lights[m].GetHashCode().ToString() + m, intencity);
		}
		componentsInChildren = MeshObject.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer rend3 in componentsInChildren)
		{
			Material[] mats3 = rend3.materials;
			for (int k = 0; k < mats3.Length; k++)
			{
				if (mats3[k].name.Contains("MeshEffect"))
				{
					GetStartAlphaByProperties(rend3.GetHashCode().ToString(), k, mats3[k]);
				}
			}
		}
		componentsInChildren2 = MeshObject.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		foreach (SkinnedMeshRenderer rend4 in componentsInChildren2)
		{
			Material[] mats4 = ((Renderer)rend4).materials;
			for (int l = 0; l < mats4.Length; l++)
			{
				if (mats4[l].name.Contains("MeshEffect"))
				{
					GetStartAlphaByProperties(rend4.GetHashCode().ToString(), l, mats4[l]);
				}
			}
		}
	}

	private void InitStartParticleParameters()
	{
		startParticleParameters = new Dictionary<ParticleSystem, ParticleStartInfo>();
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		foreach (ParticleSystem ps in componentsInChildren)
		{
			startParticleParameters.Add(ps, new ParticleStartInfo
			{
				StartSize = ps.main.startSize,
				StartSpeed = ps.main.startSpeed
			});
		}
	}

	private void UpdateVisibleStatus()
	{
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer rend in componentsInChildren)
		{
			Material[] mats = rend.materials;
			for (int i = 0; i < mats.Length; i++)
			{
				if (mats[i].name.Contains("MeshEffect"))
				{
					UpdateAlphaByProperties(rend.GetHashCode().ToString(), i, mats[i], alpha);
				}
			}
		}
		componentsInChildren = GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer rend2 in componentsInChildren)
		{
			Material[] mats2 = rend2.materials;
			for (int j = 0; j < mats2.Length; j++)
			{
				if (mats2[j].name.Contains("MeshEffect"))
				{
					UpdateAlphaByProperties(rend2.GetHashCode().ToString(), j, mats2[j], alpha);
				}
			}
		}
		componentsInChildren = MeshObject.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer rend3 in componentsInChildren)
		{
			Material[] mats3 = rend3.materials;
			for (int k = 0; k < mats3.Length; k++)
			{
				if (mats3[k].name.Contains("MeshEffect"))
				{
					UpdateAlphaByProperties(rend3.GetHashCode().ToString(), k, mats3[k], alpha);
				}
			}
		}
		componentsInChildren = MeshObject.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer rend4 in componentsInChildren)
		{
			Material[] mats4 = rend4.materials;
			for (int l = 0; l < mats4.Length; l++)
			{
				if (mats4[l].name.Contains("MeshEffect"))
				{
					UpdateAlphaByProperties(rend4.GetHashCode().ToString(), l, mats4[l], alpha);
				}
			}
		}
		ME_LightCurves[] componentsInChildren2 = GetComponentsInChildren<ME_LightCurves>(includeInactive: true);
		for (int n = 0; n < componentsInChildren2.Length; n++)
		{
			componentsInChildren2[n].enabled = IsActive;
		}
		Light[] lights = GetComponentsInChildren<Light>(includeInactive: true);
		for (int m = 0; m < lights.Length; m++)
		{
			if (!IsActive)
			{
				float startAlpha = startAlphaColors[lights[m].GetHashCode().ToString() + m];
				lights[m].intensity = alpha * startAlpha;
			}
		}
		ParticleSystem[] componentsInChildren3 = GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		foreach (ParticleSystem ps in componentsInChildren3)
		{
			if (!IsActive && !ps.isStopped)
			{
				ps.Stop();
			}
			if (IsActive && ps.isStopped)
			{
				ps.Play();
			}
		}
		ME_TrailRendererNoise[] componentsInChildren4 = GetComponentsInChildren<ME_TrailRendererNoise>();
		for (int n = 0; n < componentsInChildren4.Length; n++)
		{
			componentsInChildren4[n].IsActive = IsActive;
		}
	}

	private void UpdateAlphaByProperties(string rendName, int materialNumber, Material mat, float alpha)
	{
		string[] array = colorProperties;
		foreach (string prop in array)
		{
			if (mat.HasProperty(prop))
			{
				float startAlpha = startAlphaColors[rendName + materialNumber + prop.ToString()];
				Color color = mat.GetColor(prop);
				color.a = alpha * startAlpha;
				mat.SetColor(prop, color);
			}
		}
	}

	private void GetStartAlphaByProperties(string rendName, int materialNumber, Material mat)
	{
		string[] array = colorProperties;
		foreach (string prop in array)
		{
			if (mat.HasProperty(prop))
			{
				string key = rendName + materialNumber + prop.ToString();
				if (!startAlphaColors.ContainsKey(key))
				{
					startAlphaColors.Add(rendName + materialNumber + prop.ToString(), mat.GetColor(prop).a);
				}
			}
		}
	}

	public void UpdateColor(Color color)
	{
		if (!(MeshObject == null))
		{
			ME_ColorHelper.HSBColor hsv = ME_ColorHelper.ColorToHSV(color);
			ME_ColorHelper.ChangeObjectColorByHUE(MeshObject, hsv.H);
		}
	}

	public void UpdateColor(float HUE)
	{
		if (!(MeshObject == null))
		{
			ME_ColorHelper.ChangeObjectColorByHUE(MeshObject, HUE);
		}
	}

	public void UpdateMeshEffect()
	{
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = default(Quaternion);
		rendererMaterials.Clear();
		skinnedMaterials.Clear();
		if (!(MeshObject == null))
		{
			UpdatePSMesh(MeshObject);
			AddMaterialToMesh(MeshObject);
		}
	}

	private void CheckScaleIncludedParticles()
	{
	}

	public void UpdateMeshEffect(GameObject go)
	{
		rendererMaterials.Clear();
		skinnedMaterials.Clear();
		if (go == null)
		{
			Debug.Log("You need set a gameObject");
			return;
		}
		MeshObject = go;
		UpdatePSMesh(MeshObject);
		AddMaterialToMesh(MeshObject);
	}

	private void UpdatePSMesh(GameObject go)
	{
		if (startParticleParameters == null)
		{
			InitStartParticleParameters();
		}
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		MeshRenderer meshRend = go.GetComponentInChildren<MeshRenderer>();
		SkinnedMeshRenderer skinMeshRend = go.GetComponentInChildren<SkinnedMeshRenderer>();
		Light[] lights = GetComponentsInChildren<Light>();
		float realBound = 1f;
		float transformMax = 1f;
		if (meshRend != null)
		{
			realBound = ((Renderer)meshRend).bounds.size.magnitude;
			transformMax = meshRend.transform.lossyScale.magnitude;
		}
		if (skinMeshRend != null)
		{
			realBound = ((Renderer)skinMeshRend).bounds.size.magnitude;
			transformMax = skinMeshRend.transform.lossyScale.magnitude;
		}
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSys in array)
		{
			particleSys.transform.gameObject.SetActive(value: false);
			ParticleSystem.ShapeModule sh = particleSys.shape;
			if (sh.enabled)
			{
				if (meshRend != null)
				{
					sh.shapeType = ParticleSystemShapeType.MeshRenderer;
					sh.meshRenderer = meshRend;
				}
				if (skinMeshRend != null)
				{
					sh.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
					sh.skinnedMeshRenderer = skinMeshRend;
				}
			}
			ParticleSystem.MainModule mainPS = particleSys.main;
			ParticleStartInfo startParticleInfo = startParticleParameters[particleSys];
			mainPS.startSize = UpdateParticleParam(startParticleInfo.StartSize, mainPS.startSize, realBound / transformMax * StartScaleMultiplier);
			mainPS.startSpeed = UpdateParticleParam(startParticleInfo.StartSpeed, mainPS.startSpeed, realBound / transformMax * StartScaleMultiplier);
			particleSys.transform.gameObject.SetActive(value: true);
		}
		if (meshRend != null)
		{
			Light[] array2 = lights;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].transform.position = ((Renderer)meshRend).bounds.center;
			}
		}
		if (skinMeshRend != null)
		{
			Light[] array2 = lights;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].transform.position = ((Renderer)skinMeshRend).bounds.center;
			}
		}
	}

	private ParticleSystem.MinMaxCurve UpdateParticleParam(ParticleSystem.MinMaxCurve startParam, ParticleSystem.MinMaxCurve currentParam, float scale)
	{
		if (currentParam.mode == ParticleSystemCurveMode.TwoConstants)
		{
			currentParam.constantMin = startParam.constantMin * scale;
			currentParam.constantMax = startParam.constantMax * scale;
		}
		else if (currentParam.mode == ParticleSystemCurveMode.Constant)
		{
			currentParam.constant = startParam.constant * scale;
		}
		return currentParam;
	}

	private void AddMaterialToMesh(GameObject go)
	{
		ME_MeshMaterialEffect meshMatEffect = GetComponentInChildren<ME_MeshMaterialEffect>();
		if (!(meshMatEffect == null))
		{
			MeshRenderer meshRenderer = go.GetComponentInChildren<MeshRenderer>();
			SkinnedMeshRenderer skinMeshRenderer = go.GetComponentInChildren<SkinnedMeshRenderer>();
			if (meshRenderer != null)
			{
				rendererMaterials.Add(((Renderer)meshRenderer).sharedMaterials);
				((Renderer)meshRenderer).sharedMaterials = AddToSharedMaterial(((Renderer)meshRenderer).sharedMaterials, meshMatEffect);
			}
			if (skinMeshRenderer != null)
			{
				skinnedMaterials.Add(((Renderer)skinMeshRenderer).sharedMaterials);
				((Renderer)skinMeshRenderer).sharedMaterials = AddToSharedMaterial(((Renderer)skinMeshRenderer).sharedMaterials, meshMatEffect);
			}
		}
	}

	private Material[] AddToSharedMaterial(Material[] sharedMaterials, ME_MeshMaterialEffect meshMatEffect)
	{
		if (meshMatEffect.IsFirstMaterial)
		{
			return new Material[1] { meshMatEffect.Material };
		}
		List<Material> materials = sharedMaterials.ToList();
		for (int i = 0; i < materials.Count; i++)
		{
			if (materials[i].name.Contains("MeshEffect"))
			{
				materials.RemoveAt(i);
			}
		}
		materials.Add(meshMatEffect.Material);
		return materials.ToArray();
	}

	private void OnDestroy()
	{
		if (MeshObject == null)
		{
			return;
		}
		MeshRenderer[] meshRenderers = MeshObject.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] skinMeshRenderers = MeshObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int j = 0; j < meshRenderers.Length; j++)
		{
			if (rendererMaterials.Count == meshRenderers.Length)
			{
				((Renderer)meshRenderers[j]).sharedMaterials = rendererMaterials[j];
			}
			List<Material> materials = ((Renderer)meshRenderers[j]).sharedMaterials.ToList();
			for (int k = 0; k < materials.Count; k++)
			{
				if (materials[k].name.Contains("MeshEffect"))
				{
					materials.RemoveAt(k);
				}
			}
			((Renderer)meshRenderers[j]).sharedMaterials = materials.ToArray();
		}
		for (int i = 0; i < skinMeshRenderers.Length; i++)
		{
			if (skinnedMaterials.Count == skinMeshRenderers.Length)
			{
				((Renderer)skinMeshRenderers[i]).sharedMaterials = skinnedMaterials[i];
			}
			List<Material> materials2 = ((Renderer)skinMeshRenderers[i]).sharedMaterials.ToList();
			for (int l = 0; l < materials2.Count; l++)
			{
				if (materials2[l].name.Contains("MeshEffect"))
				{
					materials2.RemoveAt(l);
				}
			}
			((Renderer)skinMeshRenderers[i]).sharedMaterials = materials2.ToArray();
		}
		rendererMaterials.Clear();
		skinnedMaterials.Clear();
	}
}
