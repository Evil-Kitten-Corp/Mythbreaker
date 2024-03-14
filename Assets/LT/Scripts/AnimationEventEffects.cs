using System;
using UnityEngine;

public class AnimationEventEffects : MonoBehaviour
{
	[Serializable]
	public class EffectInfo
	{
		public GameObject Effect;

		public Transform StartPositionRotation;

		public float DestroyAfter = 10f;

		public bool UseLocalPosition = true;
	}

	public EffectInfo[] Effects;

	private void Start()
	{
	}

	private void InstantiateEffect(int EffectNumber)
	{
		if (Effects == null || Effects.Length <= EffectNumber)
		{
			Debug.LogError("Incorrect effect number or effect is null");
		}
		GameObject instance = UnityEngine.Object.Instantiate(Effects[EffectNumber].Effect, Effects[EffectNumber].StartPositionRotation.position, Effects[EffectNumber].StartPositionRotation.rotation);
		if (Effects[EffectNumber].UseLocalPosition)
		{
			instance.transform.parent = Effects[EffectNumber].StartPositionRotation.transform;
			instance.transform.localPosition = Vector3.zero;
			instance.transform.localRotation = default(Quaternion);
		}
		UnityEngine.Object.Destroy(instance, Effects[EffectNumber].DestroyAfter);
	}
}
