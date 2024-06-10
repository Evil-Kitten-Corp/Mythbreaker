using Minimalist.Bar.Quantity;
using UnityEngine;

namespace FinalScripts
{
    [RequireComponent(typeof(Collider))]
    public class EnemyAppendage : MonoBehaviour
    {
        public GameObject healthBar;
        public QuantityBhv health;
        public EnemyBehaviour owner;
        [HideInInspector] public bool isDead;

        public void TakeDamage(int atmAttack)
        {
            if (!isDead)
            {
                health.Amount -= atmAttack;
            
                Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), 
                    Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
               
                DamagePopUpGenerator.Current.CreatePopUp(transform.position + randomness, 
                    atmAttack.ToString(), Color.cyan);

                if (health.Amount <= 0)
                {
                    owner.stats.speed -= 1;
                    isDead = true;
                    healthBar.SetActive(false);
                    health.enabled = false;
                }
            }
        }
    }
}