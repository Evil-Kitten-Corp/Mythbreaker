using System;
using UnityEngine;

[Serializable]
public struct HandInfo
{
	[Header("[Hand Info]")]
	public Transform HandTransform;

	public Transform HintTransform;

	public Vector3 OffsetPosition;

	public Vector3 OffsetNormal;

	[Range(0f, 1f)]
	public float Weight;
}
