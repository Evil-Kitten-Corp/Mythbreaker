using System;
using UnityEngine;

[Serializable]
public struct AnimationCurveData
{
	[Header("[Animation Curve Data]")]
	public float DampTime;

	public float LeanTime;

	public AnimationCurve WalkCurve;

	public AnimationCurve RunCurve;

	public AnimationCurve StrafeCurve;

	public AnimationCurve LeanCurve;
}
