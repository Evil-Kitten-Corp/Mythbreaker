// Decompiled with JetBrains decompiler
// Type: WeaponBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Component;
using UnityEngine;

#nullable disable
public class WeaponBase : MonoBehaviour, IPickable
{
  [Header("[Weapon Base]")]
  public Character owner;
  public EAttachSocket equipSocket;
  public EAttachSocket unequipSocket;
  public Collider weaponCollider;
  [SerializeField]
  private List<Character> hitCharacters;
  [Header("[Weapon Trace]")]
  public eTraceType traceType;
  public LayerMask hitLayer;
  public float traceRadius = 0.5f;
  [SerializeField]
  private Transform startSlot;
  [SerializeField]
  private Transform endSlot;
  private RaycastHit hitInfo;
  [Header("[Weapon Option]")]
  public HitDecalComponent hitDecalComponent;
  public CinemachineCollisionImpulseSource hitShake;
  [Header("[Coroutine]")]
  private Coroutine C_Trace;

  public bool isTrace => weaponCollider.enabled;

  private void Start()
  {
    if (GetComponentInParent<Character>() != null)
      owner = GetComponentInParent<Character>();
    weaponCollider = GetComponent<Collider>();
    hitDecalComponent = GetComponent<HitDecalComponent>();
    hitShake = GetComponent<CinemachineCollisionImpulseSource>();
  }

  protected virtual void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.layer != LayerMask.NameToLayer("Character"))
      return;
    Pickup(other.GetComponentInParent<Character>());
  }

  public virtual void Pickup(Character owner)
  {
    if (this.owner != null || owner == null || !(owner.attachSockets[(int) equipSocket].socketTransform != null))
      return;
    this.owner = owner;
    owner.attachSockets[(int) equipSocket].AttachToObject(gameObject);
    AttachWeapon attachWeapon = new AttachWeapon()
    {
      equipSocket = this.equipSocket,
      unequipSocket = this.unequipSocket,
      weapon = this
    };
    owner.CurrentWeapon.Add(attachWeapon);
    this.weaponCollider.enabled = false;
    Physics.IgnoreCollision(weaponCollider, owner.CharacterCollider, true);
    Physics.IgnoreCollision(weaponCollider, owner.CharacterController, true);
  }

  public virtual void Drop()
  {
    if ((UnityEngine.Object) this.owner == (UnityEngine.Object) null)
      return;
    Physics.IgnoreCollision(this.weaponCollider, (Collider) this.owner.CharacterCollider, false);
    Physics.IgnoreCollision(this.weaponCollider, (Collider) this.owner.CharacterController, false);
  }

  public bool CheckHitCharacter(Character character)
  {
    if (this.hitCharacters.Contains(character))
      return true;
    this.hitCharacters.Add(character);
    return false;
  }

  public void WeaponReset() => this.hitCharacters.Clear();

  public void ShakeDirection(Vector3 direction) => this.hitShake.m_DefaultVelocity = direction;

  private IEnumerator BoxTrace()
  {
    yield return new WaitForEndOfFrame();
    while (isTrace)
    {
      Collider[] colls = Physics.OverlapBox(halfExtents: new Vector3(traceRadius, Vector3.Distance(startSlot.position, endSlot.position), traceRadius) * 0.5f, center: startSlot.position, orientation: base.transform.rotation, layerMask: hitLayer.value);
      colls = colls.Where((Collider coll) => coll.GetComponentInParent<Character>() != owner).ToArray();
      Collider[] array = colls;
      foreach (Collider coll2 in array)
      {
        if ((bool)coll2.GetComponentInParent<Character>())
        {
          Character enemy = coll2.GetComponentInParent<Character>();
          if (CheckHitCharacter(enemy))
          {
            yield break;
          }
          hitDecalComponent.OnHitDecal(coll2);
          yield return new WaitForEndOfFrame();
          hitDecalComponent.OffHitDecal(coll2);
          switch (enemy.CombatData.CombatType)
          {
            case ECombatType.None:
            case ECombatType.Attack:
            case ECombatType.HitReaction:
              enemy.TakeDamage(owner, 0f);
              enemy.SetLookAt(owner.gameObject, 0.5f, 3f);
              MonoSingleton<TimeManager>.instance.OnSlowMotion(0.1f, 0.065f);
              break;
            case ECombatType.Dodge:
              yield break;
          }
          yield return new WaitForSeconds(0.1f);
          ShakeDirection(hitDecalComponent.GetDirection);
        }
      }
      yield return null;
    }
  }

  public void PlayTrace()
  {
    if (this.C_Trace != null)
      this.StopCoroutine(this.C_Trace);
    if (this.traceType != eTraceType.Box)
      return;
    this.C_Trace = this.StartCoroutine(this.BoxTrace());
  }
}
