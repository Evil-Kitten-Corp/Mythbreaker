using UnityEngine;

public class Slash_UIEffectList : MonoBehaviour
{
	public GameObject[] Prefabs;

	private int currentNomber;

	private GameObject currentInstance;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ChangeCurrent(int delta)
	{
		currentNomber += delta;
		if (currentNomber > Prefabs.Length - 1)
		{
			currentNomber = 0;
		}
		else if (currentNomber < 0)
		{
			currentNomber = Prefabs.Length - 1;
		}
		if (currentInstance != null)
		{
			Object.Destroy(currentInstance);
			RemoveClones();
		}
		currentInstance = Object.Instantiate(Prefabs[currentNomber]);
	}

	private void RemoveClones()
	{
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		foreach (GameObject go in array)
		{
			if (go.name.Contains("(Clone)"))
			{
				Object.Destroy(go);
			}
		}
	}
}
