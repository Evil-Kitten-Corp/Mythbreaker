using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HitDecalComponent : MonoBehaviour
{
	[Header("[Hit Decal Component]")]
	public GameObject decalPrefab;

	public float fadeSpeed = 1f;

	private Vector3 direction;

	private Vector3 startPosition;

	private Vector3 endPosition;

	[Header("[Coroutine]")]
	private Coroutine C_FadeoutEmission;

	public Vector3 GetDirection => direction;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(startPosition, 0.1f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(endPosition, 0.1f);
	}

	private IEnumerator FadeoutEmission(DecalProjector decal)
	{
		if (!(decal == null))
		{
			decal.fadeFactor = 1f;
			while (decal.fadeFactor > 0f)
			{
				decal.fadeFactor -= Time.deltaTime * fadeSpeed;
				yield return null;
			}
			Destroy(decal.gameObject);
		}
	}

	public void OnHitDecal(Collider other)
	{
		startPosition = other.ClosestPoint(transform.position);
	}

	public void OffHitDecal(Collider other)
	{
		endPosition = other.ClosestPoint(transform.position);
		direction = endPosition - startPosition;
		DecalProjector decal = Instantiate(decalPrefab, (startPosition + endPosition) * 0.5f, 
			Quaternion.LookRotation(direction), other.transform).GetComponent<DecalProjector>();
		//decal.decalLayerMask = DecalLayerEnum.DecalLayer1;
		StartCoroutine(FadeoutEmission(decal));
	}
}
