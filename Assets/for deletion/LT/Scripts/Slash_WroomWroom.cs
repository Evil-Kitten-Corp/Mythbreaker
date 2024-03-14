using UnityEngine;

public class Slash_WroomWroom : MonoBehaviour
{
	public float Speed = 1f;

	public GameObject Target;

	private void OnEnable()
	{
		base.transform.localPosition = new Vector3(0f, 0f, 0f);
	}

	private void Update()
	{
		base.transform.position = Vector3.MoveTowards(base.transform.position, Target.transform.position, Speed * Time.deltaTime);
	}
}
