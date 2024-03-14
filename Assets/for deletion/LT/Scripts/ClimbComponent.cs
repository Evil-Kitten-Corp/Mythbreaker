using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class ClimbComponent : MonoBehaviour
{
	[Header("[Component]")]
	private Character Character;

	[Header("[Climb Component]")]
	public eClimbMoveDirection ClimbMoveDirection;

	public float CheckDistance = 2f;

	public float CheckAngle = 140f;

	public float MaxHeight = 3f;

	public float MinHeight = 1.5f;

	public float MoveSpeed = 0.5f;

	public bool CheckClimbing;

	public bool IsClimbing;

	public Vector3 ClimbPosition;

	public Vector3 ClimbOffset;

	[Header("[Raycast]")]
	public LayerMask ClimbLayer;

	public RaycastHit HitInfo;

	[Header("[Hand IK]")]
	public bool UseHandIK;

	public bool RightHand;

	public HandIK HandIK;

	public Vector3 RightHandPosition;

	public Vector3 LeftHandPosition;

	public float IKSpeed = 0.4f;

	public float MoveDistance = 0.5f;

	public AnimationCurve MoveCurve;

	[Header("[Coroutine]")]
	private Coroutine C_CheckGround;

	[Header("[Tween]")]
	private Sequence MoveSequence;

	[Header("[Draw Gizmos]")]
	public bool IsDrawGizmos;

	public float GizmosRadius = 0.15f;

	private void Awake()
	{
		Character = GetComponent<Character>();
	}

	private void LateUpdate()
	{
		CheckClimb();
		OnUpdateClimb();
		OnClimbEnd();
		SetClimbMoveDirection();
	}

	private void OnDrawGizmos()
	{
		if (IsDrawGizmos)
		{
			Gizmos.color = (CheckClimbing ? Color.green : Color.red);
			Gizmos.DrawLine(base.transform.position + base.transform.TransformDirection(0f, 1.5f, 0f), base.transform.position + base.transform.TransformDirection(0f, 1.5f, 2f));
			Gizmos.DrawSphere(ClimbPosition, GizmosRadius);
			if (UseHandIK)
			{
				Gizmos.color = (RightHand ? Color.green : Color.red);
				Gizmos.DrawSphere(new Vector3(RightHandPosition.x, ClimbPosition.y + HandIK.RightHand.OffsetPosition.y, RightHandPosition.z), GizmosRadius * 0.5f);
				Gizmos.color = ((!RightHand) ? Color.green : Color.red);
				Gizmos.DrawSphere(new Vector3(LeftHandPosition.x, ClimbPosition.y + HandIK.LeftHand.OffsetPosition.y, LeftHandPosition.z), GizmosRadius * 0.5f);
			}
		}
	}

	private void CheckClimb()
	{
		Collider[] array = Physics.OverlapSphere(base.transform.position, CheckDistance, ClimbLayer.value);
		float shortestDistance = float.PositiveInfinity;
		Collider[] array2 = array;
		foreach (Collider coll in array2)
		{
			float distanceToTarget = Vector3.Distance(base.transform.position, coll.transform.position);
			if (distanceToTarget < shortestDistance)
			{
				shortestDistance = distanceToTarget;
				ClimbPosition = coll.ClosestPoint(new Vector3(base.transform.position.x, coll.bounds.max.y, base.transform.position.z));
				Vector3 direction = ClimbPosition - base.transform.position;
				if (Vector3.Angle(base.transform.forward, direction.normalized) < CheckAngle * 0.5f && GetClimbHeight(ClimbPosition) <= MaxHeight && GetClimbHeight(ClimbPosition) > MinHeight)
				{
					CheckClimbing = true;
					OnClimbBegin();
				}
				else
				{
					CheckClimbing = false;
				}
			}
		}
	}

	public void OnClimbBegin()
	{
		if (CheckClimbing && !IsClimbing && !Character.LocomotionData.IsGrounded)
		{
			IsClimbing = true;
			UseHandIK = true;
			Character.CharacterController.enabled = false;
			Character.LocomotionData.CharacterMoveMode = eCharacterMoveMode.Strafe;
			Character.LocomotionData.GroundLayer = 1 << LayerMask.NameToLayer("Nothing");
			base.transform.DOMove(ClimbPosition + base.transform.TransformDirection(ClimbOffset), 0.5f).OnComplete(delegate
			{
				Character.CharacterAnim.CrossFadeInFixedTime("Climb_Begin", 0.1f);
			});
			Vector3 direction = ClimbPosition - base.transform.position;
			direction.y = 0f;
			base.transform.DORotateQuaternion(Quaternion.LookRotation(direction.normalized), 0.1f);
			RightHandPosition = ClimbPosition + base.transform.TransformDirection(HandIK.RightHand.OffsetPosition);
			LeftHandPosition = ClimbPosition + base.transform.TransformDirection(HandIK.LeftHand.OffsetPosition);
		}
	}

	public void OnUpdateClimb()
	{
		if (IsClimbing)
		{
			ClimbOffset.x = MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().x * MoveSpeed;
			ClimbMoveDirection = ((ClimbOffset.x != 0f) ? ((ClimbOffset.x > 0.1f) ? eClimbMoveDirection.Right : eClimbMoveDirection.Left) : eClimbMoveDirection.None);
			base.transform.DOMoveX(ClimbPosition.x + base.transform.TransformDirection(ClimbOffset).x, 1f);
			base.transform.DOMoveY(ClimbPosition.y + base.transform.TransformDirection(ClimbOffset).y, 0f);
			base.transform.DOMoveZ(ClimbPosition.z + base.transform.TransformDirection(ClimbOffset).z, 1f);
			Vector3 direction = ClimbPosition - base.transform.position;
			direction.y = 0f;
			base.transform.DORotateQuaternion(Quaternion.LookRotation(direction.normalized), 0.1f);
		}
	}

	public void OnClimbEnd()
	{
		if (!IsClimbing)
		{
			return;
		}
		if (MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().y > 0f && MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Jump.triggered)
		{
			Character.CharacterAnim.CrossFadeInFixedTime("Climb_Up", 0.1f);
			IsClimbing = false;
			Character.LocomotionData.CharacterMoveMode = eCharacterMoveMode.Directional;
			base.transform.DOMove(ClimbPosition, 0.3f).OnComplete(delegate
			{
				UseHandIK = false;
				Character.LocomotionData.GroundLayer = 1 << LayerMask.NameToLayer("Map");
				Character.CharacterController.enabled = true;
				ClimbPosition = Vector3.zero;
				ClimbOffset.x = 0f;
			});
		}
		if (MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Move.ReadValue<Vector2>().y < 0f && MonoSingleton<InputSystemManager>.instance.GetInputAction.Locomotion.Jump.triggered)
		{
			Character.CharacterAnim.CrossFadeInFixedTime("Climb_Down", 0.1f);
			UseHandIK = false;
			Character.LocomotionData.CharacterMoveMode = eCharacterMoveMode.Directional;
			Character.LocomotionData.GroundLayer = 1 << LayerMask.NameToLayer("Map");
			Character.CharacterController.enabled = true;
		}
	}

	private void SetClimbMoveDirection()
	{
		if (!IsClimbing)
		{
			return;
		}
		switch (ClimbMoveDirection)
		{
		case eClimbMoveDirection.None:
			DOTween.To(() => RightHandPosition, delegate(Vector3 x)
			{
				RightHandPosition = x;
			}, ClimbPosition + base.transform.TransformDirection(HandIK.RightHand.OffsetPosition), IKSpeed).SetEase(MoveCurve);
			DOTween.To(() => LeftHandPosition, delegate(Vector3 x)
			{
				LeftHandPosition = x;
			}, ClimbPosition + base.transform.TransformDirection(HandIK.LeftHand.OffsetPosition), IKSpeed).SetEase(MoveCurve);
			break;
		case eClimbMoveDirection.Right:
		{
			Vector3 offset = new Vector3(MoveDistance, 0f, 0f);
			if (Vector3.Distance(ClimbPosition, Character.CharacterAnim.GetBoneTransform((HumanBodyBones)18).position) > 0.2f && RightHand)
			{
				DOTween.To(() => RightHandPosition, delegate(Vector3 x)
				{
					RightHandPosition = x;
				}, ClimbPosition + base.transform.TransformDirection(HandIK.RightHand.OffsetPosition + offset), IKSpeed).OnComplete(delegate
				{
					RightHand = false;
				}).SetEase(MoveCurve);
			}
			if (Vector3.Distance(ClimbPosition, Character.CharacterAnim.GetBoneTransform((HumanBodyBones)17).position) > 0.2f && !RightHand)
			{
				DOTween.To(() => LeftHandPosition, delegate(Vector3 x)
				{
					LeftHandPosition = x;
				}, ClimbPosition + base.transform.TransformDirection(HandIK.LeftHand.OffsetPosition + offset), IKSpeed).OnComplete(delegate
				{
					RightHand = true;
				}).SetEase(MoveCurve);
			}
			break;
		}
		case eClimbMoveDirection.Left:
		{
			Vector3 offset2 = new Vector3(0f - MoveDistance, 0f, 0f);
			if (Vector3.Distance(ClimbPosition, Character.CharacterAnim.GetBoneTransform((HumanBodyBones)18).position) > 0.2f && RightHand)
			{
				DOTween.To(() => RightHandPosition, delegate(Vector3 x)
				{
					RightHandPosition = x;
				}, ClimbPosition + base.transform.TransformDirection(HandIK.RightHand.OffsetPosition + offset2), IKSpeed).OnComplete(delegate
				{
					RightHand = false;
				}).SetEase(MoveCurve);
			}
			if (Vector3.Distance(ClimbPosition, Character.CharacterAnim.GetBoneTransform((HumanBodyBones)17).position) > 0.2f && !RightHand)
			{
				DOTween.To(() => LeftHandPosition, delegate(Vector3 x)
				{
					LeftHandPosition = x;
				}, ClimbPosition + base.transform.TransformDirection(HandIK.LeftHand.OffsetPosition + offset2), IKSpeed).OnComplete(delegate
				{
					RightHand = true;
				}).SetEase(MoveCurve);
			}
			break;
		}
		}
	}

	public float GetClimbHeight(Vector3 height)
	{
		return Mathf.Abs(base.transform.position.y - height.y);
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (UseHandIK)
		{
			Character.CharacterAnim.SetIKPosition((AvatarIKGoal)3, new Vector3(RightHandPosition.x, ClimbPosition.y + HandIK.RightHand.OffsetPosition.y, RightHandPosition.z));
			Character.CharacterAnim.SetIKPositionWeight((AvatarIKGoal)3, HandIK.RightHand.Weight);
			Character.CharacterAnim.SetIKHintPosition((AvatarIKHint)3, HandIK.RightHand.HintTransform.position);
			Character.CharacterAnim.SetIKHintPositionWeight((AvatarIKHint)3, HandIK.RightHand.Weight);
			Character.CharacterAnim.SetIKRotation((AvatarIKGoal)3, Quaternion.LookRotation(base.transform.forward) * Quaternion.Euler(HandIK.RightHand.OffsetNormal));
			Character.CharacterAnim.SetIKRotationWeight((AvatarIKGoal)3, HandIK.RightHand.Weight);
			Character.CharacterAnim.SetIKPosition((AvatarIKGoal)2, new Vector3(LeftHandPosition.x, ClimbPosition.y + HandIK.LeftHand.OffsetPosition.y, LeftHandPosition.z));
			Character.CharacterAnim.SetIKPositionWeight((AvatarIKGoal)2, HandIK.LeftHand.Weight);
			Character.CharacterAnim.SetIKHintPosition((AvatarIKHint)2, HandIK.LeftHand.HintTransform.position);
			Character.CharacterAnim.SetIKHintPositionWeight((AvatarIKHint)2, HandIK.LeftHand.Weight);
			Character.CharacterAnim.SetIKRotation((AvatarIKGoal)2, Quaternion.LookRotation(base.transform.forward) * Quaternion.Euler(-HandIK.LeftHand.OffsetNormal));
			Character.CharacterAnim.SetIKRotationWeight((AvatarIKGoal)2, HandIK.LeftHand.Weight);
		}
	}

	private IEnumerator CheckGround()
	{
		yield return (object)new WaitWhile((Func<bool>)(() => !Character.LocomotionData.IsGrounded));
		IsClimbing = false;
		ClimbPosition = Vector3.zero;
		ClimbOffset.x = 0f;
	}

	public void Anim_OnClimbEnd()
	{
		if (C_CheckGround != null)
		{
			StopCoroutine(C_CheckGround);
		}
		C_CheckGround = StartCoroutine(CheckGround());
	}
}
