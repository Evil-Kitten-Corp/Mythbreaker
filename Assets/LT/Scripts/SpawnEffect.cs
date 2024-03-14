using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : StateMachineBehaviour
{
	public GameObject effectPrefab;

	public int spawnCount = 1;

	public float destroyTime = 1f;

	public float startDelayTime;

	public float intervalTime = 0.2f;

	public List<Vector3> spawnPositions;

	public List<Vector3> spawnEulers;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (effectPrefab == null)
		{
			Debug.LogError("Prefab is null.");
		}
		else if (animator.GetComponent<Character>() != null)
		{
			Character character = animator.GetComponent<Character>();
			character.StartCoroutine(Spawn(character));
		}
	}

	private IEnumerator Spawn(Character spawner)
	{
		yield return new WaitForSeconds(startDelayTime);
		for (int i = 0; i < spawnCount; i++)
		{
			Vector3 position = spawner.transform.position + spawner.transform.TransformDirection(spawnPositions[i]);
			Quaternion rotation = Quaternion.LookRotation(spawner.transform.forward) * Quaternion.Euler(spawnEulers[i]);
			Object.Destroy(Object.Instantiate(effectPrefab, position, rotation), destroyTime);
			yield return new WaitForSeconds(intervalTime);
		}
	}
}
