using UnityEngine;

[ExecuteInEditMode]
public class Slash_AxisRotateByTime : MonoBehaviour
{
	public Vector3 RotateAxis = new Vector3(0f, 0f, 0f);

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(RotateAxis * Time.deltaTime);
	}
}
