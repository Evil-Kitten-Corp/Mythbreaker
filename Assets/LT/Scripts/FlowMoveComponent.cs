using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class FlowMoveComponent : MonoBehaviour
{
	[Header("[Component]")]
	private Character Character;

	private RaycastHit HitInfo;

	private float DelayTime;

	private readonly float MaxHeight = 3.5f;

	private readonly float MinHeight = 0.75f;

	[Header("[Flow Move Component]")]
	public LayerMask GroundLayer;

	public Vector3 Offset;

	public float CheckDistance = 10f;

	public float MoveDuration = 1f;

	public float Distance;

	public HitResult HitResult;

	public bool IsFreeMove;

	public bool IsMoveUp;

	[Header("[Wall Move Component]")]
	public bool IsWallMove;

	public float CheckWallDistance = 0.6f;

	public float Angle;

	public float AngleLimit = 90f;

	public float MoveY;

	private Coroutine AirDragCoroutine;

	[Header("Parkour Component")]
	public float ParkourDistance = 1f;

	public float MaxThickness = 4f;

	public Vector3 ParkourOffset;

	public float StartDuration = 0.5f;

	public float EndDuration = 1f;

	public AnimationCurve StartCurve;

	public AnimationCurve EndCurve;

	private Vector3 StartPosition;

	private Vector3 CenterPosition;

	private Vector3 EndPosition;

	[Header("[Coroutine]")]
	private Coroutine C_Parkour;

	private Coroutine C_Mantle;

	[Header("[Draw Debug]")]
	public bool IsDrawGizmos;

	private void Start()
	{
		Character = GetComponent<Character>();
	}

	private void LateUpdate()
	{
		SetDelayTime();
		CheckHeight();
		CheckThickness();
	}

	private void OnDrawGizmos()
	{
		if (IsDrawGizmos)
		{
			if (HitInfo.collider != null)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(base.transform.position + base.transform.TransformDirection(Offset), HitInfo.point);
				Gizmos.DrawSphere(HitInfo.point, 0.15f);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(StartPosition, 0.1f);
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(CenterPosition, 0.1f);
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(EndPosition, 0.1f);
			}
			Gizmos.DrawRay(base.transform.position + base.transform.TransformDirection(0f, 0.5f, 0f), (base.transform.forward + base.transform.right) * CheckWallDistance);
			Gizmos.DrawRay(base.transform.position + base.transform.TransformDirection(0f, 0.5f, 0f), (base.transform.forward + -base.transform.right) * CheckWallDistance);
		}
	}

	private IEnumerator AirDrag()
	{
		IsWallMove = false;
		Character.CharacterAnim.CrossFadeInFixedTime("Free Move Jump_Low", 0.1f);
		base.transform.DORotate(new Vector3(0f, base.transform.eulerAngles.y, 0f), 1f);
		Character.LocomotionData.IsAir = true;
		while (!Character.CharacterController.isGrounded)
		{
			MoveY += Physics.gravity.y * 4f * Time.deltaTime;
			Character.CharacterController.Move(Vector3.up * MoveY * Time.deltaTime);
			yield return null;
		}
		MoveY = 0f;
		Character.CharacterController.slopeLimit = 45f;
	}

	private IEnumerator Parkour(eThicknessType type)
	{
		IsFreeMove = true;
		Character.CharacterController.enabled = false;
		Character.LocomotionData.LockedRotation = true;
		Character.CharacterAnim.CrossFadeInFixedTime("Parkour_" + type, 0.1f);
		base.transform.DOPath(new Vector3[3]
		{
			base.transform.position,
			base.transform.position + base.transform.TransformDirection(0f, HitResult.Height, 0.5f),
			CenterPosition + ParkourOffset
		}, StartDuration).SetEase(StartCurve);
		Vector3 direction = (EndPosition - StartPosition).normalized;
		direction.y = 0f;
		base.transform.DORotateQuaternion(Quaternion.LookRotation(direction), StartDuration * 0.5f);
		yield return new WaitForSeconds(StartDuration - 0.1f);
		Vector3 endPosition = EndPosition + direction * 1f;
		endPosition.y = HitInfo.point.y;
		base.transform.DOPath(new Vector3[2] { EndPosition, endPosition }, EndDuration, PathType.CatmullRom).SetEase(EndCurve);
		yield return new WaitForSeconds(EndDuration - 0.1f);
		IsFreeMove = false;
		Character.CharacterController.enabled = true;
		Character.LocomotionData.LockedRotation = false;
	}

	private IEnumerator Mantle(eHeightType type)
	{
		IsFreeMove = true;
		Character.CharacterController.enabled = false;
		Character.LocomotionData.LockedRotation = true;
		Character.CharacterAnim.CrossFadeInFixedTime("Mantle_" + type, 0.1f);
		Vector3[] path = new Vector3[2]
		{
			base.transform.position,
			HitInfo.collider.ClosestPoint(new Vector3(base.transform.position.x, HitInfo.collider.bounds.max.y, base.transform.position.z))
		};
		base.transform.DOPath(path, StartDuration, PathType.CatmullRom);
		Vector3 direction = (EndPosition - StartPosition).normalized;
		direction.y = 0f;
		base.transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);
		yield return new WaitForSeconds(0.6f);
		IsFreeMove = false;
		Character.CharacterController.enabled = true;
		Character.LocomotionData.LockedRotation = false;
	}

	private void SetDelayTime()
	{
		if (IsFreeMove && DelayTime > 0f)
		{
			DelayTime -= Time.deltaTime;
			Character.CharacterController.Move((base.transform.forward * (Distance * 3f) + Vector3.up * (HitResult.Height * 3f)) * Time.deltaTime);
			if (DelayTime <= 0f)
			{
				IsFreeMove = false;
				DelayTime = 0f;
			}
		}
	}

	private void CheckHeight()
	{
		if (!IsWallMove && !Character.LocomotionData.IsJump && Physics.Raycast(base.transform.position + base.transform.TransformDirection(Offset), Vector3.down, out HitInfo, CheckDistance, GroundLayer.value))
		{
			Distance = Vector3.Distance(base.transform.position, base.transform.position + base.transform.TransformDirection(0f, 0f, Offset.z));
			HitResult.Height = HitInfo.point.y - base.transform.position.y;
			IsMoveUp = HitResult.Height > 0f;
		}
	}

	private void CheckThickness()
	{
		if (IsFreeMove || IsWallMove || Character.LocomotionData.IsJump || !Physics.Raycast(base.transform.position + base.transform.TransformDirection(0f, 0.5f, 0f), base.transform.forward, out var hitInfo, ParkourDistance, GroundLayer.value))
		{
			return;
		}
		StartPosition = hitInfo.collider.ClosestPoint(new Vector3(base.transform.position.x, hitInfo.collider.bounds.max.y, base.transform.position.z));
		Vector3 posToCollider = (StartPosition - base.transform.position).normalized * hitInfo.collider.bounds.size.sqrMagnitude;
		Vector3 otherSide = StartPosition + posToCollider;
		Vector3 farPoint = hitInfo.collider.ClosestPoint(otherSide);
		EndPosition = new Vector3(farPoint.x, hitInfo.collider.bounds.max.y, farPoint.z);
		CenterPosition = (StartPosition + EndPosition) * 0.5f;
		HitResult.Thickness = Vector3.Distance(StartPosition, EndPosition);
		if (Character.LocomotionData.HasInput && Mathf.Abs(HitResult.Height) > MinHeight && Mathf.Abs(HitResult.Height) <= MaxHeight && HitResult.Thickness <= MaxThickness)
		{
			if (C_Parkour != null)
			{
				StopCoroutine(C_Parkour);
			}
			if (HitResult.Thickness < 3f)
			{
				C_Parkour = StartCoroutine(Parkour(eThicknessType.Short));
			}
			else
			{
				C_Parkour = StartCoroutine(Parkour(eThicknessType.Medium));
			}
		}
		else if (Character.LocomotionData.HasInput && Mathf.Abs(HitResult.Height) > MinHeight && Mathf.Abs(HitResult.Height) <= MaxHeight && HitResult.Thickness > MaxThickness)
		{
			if (C_Mantle != null)
			{
				StopCoroutine(C_Mantle);
			}
			C_Mantle = StartCoroutine(Mantle(eHeightType.Low));
		}
	}

	private void CheckWall()
	{
		if (IsFreeMove || Character.LocomotionData.IsJump)
		{
			return;
		}
		if (Character.CharacterAnim.GetFloat("Speed") > 0.8f)
		{
			if (Physics.Raycast(base.transform.position + base.transform.TransformDirection(0f, 0.5f, 0f), base.transform.forward + base.transform.right, out HitInfo, CheckWallDistance, GroundLayer.value))
			{
				Angle = Vector3.Angle(base.transform.up, HitInfo.normal);
				if (!(Angle > AngleLimit) && Character.LocomotionData.HasInput)
				{
					if (!IsWallMove)
					{
						Character.CharacterController.slopeLimit = AngleLimit;
					}
					IsWallMove = true;
					MoveY += (0f - Physics.gravity.y) * 2.2f * Time.deltaTime;
					Character.CharacterController.Move(Vector3.up * MoveY * Time.deltaTime);
					Quaternion normalRot2 = Quaternion.FromToRotation(base.transform.up, HitInfo.normal);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, normalRot2 * base.transform.rotation, Time.deltaTime * 5f);
				}
			}
			else if (Physics.Raycast(base.transform.position + base.transform.TransformDirection(0f, 0.5f, 0f), base.transform.forward + -base.transform.right, out HitInfo, CheckWallDistance, GroundLayer.value))
			{
				Angle = Vector3.Angle(base.transform.up, HitInfo.normal);
				if (!(Angle > AngleLimit) && Character.LocomotionData.HasInput)
				{
					if (!IsWallMove)
					{
						Character.CharacterController.slopeLimit = AngleLimit;
					}
					IsWallMove = true;
					MoveY += (0f - Physics.gravity.y) * 2.2f * Time.deltaTime;
					Character.CharacterController.Move(Vector3.up * MoveY * Time.deltaTime);
					Quaternion normalRot = Quaternion.FromToRotation(base.transform.up, HitInfo.normal);
					base.transform.rotation = Quaternion.Slerp(base.transform.rotation, normalRot * base.transform.rotation, Time.deltaTime * 5f);
				}
			}
			else if (IsWallMove)
			{
				if (AirDragCoroutine != null)
				{
					StopCoroutine(AirDragCoroutine);
				}
				AirDragCoroutine = StartCoroutine(AirDrag());
			}
		}
		else if (IsWallMove)
		{
			if (AirDragCoroutine != null)
			{
				StopCoroutine(AirDragCoroutine);
			}
			AirDragCoroutine = StartCoroutine(AirDrag());
		}
	}
}
