using Characters.Interfaces;
using UnityEngine;

namespace Characters.Combat.Info
{
    public class HurtInfo3D : HurtInfoBase
    {
        public Vector3 center, forward, right;

        public HurtInfo3D() : base()
        {

        }

        public HurtInfo3D(HitInfoBase hitInfo, Vector3 center, Vector3 forward, Vector3 right)
        {
            this.HitInfo = hitInfo;
            this.center = center;
            this.forward = forward;
            this.right = right;
        }
    }
}