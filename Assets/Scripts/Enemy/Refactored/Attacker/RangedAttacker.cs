using Enemy.Refactored.Utilities;
using UnityEngine;

namespace FinalScripts.Refactored.Attacker
{
    public class RangedAttacker : MonoBehaviour
    {
        public Vector3 muzzleOffset;

        public Projectile projectile;

        public Projectile LoadedProjectile => LoadedProj;

        protected Projectile LoadedProj = null;
        protected ObjectPooler<Projectile> ProjectilePool;

        private void Start()
        {
            ProjectilePool = new ObjectPooler<Projectile>();
            ProjectilePool.Initialize(20, projectile);
        }

        public void Attack(Vector3 target)
        {
            AttackProjectile(target);
        }

        public void LoadProjectile()
        {
            if (LoadedProj != null)
                return;

            LoadedProj = ProjectilePool.GetNew();
            LoadedProj.transform.SetParent(transform, false);
            LoadedProj.transform.localPosition = muzzleOffset;
            LoadedProj.transform.localRotation = Quaternion.identity;
        }

        void AttackProjectile(Vector3 target)
        {
            if (LoadedProj == null) LoadProjectile();

            LoadedProj.transform.SetParent(null, true);
            LoadedProj.Shot(target, this);
            LoadedProj = null; //once shot, we don't own the projectile anymore, it does it's own life.
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 worldOffset = transform.TransformPoint(muzzleOffset);
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawLine(worldOffset + Vector3.up * 0.4f, worldOffset + Vector3.down * 0.4f);
            UnityEditor.Handles.DrawLine(worldOffset + Vector3.forward * 0.4f, worldOffset + Vector3.back * 0.4f);
        }
#endif
    }
}