

using System;
using UnityEngine;

public static class ME_ColorHelper
{
	public struct HSBColor
	{
		public float H;

		public float S;

		public float B;

		public float A;

		public HSBColor(float h, float s, float b, float a)
		{
			H = h;
			S = s;
			B = b;
			A = a;
		}
	}

	private const float TOLERANCE = 0.0001f;

	private static string[] colorProperties = new string[10] { "_TintColor", "_Color", "_EmissionColor", "_BorderColor", "_ReflectColor", "_RimColor", "_MainColor", "_CoreColor", "_FresnelColor", "_CutoutColor" };

	public static HSBColor ColorToHSV(Color color)
	{
		HSBColor ret = new HSBColor(0f, 0f, 0f, color.a);
		float r = color.r;
		float g = color.g;
		float b = color.b;
		float max = Mathf.Max(r, Mathf.Max(g, b));
		if (max <= 0f)
		{
			return ret;
		}
		float min = Mathf.Min(r, Mathf.Min(g, b));
		float dif = max - min;
		if (max > min)
		{
			if (Math.Abs(g - max) < 0.0001f)
			{
				ret.H = (b - r) / dif * 60f + 120f;
			}
			else if (Math.Abs(b - max) < 0.0001f)
			{
				ret.H = (r - g) / dif * 60f + 240f;
			}
			else if (b > g)
			{
				ret.H = (g - b) / dif * 60f + 360f;
			}
			else
			{
				ret.H = (g - b) / dif * 60f;
			}
			if (ret.H < 0f)
			{
				ret.H += 360f;
			}
		}
		else
		{
			ret.H = 0f;
		}
		ret.H *= 0.0027777778f;
		ret.S = dif / max * 1f;
		ret.B = max;
		return ret;
	}

