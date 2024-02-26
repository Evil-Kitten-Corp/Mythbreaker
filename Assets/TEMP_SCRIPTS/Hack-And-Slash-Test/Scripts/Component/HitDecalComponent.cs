using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Component
{
  public class HitDecalComponent : MonoBehaviour
  {
    [Header("[Hit Decal Component]")] 
    public GameObject decalPrefab;
    public float fadeSpeed = 1f;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    
    [Header("[Coroutine]")] private Coroutine _cFadeoutEmission;

    public Vector3 GetDirection { get; private set; }

    private void OnDrawGizmos()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawSphere(_startPosition, 0.1f);
      Gizmos.color = Color.green;
      Gizmos.DrawSphere(_endPosition, 0.1f);
    }

    private IEnumerator FadeoutEmission(DecalProjector decal)
    {
      if (!(decal == null))
      {
        decal.fadeFactor = 1f;
        while (decal.fadeFactor > 0.0)
        {
          decal.fadeFactor -= Time.deltaTime * fadeSpeed;
          yield return null;
        }

        Destroy(decal.gameObject);
      }
    }

    public void OnHitDecal(Collider other)
    {
      _startPosition = other.ClosestPoint(transform.position);
    }

    public void OffHitDecal(Collider other)
    {
      _endPosition = other.ClosestPoint(transform.position);
      GetDirection = _endPosition - _startPosition;
      DecalProjector decal = Instantiate(decalPrefab, (_startPosition + _endPosition) * 0.5f,
        Quaternion.LookRotation(GetDirection), other.transform).GetComponent<DecalProjector>();
      StartCoroutine(FadeoutEmission(decal));
    }
  }
}