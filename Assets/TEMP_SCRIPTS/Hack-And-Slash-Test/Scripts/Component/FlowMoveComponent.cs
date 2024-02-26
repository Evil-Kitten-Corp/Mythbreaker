using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Component
{
  public class FlowMoveComponent : MonoBehaviour
  {
    [Header("[Component]")] private global::TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character.Character _character;
    private RaycastHit _hitInfo;
    private float _delayTime;
    private const float MaxHeight = 3.5f;
    private const float MinHeight = 0.75f;

    [Header("[Flow Move Component]")] 
    public LayerMask groundLayer;
    public Vector3 offset;
    public float checkDistance = 10f;
    public float moveDuration = 1f;
    public float distance;
    public HitResult hitResult;
    public bool isFreeMove;
    public bool isMoveUp;
    
    [Header("[Wall Move Component]")] 
    public bool isWallMove;
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
    
    [Header("[Draw Debug]")] public bool IsDrawGizmos;

    private void Start()
    {
      _character = GetComponent<global::TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character.Character>();
    }

    private void LateUpdate()
    {
      SetDelayTime();
      CheckHeight();
      CheckThickness();
    }

    private void OnDrawGizmos()
    {
      if (!IsDrawGizmos)
        return;
      if (_hitInfo.collider != null)
      {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.TransformDirection(offset), _hitInfo.point);
        Gizmos.DrawSphere(_hitInfo.point, 0.15f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(StartPosition, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(CenterPosition, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(EndPosition, 0.1f);
      }

      Gizmos.DrawRay(transform.position + transform.TransformDirection(0.0f, 0.5f, 0.0f),
        (transform.forward + transform.right) * CheckWallDistance);
      Gizmos.DrawRay(transform.position + transform.TransformDirection(0.0f, 0.5f, 0.0f),
        (transform.forward + -transform.right) * CheckWallDistance);
    }

    private IEnumerator AirDrag()
    {
      var flowMoveComponent = this;
      flowMoveComponent.isWallMove = false;
      flowMoveComponent._character.CharacterAnim.CrossFadeInFixedTime("Free Move Jump_Low", 0.1f);
      flowMoveComponent.transform.DORotate(new Vector3(0.0f, flowMoveComponent.transform.eulerAngles.y, 0.0f), 1f);
      flowMoveComponent._character.LocomotionData.IsAir = true;
      while (!flowMoveComponent._character.CharacterController.isGrounded)
      {
        flowMoveComponent.MoveY += Physics.gravity.y * 4f * Time.deltaTime;
        var num = (int)flowMoveComponent._character.CharacterController
          .Move(Vector3.up * (flowMoveComponent.MoveY * Time.deltaTime));
        yield return null;
      }

      flowMoveComponent.MoveY = 0.0f;
      flowMoveComponent._character.CharacterController.slopeLimit = 45f;
    }

    private IEnumerator Parkour(eThicknessType type)
    {
      isFreeMove = true;
      _character.CharacterController.enabled = false;
      _character.LocomotionData.LockedRotation = true;
      _character.CharacterAnim.CrossFadeInFixedTime("Parkour_" + type, 0.1f);
      transform.DOPath(new Vector3[3]
      {
        transform.position,
        transform.position + transform.TransformDirection(0f, hitResult.Height, 0.5f),
        CenterPosition + ParkourOffset
      }, StartDuration).SetEase(StartCurve);
      Vector3 direction = (EndPosition - StartPosition).normalized;
      direction.y = 0f;
      base.transform.DORotateQuaternion(Quaternion.LookRotation(direction), StartDuration * 0.5f);
      yield return new WaitForSeconds(StartDuration - 0.1f);
      Vector3 endPosition = EndPosition + direction * 1f;
      endPosition.y = _hitInfo.point.y;
      base.transform.DOPath(new Vector3[2] { EndPosition, endPosition }, EndDuration, PathType.CatmullRom).SetEase(EndCurve);
      yield return new WaitForSeconds(EndDuration - 0.1f);
      isFreeMove = false;
      _character.CharacterController.enabled = true;
      _character.LocomotionData.LockedRotation = false;
    }

    private IEnumerator Mantle(eHeightType type)
    {
      isFreeMove = true;
      _character.CharacterController.enabled = false;
      _character.LocomotionData.LockedRotation = true;
      _character.CharacterAnim.CrossFadeInFixedTime("Mantle_" + type, 0.1f);
      var path = new Vector3[2]
      {
        transform.position,
        _hitInfo.collider.ClosestPoint(new Vector3(transform.position.x, _hitInfo.collider.bounds.max.y,
          transform.position.z))
      };
      transform.DOPath(path, StartDuration, PathType.CatmullRom);
      var direction = (EndPosition - StartPosition).normalized;
      direction.y = 0f;
      transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.5f);
      yield return new WaitForSeconds(0.6f);
      isFreeMove = false;
      _character.CharacterController.enabled = true;
      _character.LocomotionData.LockedRotation = false;
    }

    private void SetDelayTime()
    {
      if (!isFreeMove || _delayTime <= 0.0)
        return;
      _delayTime -= Time.deltaTime;
      var num = (int)_character.CharacterController.Move(
        (transform.forward * (distance * 3f) + Vector3.up * (hitResult.Height * 3f)) * Time.deltaTime);
      if (_delayTime > 0.0)
        return;
      isFreeMove = false;
      _delayTime = 0.0f;
    }

    private void CheckHeight()
    {
      if (isWallMove || _character.LocomotionData.IsJump || !Physics.Raycast(
            transform.position + transform.TransformDirection(offset), Vector3.down, out _hitInfo, checkDistance,
            groundLayer.value))
        return;
      distance = Vector3.Distance(transform.position,
        transform.position + transform.TransformDirection(0.0f, 0.0f, offset.z));
      hitResult.Height = _hitInfo.point.y - transform.position.y;
      isMoveUp = hitResult.Height > 0.0;
    }

    private void CheckThickness()
    {
      RaycastHit hitInfo;
      if (isFreeMove || isWallMove || _character.LocomotionData.IsJump || !Physics.Raycast(
            transform.position + transform.TransformDirection(0.0f, 0.5f, 0.0f), transform.forward, out hitInfo,
            ParkourDistance, groundLayer.value))
        return;
      var collider = hitInfo.collider;
      double x1 = transform.position.x;
      var bounds = hitInfo.collider.bounds;
      double y1 = bounds.max.y;
      double z1 = transform.position.z;
      var position1 = new Vector3((float)x1, (float)y1, (float)z1);
      StartPosition = collider.ClosestPoint(position1);
      var normalized = (StartPosition - transform.position).normalized;
      bounds = hitInfo.collider.bounds;
      double sqrMagnitude = bounds.size.sqrMagnitude;
      var position2 = StartPosition + normalized * (float)sqrMagnitude;
      var vector3 = hitInfo.collider.ClosestPoint(position2);
      double x2 = vector3.x;
      bounds = hitInfo.collider.bounds;
      double y2 = bounds.max.y;
      double z2 = vector3.z;
      EndPosition = new Vector3((float)x2, (float)y2, (float)z2);
      CenterPosition = (StartPosition + EndPosition) * 0.5f;
      hitResult.Thickness = Vector3.Distance(StartPosition, EndPosition);
      if (_character.LocomotionData.HasInput && Mathf.Abs(hitResult.Height) > (double)MinHeight &&
          Mathf.Abs(hitResult.Height) <= (double)MaxHeight && hitResult.Thickness <= (double)MaxThickness)
      {
        if (C_Parkour != null)
          StopCoroutine(C_Parkour);
        if (hitResult.Thickness < 3.0)
          C_Parkour = StartCoroutine(Parkour(eThicknessType.Short));
        else
          C_Parkour = StartCoroutine(Parkour(eThicknessType.Medium));
      }
      else
      {
        if (!_character.LocomotionData.HasInput || Mathf.Abs(hitResult.Height) <= (double)MinHeight ||
            Mathf.Abs(hitResult.Height) > (double)MaxHeight || hitResult.Thickness <= (double)MaxThickness)
          return;
        if (C_Mantle != null)
          StopCoroutine(C_Mantle);
        C_Mantle = StartCoroutine(Mantle(eHeightType.Low));
      }
    }

    private void CheckWall()
    {
      if (isFreeMove || _character.LocomotionData.IsJump)
        return;
      if (_character.CharacterAnim.GetFloat("Speed") > 0.800000011920929)
      {
        if (Physics.Raycast(transform.position + transform.TransformDirection(0.0f, 0.5f, 0.0f),
              transform.forward + transform.right, out _hitInfo, CheckWallDistance, groundLayer.value))
        {
          Angle = Vector3.Angle(transform.up, _hitInfo.normal);
          if (Angle > (double)AngleLimit || !_character.LocomotionData.HasInput)
            return;
          if (!isWallMove)
            _character.CharacterController.slopeLimit = AngleLimit;
          isWallMove = true;
          MoveY += (float)(-(double)Physics.gravity.y * 2.2000000476837158) * Time.deltaTime;
          var num = (int)_character.CharacterController.Move(Vector3.up * MoveY * Time.deltaTime);
          transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.FromToRotation(transform.up, _hitInfo.normal) * transform.rotation, Time.deltaTime * 5f);
        }
        else if (Physics.Raycast(transform.position + transform.TransformDirection(0.0f, 0.5f, 0.0f),
                   transform.forward + -transform.right, out _hitInfo, CheckWallDistance, groundLayer.value))
        {
          Angle = Vector3.Angle(transform.up, _hitInfo.normal);
          if (Angle > (double)AngleLimit || !_character.LocomotionData.HasInput)
            return;
          if (!isWallMove)
            _character.CharacterController.slopeLimit = AngleLimit;
          isWallMove = true;
          MoveY += (float)(-(double)Physics.gravity.y * 2.2000000476837158) * Time.deltaTime;
          var num = (int)_character.CharacterController.Move(Vector3.up * MoveY * Time.deltaTime);
          transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.FromToRotation(transform.up, _hitInfo.normal) * transform.rotation, Time.deltaTime * 5f);
        }
        else
        {
          if (!isWallMove)
            return;
          if (AirDragCoroutine != null)
            StopCoroutine(AirDragCoroutine);
          AirDragCoroutine = StartCoroutine(AirDrag());
        }
      }
      else
      {
        if (!isWallMove)
          return;
        if (AirDragCoroutine != null)
          StopCoroutine(AirDragCoroutine);
        AirDragCoroutine = StartCoroutine(AirDrag());
      }
    }
  }
}