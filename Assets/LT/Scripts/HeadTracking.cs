using DG.Tweening;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Character))]
public class HeadTracking : MonoBehaviour
{
	private Character GetOwner;

	[Header("[Head Tracking]")]
	public Rig HeadRig;

	public Transform AimTargetTransform;

	public LayerMask TargetLayer;

	public float TrackingRadius = 10f;

	public float RetargetSpeed = 1f;

	public float WeightSpeed = 3f;

	public float MaxAngle = 90f;

	public float CurrentRigWeight;

	public Vector3 TargetPosition;

	private float RadiusSqr;

	private Vector3 OriginPos;

	[Header("[Debug]")]
	public bool IsDrawDebug;

	private void Start()
	{
		Initiate();
	}

	private void Update()
	{
		Tracking();
	}

	private void OnDrawGizmos()
	{
		if (IsDrawDebug)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(AimTargetTransform.position, 0.1f);
		}
	}

	private void Initiate()
	{
		GetOwner = GetComponent<Character>();
		RadiusSqr = TrackingRadius * TrackingRadius;
		OriginPos = AimTargetTransform.position;
	}

	private void Tracking()
	{
		Transform tracking = null;
		Collider[] targets = Physics.OverlapSphere(base.transform.position, TrackingRadius, TargetLayer);
		float shortestDistance = float.PositiveInfinity;
		Transform nearestTarget = null;
		Collider[] array = targets;
		foreach (Collider target in array)
		{
			if ((bool)target.GetComponentInParent<Character>() && target.GetComponentInParent<Character>() != GetOwner)
			{
				float dist = Vector3.Distance(target.transform.position, base.transform.position);
				Vector3 direction = target.transform.position - base.transform.position;
				if (dist < shortestDistance)
				{
					shortestDistance = dist;
					nearestTarget = target.transform;
				}
				tracking = ((!(direction.sqrMagnitude < RadiusSqr)) ? null : ((!(Vector3.Angle(base.transform.forward, direction) < MaxAngle)) ? null : nearestTarget));
			}
		}
		if (tracking != null && targets.Length != 0)
		{
			TargetPosition = tracking.GetComponentInParent<Animator>().GetBoneTransform((HumanBodyBones)10).position;
			CurrentRigWeight = 1f;
		}
		else
		{
			TargetPosition = GetOwner.transform.position + GetOwner.transform.TransformDirection(0f, 1.6f, 2f);
			CurrentRigWeight = 0f;
		}
		AimTargetTransform.DOMove(TargetPosition, RetargetSpeed);
		HeadRig.weight = Mathf.Lerp(HeadRig.weight, CurrentRigWeight, Time.deltaTime * WeightSpeed);
	}

	private void Tracking_Aiming()
	{
		AimTargetTransform.DOMove(base.transform.position + Camera.main.transform.forward * 50f, RetargetSpeed);
		HeadRig.weight = Mathf.Lerp(HeadRig.weight, 1f, Time.deltaTime * WeightSpeed);
	}

	private bool CheckTarget(Transform target)
	{
		if (target == null)
		{
			return false;
		}
		Vector3 startPosition = GetOwner.CharacterAnim.GetBoneTransform((HumanBodyBones)10).position;
		Vector3 endPosition = target.GetComponentInParent<Character>().CharacterAnim.GetBoneTransform((HumanBodyBones)8).position;
		if (Physics.Raycast(startPosition, GetTargetDirection(startPosition, endPosition, isIgnoreY: false), out var _, TrackingRadius, GetOwner.LocomotionData.GroundLayer.value))
		{
			Debug.DrawRay(startPosition, GetTargetDirection(startPosition, endPosition, isIgnoreY: false) * GetTargetDistance(target), Color.yellow);
			return false;
		}
		Debug.DrawRay(startPosition, GetTargetDirection(startPosition, endPosition, isIgnoreY: false) * GetTargetDistance(target), Color.red);
		return true;
	}

	private float GetTargetDistance(Transform target)
	{
		return Vector3.Distance(base.transform.position, target.position);
	}

	private Vector3 GetTargetDirection(Vector3 startPosition, Vector3 endPosition, bool isIgnoreY = true)
	{
		Vector3 direction = startPosition - endPosition;
		if (isIgnoreY)
		{
			direction.y = 0f;
		}
		return -direction.normalized;
	}
}
