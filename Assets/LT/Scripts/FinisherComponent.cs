using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Character))]
public class FinisherComponent : MonoBehaviour
{
	private Character character;

	[Header("[Finisher Component]")]
	public LayerMask targetLayer;

	public Character targetObject;

	public float checkRadius = 3f;

	public List<SO_Finisher> finishers;

	[Header("[Coroutine]")]
	private Coroutine C_PlayFinisher;

	[Header("[Finisher UI]")]
	public GameObject ButtonObject;

	[SerializeField]
	[Header("[Debug]")]
	private bool IsDrawGizmos;

	private void Start()
	{
		character = GetComponent<Character>();
	}

	private void LateUpdate()
	{
		NearestToTarget();
		VisibleUI();
	}

	private void OnDrawGizmos()
	{
		if (IsDrawGizmos)
		{
			Gizmos.color = ((targetObject != null) ? Color.green : Color.red);
			Gizmos.DrawWireSphere(base.transform.position, checkRadius);
		}
	}

	private void NearestToTarget()
	{
		Collider[] colls = Physics.OverlapSphere(base.transform.position, checkRadius, targetLayer.value);
		if (colls.Length == 0)
		{
			if (targetObject != null)
			{
				targetObject = null;
			}
			return;
		}
		float nearestToDistance = float.PositiveInfinity;
		Collider[] array = colls;
		foreach (Collider coll in array)
		{
			if (!Util.CompareToCharacter(character, coll.GetComponentInParent<Character>()))
			{
				float distance = Vector3.Distance(character.transform.position, coll.transform.position);
				if (nearestToDistance > distance)
				{
					nearestToDistance = distance;
					targetObject = coll.GetComponent<Character>();
					PlayFinisher();
				}
			}
		}
		if (targetObject != null && Vector3.Distance(base.transform.position, targetObject.transform.position) > checkRadius)
		{
			targetObject = null;
		}
	}

	public void PlayFinisher()
	{
		if (character.CombatData.CombatType != 0 || MonoSingleton<InputSystemManager>.instance.GetInputAction.Combat.Finisher.phase != InputActionPhase.Performed)
		{
			return;
		}
		if (targetObject == null)
		{
			Debug.LogError("target is null.");
			return;
		}
		if (C_PlayFinisher != null)
		{
			StopCoroutine(C_PlayFinisher);
		}
		C_PlayFinisher = StartCoroutine(finishers[Random.Range(0, finishers.Count)].finisherData.PlayFinisher(character, targetObject, 1f));
		character.GetComponent<CombatComponent>().ResetCombo();
	}

	private void VisibleUI()
	{
		ButtonObject.SetActive(targetObject != null);
		if (targetObject != null)
		{
			ButtonObject.transform.position = Camera.main.WorldToScreenPoint(targetObject.CharacterAnim.GetBoneTransform((HumanBodyBones)8).position);
		}
	}
}
