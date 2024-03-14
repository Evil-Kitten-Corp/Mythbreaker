using System;
using UnityEngine;

[Serializable]
public struct CombatData
{
	[Header("[Combat Data]")]
	public ECombatType CombatType;

	public EAttackType AttackType;

	public EAttackDirection AttackDireciton;
}
