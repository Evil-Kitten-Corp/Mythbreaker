using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AnimationEvent : MonoBehaviour
{
	[SerializeField]
	[Tooltip("This is an event script used in animation clips.")]
	[Header("[Animation Event]")]
	private Character owner;

	private void Start()
	{
		owner = GetComponent<Character>();
	}

	public void Anim_OnAttack(string type)
	{
		if (string.IsNullOrEmpty(type))
		{
			owner.CombatData.AttackType = EAttackType.LightAttack;
			owner.CombatData.AttackDireciton = EAttackDirection.Front;
			Debug.LogError("Anim_OnAttack - parameter is null.");
		}
		else
		{
			owner.CombatData.AttackType = (EAttackType)Enum.Parse(typeof(EAttackType), type.Split("/")[0]);
			owner.CombatData.AttackDireciton = (EAttackDirection)Enum.Parse(typeof(EAttackDirection), type.Split("/")[1]);
		}
		if (owner.CurrentWeapon.Count > 0)
		{
			owner.CurrentWeapon.ForEach(delegate(AttachWeapon obj)
			{
				obj.weapon.WeaponReset();
				obj.weapon.weaponCollider.enabled = true;
				obj.weapon.PlayTrace();
			});
		}
	}

	public void Anim_OffAttack()
	{
		if (owner.CurrentWeapon.Count > 0)
		{
			owner.CurrentWeapon.ForEach(delegate(AttachWeapon obj)
			{
				obj.weapon.weaponCollider.enabled = false;
			});
		}
	}

	public void Anim_OnHit()
	{
		owner.CombatData.CombatType = ECombatType.HitReaction;
	}

	public void Anim_OffHit()
	{
		owner.CombatData.CombatType = ECombatType.None;
	}
}
