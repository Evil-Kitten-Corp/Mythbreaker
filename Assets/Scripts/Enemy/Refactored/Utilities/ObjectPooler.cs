using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Refactored.Utilities
{
    public class ObjectPooler<T> where T : MonoBehaviour, IPooledObj<T>
    {
        public T[] Instances;

        protected Stack<int> FreeIdx;

        public void Initialize(int count, T prefab)
        {
            Instances = new T[count];
            FreeIdx = new Stack<int>(count);

            for (int i = 0; i < count; ++i)
            {
                Instances[i] = Object.Instantiate(prefab);
                Instances[i].gameObject.SetActive(false);
                Instances[i].PoolID = i;
                Instances[i].Pool = this;

                FreeIdx.Push(i);
            }
        }

        public T GetNew()
        {
            int idx = FreeIdx.Pop();
            Instances[idx].gameObject.SetActive(true);

            return Instances[idx];
        }

        public void Free(T obj)
        {
            FreeIdx.Push(obj.PoolID);
            Instances[obj.PoolID].gameObject.SetActive(false);
        }
    }
}