	public static Color HSVToColor(HSBColor hsbColor)
	{
		float r = hsbColor.B;
		float g = hsbColor.B;
		float b = hsbColor.B;
		if (Math.Abs(hsbColor.S) > 0.0001f)
		{
			float max = hsbColor.B;
			float dif = hsbColor.B * hsbColor.S;
			float min = hsbColor.B - dif;
			float h = hsbColor.H * 360f;
			if (h < 60f)
			{
				r = max;
				g = h * dif / 60f + min;
				b = min;
			}
			else if (h < 120f)
			{
				r = (0f - (h - 120f)) * dif / 60f + min;
				g = max;
				b = min;
			}
			else if (h < 180f)
			{
				r = min;
				g = max;
				b = (h - 120f) * dif / 60f + min;
			}
			else if (h < 240f)
			{
				r = min;
				g = (0f - (h - 240f)) * dif / 60f + min;
				b = max;
			}
			else if (h < 300f)
			{
				r = (h - 240f) * dif / 60f + min;
				g = min;
				b = max;
			}
			else if (h <= 360f)
			{
				r = max;
				g = min;
				b = (0f - (h - 360f)) * dif / 60f + min;
			}
			else
			{
				r = 0f;
				g = 0f;
				b = 0f;
			}
		}
		return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsbColor.A);
	}

	public static Color ConvertRGBColorByHUE(Color rgbColor, float hue)
	{
		float brightness = ColorToHSV(rgbColor).B;
		if (brightness < 0.0001f)
		{
			brightness = 0.0001f;
		}
		HSBColor hsv = ColorToHSV(rgbColor / brightness);
		hsv.H = hue;
		Color color = HSVToColor(hsv) * brightness;
		color.a = rgbColor.a;
		return color;
	}

	public static void ChangeObjectColorByHUE(GameObject go, float hue)
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>(includeInactive: true);
		foreach (Renderer rend in componentsInChildren)
		{
			Material[] mats = (Application.isPlaying ? rend.materials : rend.sharedMaterials);
			if (mats.Length == 0)
			{
				continue;
			}
			string[] array = colorProperties;
			foreach (string colorProperty in array)
			{
				Material[] array2 = mats;
				foreach (Material mat in array2)
				{
					if (mat != null && mat.HasProperty(colorProperty))
					{
						setMatHUEColor(mat, colorProperty, hue);
					}
				}
			}
		}
		ParticleSystemRenderer[] componentsInChildren2 = go.GetComponentsInChildren<ParticleSystemRenderer>(includeInactive: true);
		foreach (ParticleSystemRenderer rend2 in componentsInChildren2)
		{
			Material mat2 = rend2.trailMaterial;
			if (mat2 == null)
			{
				continue;
			}
			mat2 = (rend2.trailMaterial = new Material(mat2)
			{
				name = mat2.name + " (Instance)"
			});
			string[] array = colorProperties;
			foreach (string colorProperty2 in array)
			{
				if (mat2 != null && mat2.HasProperty(colorProperty2))
				{
					setMatHUEColor(mat2, colorProperty2, hue);
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren3 = go.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		foreach (SkinnedMeshRenderer skinRend in componentsInChildren3)
		{
			Material[] mats2 = (Application.isPlaying ? ((Renderer)skinRend).materials : ((Renderer)skinRend).sharedMaterials);
			if (mats2.Length == 0)
			{
				continue;
			}
			string[] array = colorProperties;
			foreach (string colorProperty3 in array)
			{
				Material[] array2 = mats2;
				foreach (Material mat3 in array2)
				{
					if (mat3 != null && mat3.HasProperty(colorProperty3))
					{
						setMatHUEColor(mat3, colorProperty3, hue);
					}
				}
			}
		}
		Projector[] componentsInChildren4 = go.GetComponentsInChildren<Projector>(includeInactive: true);
		foreach (Projector proj in componentsInChildren4)
		{
			if (!proj.material.name.EndsWith("(Instance)"))
			{
				proj.material = new Material(proj.material)
				{
					name = proj.material.name + " (Instance)"
				};
			}
			Material mat4 = proj.material;
			if (mat4 == null)
			{
				continue;
			}
			string[] array = colorProperties;
			foreach (string colorProperty4 in array)
			{
				if (mat4 != null && mat4.HasProperty(colorProperty4))
				{
					proj.material = setMatHUEColor(mat4, colorProperty4, hue);
				}
			}
		}
		Light[] componentsInChildren5 = go.GetComponentsInChildren<Light>(includeInactive: true);
		foreach (Light obj in componentsInChildren5)
		{
			HSBColor hsv = ColorToHSV(obj.color);
			hsv.H = hue;
			obj.color = HSVToColor(hsv);
		}
		ParticleSystem[] componentsInChildren6 = go.GetComponentsInChildren<ParticleSystem>(includeInactive: true);
		foreach (ParticleSystem obj2 in componentsInChildren6)
		{
			ParticleSystem.MainModule main = obj2.main;
			HSBColor hsv2 = ColorToHSV(obj2.main.startColor.color);
			hsv2.H = hue;
			main.startColor = HSVToColor(hsv2);
			ParticleSystem.ColorOverLifetimeModule colorProperty5 = obj2.colorOverLifetime;
			ParticleSystem.MinMaxGradient colorPS = colorProperty5.color;
			Gradient gradient = colorProperty5.color.gradient;
			GradientColorKey[] keys = colorProperty5.color.gradient.colorKeys;
			float offsetGradient = 0f;
			hsv2 = ColorToHSV(keys[0].color);
			offsetGradient = Math.Abs(ColorToHSV(keys[1].color).H - hsv2.H);
			hsv2.H = hue;
			keys[0].color = HSVToColor(hsv2);
			for (int i = 1; i < keys.Length; i++)
			{
				hsv2 = ColorToHSV(keys[i].color);
				hsv2.H = Mathf.Repeat(hsv2.H + offsetGradient, 1f);
				keys[i].color = HSVToColor(hsv2);
			}
			gradient.colorKeys = keys;
			colorPS.gradient = gradient;
			colorProperty5.color = colorPS;
		}
	}

	private static Material setMatHUEColor(Material mat, string name, float hueColor)
	{
		Color color = ConvertRGBColorByHUE(mat.GetColor(name), hueColor);
		mat.SetColor(name, color);
		return mat;
	}

	private static Material setMatAlphaColor(Material mat, string name, float alpha)
	{
		Color oldColor = mat.GetColor(name);
		oldColor.a = alpha;
		mat.SetColor(name, oldColor);
		return mat;
	}
}
