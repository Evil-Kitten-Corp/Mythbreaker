using System;
using UnityEngine;

[Serializable]
public struct AttachSocket
{
	[Header("[Weapon Socket]")]
	public EAttachSocket attachSocket;

	public Transform socketTransform;

	public void AttachToObject(GameObject obj)
	{
		obj.transform.SetParent(socketTransform);
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
	}
}
