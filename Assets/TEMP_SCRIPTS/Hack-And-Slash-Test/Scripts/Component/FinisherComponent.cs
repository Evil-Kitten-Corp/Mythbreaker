// Decompiled with JetBrains decompiler
// Type: FinisherComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;
using UnityEngine.InputSystem;

#nullable disable
public class FinisherComponent : MonoBehaviour
{
  private Character character;
  [Header("[Finisher Component]")]
  public LayerMask targetLayer;
  public Character targetObject;
  public float checkRadius = 3f;
  public List<SO_Finisher> finishers;
  [Header("[Coroutine]")]
  private Coroutine C_PlayFinisher;
  [Header("[Finisher UI]")]
  public GameObject ButtonObject;
  [SerializeField]
  [Header("[Debug]")]
  private bool IsDrawGizmos;

  private void Start() => this.character = this.GetComponent<Character>();

  private void LateUpdate()
  {
    this.NearestToTarget();
    this.VisibleUI();
  }

  private void OnDrawGizmos()
  {
    if (!this.IsDrawGizmos)
      return;
    Gizmos.color = (Object) this.targetObject != (Object) null ? Color.green : Color.red;
    Gizmos.DrawWireSphere(this.transform.position, this.checkRadius);
  }

  private void NearestToTarget()
  {
    Collider[] colliderArray = Physics.OverlapSphere(this.transform.position, this.checkRadius, this.targetLayer.value);
    if (colliderArray.Length == 0)
    {
      if (!((Object) this.targetObject != (Object) null))
        return;
      this.targetObject = (Character) null;
    }
    else
    {
      float num1 = float.PositiveInfinity;
      foreach (Collider collider in colliderArray)
      {
        if (!Util.CompareToCharacter(this.character, collider.GetComponentInParent<Character>()))
        {
          float num2 = Vector3.Distance(this.character.transform.position, collider.transform.position);
          if ((double) num1 > (double) num2)
          {
            num1 = num2;
            this.targetObject = collider.GetComponent<Character>();
            this.PlayFinisher();
          }
        }
      }
      if (!((Object) this.targetObject != (Object) null) || (double) Vector3.Distance(this.transform.position, this.targetObject.transform.position) <= (double) this.checkRadius)
        return;
      this.targetObject = (Character) null;
    }
  }

  public void PlayFinisher()
  {
    if (this.character.CombatData.CombatType != ECombatType.None || MonoSingleton<InputSystemManager>.instance.GetInputAction.Combat.Finisher.phase != InputActionPhase.Performed)
      return;
    if ((Object) this.targetObject == (Object) null)
    {
      Debug.LogError((object) "target is null.");
    }
    else
    {
      if (this.C_PlayFinisher != null)
        this.StopCoroutine(this.C_PlayFinisher);
      this.C_PlayFinisher = this.StartCoroutine(this.finishers[Random.Range(0, this.finishers.Count)].finisherData.PlayFinisher(this.character, this.targetObject, 1f));
      this.character.GetComponent<CombatComponent>().ResetCombo();
    }
  }

  private void VisibleUI()
  {
    this.ButtonObject.SetActive((Object) this.targetObject != (Object) null);
    if (!((Object) this.targetObject != (Object) null))
      return;
    this.ButtonObject.transform.position = Camera.main.WorldToScreenPoint(this.targetObject.CharacterAnim.GetBoneTransform(HumanBodyBones.Chest).position);
  }
}
