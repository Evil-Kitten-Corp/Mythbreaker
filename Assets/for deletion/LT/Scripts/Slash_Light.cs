using UnityEngine;

[RequireComponent(typeof(Light))]
public class Slash_Light : MonoBehaviour
{
	public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	public Gradient LightColor = new Gradient();

	public float GraphTimeMultiplier = 1f;

	public float GraphIntensityMultiplier = 1f;

	public bool IsLoop;

	[HideInInspector]
	public bool canUpdate;

	private float startTime;

	private Color startColor;

	private Light lightSource;

	public void SetStartColor(Color color)
	{
		startColor = color;
	}

	private void Awake()
	{
		lightSource = GetComponent<Light>();
		startColor = lightSource.color;
		lightSource.intensity = LightCurve.Evaluate(0f) * GraphIntensityMultiplier;
		lightSource.color = startColor * LightColor.Evaluate(0f);
		startTime = Time.time;
		canUpdate = true;
	}

	private void OnEnable()
	{
		startTime = Time.time;
		canUpdate = true;
		if (lightSource != null)
		{
			lightSource.intensity = LightCurve.Evaluate(0f) * GraphIntensityMultiplier;
			lightSource.color = startColor * LightColor.Evaluate(0f);
		}
	}

	private void Update()
	{
		float time = Time.time - startTime;
		if (canUpdate)
		{
			float eval = LightCurve.Evaluate(time / GraphTimeMultiplier) * GraphIntensityMultiplier;
			lightSource.intensity = eval;
			lightSource.color = startColor * LightColor.Evaluate(time / GraphTimeMultiplier);
		}
		if (time >= GraphTimeMultiplier)
		{
			if (IsLoop)
			{
				startTime = Time.time;
			}
			else
			{
				canUpdate = false;
			}
		}
	}
}
