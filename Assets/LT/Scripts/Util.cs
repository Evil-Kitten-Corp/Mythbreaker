using UnityEngine;

public class Util
{
	public static bool CompareToCharacter(Character a, Character b)
	{
		return a == b;
	}

	public static GameObject GetNearestToObject(GameObject owner, float radius, LayerMask layerMask)
	{
		Collider[] array = Physics.OverlapSphere(owner.transform.position, radius, layerMask.value);
		float nearestToDistance = float.PositiveInfinity;
		GameObject target = null;
		Collider[] array2 = array;
		foreach (Collider coll in array2)
		{
			if (owner.gameObject != coll.gameObject)
			{
				float distance = Vector3.Distance(owner.transform.position, coll.transform.position);
				if (nearestToDistance > distance)
				{
					nearestToDistance = distance;
					target = coll.gameObject;
				}
			}
		}
		return target;
	}

	public static GameObject GetDirectionToObject(GameObject owner, float radius, float angle, LayerMask layerMask)
	{
		Collider[] array = Physics.OverlapSphere(owner.transform.position, radius, layerMask.value);
		GameObject target = null;
		Collider[] array2 = array;
		foreach (Collider coll in array2)
		{
			Vector3 direction = GetDirection(coll.transform.position, owner.transform.position);
			if (Vector3.Angle(owner.GetComponent<CharacterMovement>().GetInputDirection, -direction) < angle * 0.5f && owner.gameObject != coll.gameObject)
			{
				target = coll.gameObject;
			}
		}
		return target;
	}

	public static Vector3 GetDirection(Vector3 a, Vector3 b, bool ignoreY = true)
	{
		Vector3 direction = (b - a).normalized;
		if (ignoreY)
		{
			direction.y = 0f;
		}
		return direction;
	}

	public static float GetDistance(Vector3 a, Vector3 b)
	{
		return Vector3.Distance(a, b);
	}
}
