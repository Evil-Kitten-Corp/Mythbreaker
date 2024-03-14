using System;
using UnityEngine;

[Serializable]
public struct CurrentMovementSettings
{
	public float WalkSpeed;

	public float RunSpeed;

	public float SprintSpeed;

	public AnimationCurve MovementCurve;

	public AnimationCurve RotationRateCurve;

	public float CurrentSpeed;
}
