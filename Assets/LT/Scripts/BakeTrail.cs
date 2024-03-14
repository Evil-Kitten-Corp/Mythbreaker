/*using Ara;
using UnityEngine;

[RequireComponent(typeof(AraTrail))]
public class BakeTrail : MonoBehaviour
{
	private AraTrail trail;

	private void Awake()
	{
		trail = GetComponent<AraTrail>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Bake();
		}
	}

	private void Bake()
	{
		MeshFilter filter = base.gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		if (filter != null && meshRenderer != null)
		{
			filter.mesh = Object.Instantiate(trail.mesh);
			((Renderer)meshRenderer).materials = trail.materials;
			Object.Destroy(this);
			Object.Destroy(trail);
		}
		else
		{
			Debug.LogError("[BakeTrail]: Could not bake the trail because the object already had a MeshRenderer.");
		}
	}
}*/
