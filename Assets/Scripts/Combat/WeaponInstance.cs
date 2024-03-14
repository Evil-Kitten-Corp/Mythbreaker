using UnityEngine;

namespace Combat
{
    public class WeaponInstance : MonoBehaviour
    {
        public float damage;
        public BoxCollider weaponCollider;
        private EntityCombat _owner;
        
        public void Attack() => EnableWeaponCollider(1);
        
        public void SetOwner(EntityCombat owner) => _owner = owner;
        
        private void EnableWeaponCollider(int isEnable) => weaponCollider.enabled = isEnable == 1;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Enemy"))
            {
                other.GetComponent<IDamageable>().TakeDamage(_owner.data.baseDamage + damage);
            }
        }
    }
}