using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class FreeFlowComponent : MonoBehaviour
{
	[Header("[Component]")]
	private Character owner;

	[SerializeField]
	[Header("[Free Flow Component]")]
	private GameObject target;

	[SerializeField]
	private LayerMask targetLayer;

	public float maxDistance = 8f;

	public float minDistance = 1.5f;

	public float angle = 140f;

	public float moveSpeed = 15f;

	public float rotationSpeed = 5f;

	public AnimationCurve moveCurve;

	[SerializeField]
	private float lerpTime = 1f;

	[SerializeField]
	private float currentLerpTime;

	[Header("[Coroutine]")]
	private Coroutine C_FreeFlow;

	[Header("[Debug]")]
	[SerializeField]
	private bool isDrawGizmos;

	private void Start()
	{
		Initialize();
	}

	private void OnDrawGizmos()
	{
		if (isDrawGizmos && !(target == null))
		{
			Gizmos.color = ((target != null) ? Color.green : Color.red);
			Vector3 direction = Util.GetDirection(target.transform.position, owner.transform.position);
			Gizmos.DrawWireSphere(target.transform.position + direction, 0.2f);
		}
	}

	private void Initialize()
	{
		owner = GetComponent<Character>();
	}

	private IEnumerator FreeFlow()
	{
		if (owner.LocomotionData.HasInput)
		{
			target = Util.GetDirectionToObject(owner.gameObject, maxDistance, angle, targetLayer.value);
		}
		else
		{
			target = Util.GetNearestToObject(owner.gameObject, maxDistance, targetLayer.value);
		}
		isDrawGizmos = true;
		if (target == null)
		{
			yield break;
		}
		owner.CharacterAnim.applyRootMotion = false;
		float lerpTime = 0.1f * moveSpeed;
		float currentLerpTime = 0f;
		float rotationSpeed = 3f;
		while (owner.CombatData.CombatType == ECombatType.Attack)
		{
			currentLerpTime += Time.deltaTime * moveSpeed;
			if (currentLerpTime > lerpTime)
			{
				currentLerpTime = lerpTime;
			}
			Vector3 direction = Util.GetDirection(target.transform.position, owner.transform.position);
			owner.CharacterController.Move(-direction * (currentLerpTime / lerpTime));
			owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(-direction), currentLerpTime / lerpTime * rotationSpeed);
			yield return null;
		}
		OnReset();
	}

	public void OnReset()
	{
		isDrawGizmos = false;
	}

	public void OnFreeFlow()
	{
		if (C_FreeFlow != null)
		{
			StopCoroutine(C_FreeFlow);
			OnReset();
		}
		C_FreeFlow = StartCoroutine(FreeFlow());
	}

	public void UpdateTarget()
	{
		if (owner.LocomotionData.HasInput)
		{
			target = Util.GetDirectionToObject(owner.gameObject, maxDistance, angle, targetLayer.value);
		}
		else
		{
			target = Util.GetNearestToObject(owner.gameObject, maxDistance, targetLayer.value);
		}
		isDrawGizmos = true;
		currentLerpTime = 0f;
	}

	public void UpdateFreeFlow(EFlowType flowType)
	{
		owner.CharacterAnim.SetFloat("Attack Ratio", 1f);
		if (target == null)
		{
			return;
		}
		Vector3 direction = Util.GetDirection(target.transform.position, owner.transform.position);
		float distance = Util.GetDistance(new Vector3(target.transform.position.x, 0f, target.transform.position.z), new Vector3(owner.transform.position.x, 0f, owner.transform.position.z));
		owner.CharacterAnim.SetFloat("Attack Ratio", 1f);
		if (distance >= maxDistance)
		{
			return;
		}
		owner.CharacterAnim.applyRootMotion = false;
		currentLerpTime += Time.deltaTime * moveSpeed;
		switch (flowType)
		{
		case EFlowType.Once:
		{
			if (currentLerpTime > lerpTime)
			{
				currentLerpTime = lerpTime;
				break;
			}
			float maxRatio = Mathf.Clamp01((distance - maxDistance) / (minDistance - maxDistance));
			owner.CharacterAnim.SetFloat("Attack Ratio", moveCurve.Evaluate(maxRatio));
			owner.CharacterController.Move(-direction * moveCurve.Evaluate(currentLerpTime / lerpTime));
			owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(-direction), currentLerpTime / lerpTime * rotationSpeed);
			break;
		}
		case EFlowType.Update:
		{
			if (currentLerpTime > lerpTime)
			{
				currentLerpTime = lerpTime;
			}
			float maxRatio2 = Mathf.Clamp01((distance - maxDistance) / (minDistance - maxDistance));
			owner.CharacterAnim.SetFloat("Attack Ratio", moveCurve.Evaluate(maxRatio2));
			owner.CharacterController.Move(-direction * moveCurve.Evaluate(currentLerpTime / lerpTime));
			owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(-direction), currentLerpTime / lerpTime * rotationSpeed);
			break;
		}
		}
	}

	public GameObject GetTarget()
	{
		return target;
	}
}
