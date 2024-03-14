using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(HitDecalComponent))]
[RequireComponent(typeof(CinemachineCollisionImpulseSource))]
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
		{
			owner = GetComponentInParent<Character>();
		}
		weaponCollider = GetComponent<Collider>();
		hitDecalComponent = GetComponent<HitDecalComponent>();
		hitShake = GetComponent<CinemachineCollisionImpulseSource>();
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
		{
			Character character = other.GetComponentInParent<Character>();
			Pickup(character);
		}
	}

	public virtual void Pickup(Character owner)
	{
		if (!(this.owner != null) && !(owner == null) && owner.attachSockets[(int)equipSocket].socketTransform != null)
		{
			this.owner = owner;
			owner.attachSockets[(int)equipSocket].AttachToObject(base.gameObject);
			AttachWeapon attachWeapon2 = default(AttachWeapon);
			attachWeapon2.equipSocket = equipSocket;
			attachWeapon2.unequipSocket = unequipSocket;
			attachWeapon2.weapon = this;
			AttachWeapon attachWeapon = attachWeapon2;
			owner.CurrentWeapon.Add(attachWeapon);
			weaponCollider.enabled = false;
			Physics.IgnoreCollision(weaponCollider, owner.CharacterCollider, ignore: true);
			Physics.IgnoreCollision(weaponCollider, owner.CharacterController, ignore: true);
		}
	}

	public virtual void Drop()
	{
		if (!(owner == null))
		{
			Physics.IgnoreCollision(weaponCollider, owner.CharacterCollider, ignore: false);
			Physics.IgnoreCollision(weaponCollider, owner.CharacterController, ignore: false);
		}
	}

	public bool CheckHitCharacter(Character character)
	{
		if (hitCharacters.Contains(character))
		{
			return true;
		}
		hitCharacters.Add(character);
		return false;
	}

	public void WeaponReset()
	{
		hitCharacters.Clear();
	}

	public void ShakeDirection(Vector3 direction)
	{
		hitShake.m_DefaultVelocity = direction;
	}

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
		if (C_Trace != null)
		{
			StopCoroutine(C_Trace);
		}
		if (traceType == eTraceType.Box)
		{
			C_Trace = StartCoroutine(BoxTrace());
		}
	}
}
