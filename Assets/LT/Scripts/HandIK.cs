using System;
using UnityEngine;

[Serializable]
public struct HandIK
{
	[Header("[Hand IK]")]
	public HandInfo RightHand;

	public HandInfo LeftHand;
}
