using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public struct LeanAmount
{
	public float LR;

	public float FB;

	public LeanAmount(float _LR, float _FB)
	{
		this = default(LeanAmount);
		LR = _LR;
		FB = _FB;
	}

	public void TweenLeanAmount(LeanAmount current, LeanAmount target, float duration)
	{
		DOTween.To(() => current.LR, delegate(float x)
		{
			current.LR = x;
		}, target.LR, duration);
		DOTween.To(() => current.FB, delegate(float x)
		{
			current.FB = x;
		}, target.FB, duration);
		Debug.LogError(new Vector2(current.LR, current.FB));
	}
}
