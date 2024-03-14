using System;
using UnityEngine;

[Serializable]
public struct AttachWeapon
{
	[Header("[Attach Weapon]")]
	public EAttachSocket equipSocket;

	public EAttachSocket unequipSocket;

	public WeaponBase weapon;
}
