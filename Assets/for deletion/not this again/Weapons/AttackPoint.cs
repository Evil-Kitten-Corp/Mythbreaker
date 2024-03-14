using System;
using UnityEngine;

namespace not_this_again.Weapons
{
    [Serializable]
    public struct AttackPoint
    {
        public float radius;
        public Vector3 offset;
        public Transform attackRoot;
    }
}