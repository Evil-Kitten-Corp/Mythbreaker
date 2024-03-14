using System;
using UnityEngine;

[Serializable]
public struct AnimationCurves
{
	[Header("[Animation Curves]")]
	public AnimationCurve moveCurve;

	public AnimationCurve additiveCurve;

	public AnimationCurve airCurve;

	public AnimationCurve rotationCurve;
}
