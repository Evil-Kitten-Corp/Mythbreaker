using Lean.Pool;
using UnityEngine;

namespace not_this_again.Weapons
{
    public class RangedWeapon : Weapon
    {
        public Vector3 muzzleOffset;
        public Projectile projectile;
        
        public Projectile LoadedProjectile { get; set; } = null;

        public LeanGameObjectPool projectiles;
        
        private void Start()
        {
            projectiles.PreloadAll();
        }

        public void Attack(Vector3 target)
        {
            AttackProjectile(target);
        }

        public void LoadProjectile()
        {
            if (LoadedProjectile != null)
                return;

            LoadedProjectile = projectiles.Spawn(transform).GetComponent<Projectile>();
            LoadedProjectile.transform.localPosition = muzzleOffset;
            LoadedProjectile.transform.localRotation = Quaternion.identity;
        }

        void AttackProjectile(Vector3 target)
        {
            if (LoadedProjectile == null) LoadProjectile();

            LoadedProjectile.transform.SetParent(null, true);
            LoadedProjectile.Shot(target, this);
            LoadedProjectile = null; //once shot, we don't own the projectile anymore, it does it's own life.
        }
    }
}