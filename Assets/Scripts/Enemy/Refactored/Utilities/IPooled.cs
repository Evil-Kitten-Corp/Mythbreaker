using Enemy.Refactored.Utilities;
using UnityEngine;

public interface IPooledObj<T> where T : MonoBehaviour, IPooledObj<T>
{
    int PoolID { get; set; }
    ObjectPooler<T> Pool { get; set; }
}
