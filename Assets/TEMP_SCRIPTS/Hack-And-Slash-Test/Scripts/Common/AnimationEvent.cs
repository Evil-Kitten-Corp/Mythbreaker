// Decompiled with JetBrains decompiler
// Type: AnimationEvent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using TEMP_SCRIPTS.Hack_And_Slash_Test.Scripts.Character;
using UnityEngine;

#nullable disable
public class AnimationEvent : MonoBehaviour
{
  [SerializeField]
  [Tooltip("This is an event script used in animation clips.")]
  [Header("[Animation Event]")]
  private Character owner;

  private void Start() => this.owner = this.GetComponent<Character>();

  public void Anim_OnAttack(string type)
  {
    if (string.IsNullOrEmpty(type))
    {
      this.owner.CombatData.AttackType = EAttackType.LightAttack;
      this.owner.CombatData.AttackDireciton = EAttackDirection.Front;
      Debug.LogError((object) "Anim_OnAttack - parameter is null.");
    }
    else
    {
      this.owner.CombatData.AttackType = (EAttackType) Enum.Parse(typeof (EAttackType), type.Split("/", StringSplitOptions.None)[0]);
      this.owner.CombatData.AttackDireciton = (EAttackDirection) Enum.Parse(typeof (EAttackDirection), type.Split("/", StringSplitOptions.None)[1]);
    }
    if (this.owner.CurrentWeapon.Count <= 0)
      return;
    this.owner.CurrentWeapon.ForEach((Action<AttachWeapon>) (obj =>
    {
      obj.weapon.WeaponReset();
      obj.weapon.weaponCollider.enabled = true;
      obj.weapon.PlayTrace();
    }));
  }

  public void Anim_OffAttack()
  {
    if (this.owner.CurrentWeapon.Count <= 0)
      return;
    this.owner.CurrentWeapon.ForEach((Action<AttachWeapon>) (obj => obj.weapon.weaponCollider.enabled = false));
  }

  public void Anim_OnHit() => this.owner.CombatData.CombatType = ECombatType.HitReaction;

  public void Anim_OffHit() => this.owner.CombatData.CombatType = ECombatType.None;
}
