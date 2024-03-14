using System;
using UnityEngine;

[Serializable]
public struct CharacterState
{
	[Header("[Character State]")]
	public LayerMask groundLayer;

	public float gravity;

	public float jumpForce;

	public float airForce;

	public float duration;

	public float rotationRate;

	public float ratio;

	public float angle;

	public bool isGrounded;

	public bool isJump;

	public bool lockedRotation;
}
