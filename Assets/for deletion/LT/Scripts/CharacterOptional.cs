using System;
using UnityEngine;

[Serializable]
public struct CharacterOptional
{
	[Header("[Character Optional]")]
	public bool isDoubleJump;

	public bool isBooster;

	public int jumpCount;
}
