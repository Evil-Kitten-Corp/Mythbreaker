// Decompiled with JetBrains decompiler
// Type: FreeFlowComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class FreeFlowComponent : MonoBehaviour
{
  [Header("[Component]")]
  private Character owner;
  [SerializeField]
  [Header("[Free Flow Component]")]
  private GameObject target;
  [SerializeField]
  private LayerMask targetLayer;
  public float maxDistance = 8f;
  public float minDistance = 1.5f;
  public float angle = 140f;
  public float moveSpeed = 15f;
  public float rotationSpeed = 5f;
  public AnimationCurve moveCurve;
  [SerializeField]
  private float lerpTime = 1f;
  [SerializeField]
  private float currentLerpTime;
  [Header("[Coroutine]")]
  private Coroutine C_FreeFlow;
  [Header("[Debug]")]
  [SerializeField]
  private bool isDrawGizmos;

  private void Start() => this.Initialize();

  private void OnDrawGizmos()
  {
    if (!this.isDrawGizmos || (Object) this.target == (Object) null)
      return;
    Gizmos.color = (Object) this.target != (Object) null ? Color.green : Color.red;
    Gizmos.DrawWireSphere(this.target.transform.position + Util.GetDirection(this.target.transform.position, this.owner.transform.position), 0.2f);
  }

  private void Initialize() => this.owner = this.GetComponent<Character>();

  private IEnumerator FreeFlow()
  {
    this.target = !this.owner.LocomotionData.HasInput ? Util.GetNearestToObject(this.owner.gameObject, this.maxDistance, (LayerMask) this.targetLayer.value) : Util.GetDirectionToObject(this.owner.gameObject, this.maxDistance, this.angle, (LayerMask) this.targetLayer.value);
    this.isDrawGizmos = true;
    if (!((Object) this.target == (Object) null))
    {
      this.owner.CharacterAnim.applyRootMotion = false;
      float lerpTime = 0.1f * this.moveSpeed;
      float currentLerpTime = 0.0f;
      float rotationSpeed = 3f;
      while (this.owner.CombatData.CombatType == ECombatType.Attack)
      {
        currentLerpTime += Time.deltaTime * this.moveSpeed;
        if ((double) currentLerpTime > (double) lerpTime)
          currentLerpTime = lerpTime;
        Vector3 direction = Util.GetDirection(this.target.transform.position, this.owner.transform.position);
        int num = (int) this.owner.CharacterController.Move(-direction * (currentLerpTime / lerpTime));
        this.owner.transform.rotation = Quaternion.Slerp(this.owner.transform.rotation, Quaternion.LookRotation(-direction), currentLerpTime / lerpTime * rotationSpeed);
        yield return (object) null;
      }
      this.OnReset();
    }
  }

  public void OnReset() => this.isDrawGizmos = false;

  public void OnFreeFlow()
  {
    if (this.C_FreeFlow != null)
    {
      this.StopCoroutine(this.C_FreeFlow);
      this.OnReset();
    }
    this.C_FreeFlow = this.StartCoroutine(this.FreeFlow());
  }

  public void UpdateTarget()
  {
    this.target = !this.owner.LocomotionData.HasInput ? Util.GetNearestToObject(this.owner.gameObject, this.maxDistance, (LayerMask) this.targetLayer.value) : Util.GetDirectionToObject(this.owner.gameObject, this.maxDistance, this.angle, (LayerMask) this.targetLayer.value);
    this.isDrawGizmos = true;
    this.currentLerpTime = 0.0f;
  }

  public void UpdateFreeFlow(EFlowType flowType)
  {
    this.owner.CharacterAnim.SetFloat("Attack Ratio", 1f);
    if ((Object) this.target == (Object) null)
      return;
    Vector3 direction = Util.GetDirection(this.target.transform.position, this.owner.transform.position);
    float distance = Util.GetDistance(new Vector3(this.target.transform.position.x, 0.0f, this.target.transform.position.z), new Vector3(this.owner.transform.position.x, 0.0f, this.owner.transform.position.z));
    this.owner.CharacterAnim.SetFloat("Attack Ratio", 1f);
    if ((double) distance >= (double) this.maxDistance)
      return;
    this.owner.CharacterAnim.applyRootMotion = false;
    this.currentLerpTime += Time.deltaTime * this.moveSpeed;
    switch (flowType)
    {
      case EFlowType.Once:
        if ((double) this.currentLerpTime > (double) this.lerpTime)
        {
          this.currentLerpTime = this.lerpTime;
          break;
        }
        this.owner.CharacterAnim.SetFloat("Attack Ratio", this.moveCurve.Evaluate(Mathf.Clamp01((float) (((double) distance - (double) this.maxDistance) / ((double) this.minDistance - (double) this.maxDistance)))));
        int num1 = (int) this.owner.CharacterController.Move(-direction * this.moveCurve.Evaluate(this.currentLerpTime / this.lerpTime));
        this.owner.transform.rotation = Quaternion.Slerp(this.owner.transform.rotation, Quaternion.LookRotation(-direction), this.currentLerpTime / this.lerpTime * this.rotationSpeed);
        break;
      case EFlowType.Update:
        if ((double) this.currentLerpTime > (double) this.lerpTime)
          this.currentLerpTime = this.lerpTime;
        this.owner.CharacterAnim.SetFloat("Attack Ratio", this.moveCurve.Evaluate(Mathf.Clamp01((float) (((double) distance - (double) this.maxDistance) / ((double) this.minDistance - (double) this.maxDistance)))));
        int num2 = (int) this.owner.CharacterController.Move(-direction * this.moveCurve.Evaluate(this.currentLerpTime / this.lerpTime));
        this.owner.transform.rotation = Quaternion.Slerp(this.owner.transform.rotation, Quaternion.LookRotation(-direction), this.currentLerpTime / this.lerpTime * this.rotationSpeed);
        break;
    }
  }

  public GameObject GetTarget() => this.target;
}
