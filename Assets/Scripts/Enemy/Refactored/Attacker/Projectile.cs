using Enemy.Refactored.Utilities;
using FinalScripts.Refactored.Attacker;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IPooledObj<Projectile>
{
    public int PoolID { get; set; }
    public ObjectPooler<Projectile> Pool { get; set; }

    public abstract void Shot(Vector3 target, RangedAttacker shooter);
}
