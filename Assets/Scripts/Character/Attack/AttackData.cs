using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Combat/AttackData", order = 1)]
public class AttackData : ScriptableObject
{
    public float damage;
    public bool canCrit;
}