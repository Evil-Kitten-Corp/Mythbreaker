using System;
using Minimalist.Bar.Quantity;
using TriInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FinalScripts.Refactored
{
    public abstract class EnemyBT: MonoBehaviour
    {
        public RagdollReplacer ragdoll;
        public QuantityBhv health;
        public Animator anim;
        
        [Title("Audio")]
        public RandomAudioPlayer attackAudio;
        public RandomAudioPlayer footstepAudio;
        public RandomAudioPlayer hitAudio;
        public RandomAudioPlayer gruntAudio;
        public RandomAudioPlayer deathAudio;

        public void TakeDamage(float amount, bool crit)
        {
            if (health.Amount <= 0)
            {
                return;
            }

            health.Amount -= amount;
            Debug.Log(gameObject.name + " was damaged.");
            
            Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), 
                Random.Range(0f, 0.25f), Random.Range(0f, 0.25f)); 
        
            switch (crit)
            {
                case false:
                    DamagePopUpGenerator.Current.CreatePopUp(transform.position + randomness, 
                        amount.ToString(), Color.yellow);
                    break;
            
                case true:
                    DamagePopUpGenerator.Current.CreatePopUp(transform.position + randomness, 
                        amount.ToString(), Color.cyan);
                    break;
            } 
            
            anim.SetTrigger("Hit");
            
            if (hitAudio != null)
            {
                hitAudio.PlayRandomClip();
            }

            if (health.Amount <= 0) Die();
        }

        private void Die()
        {
            if (deathAudio != null)
            {
                deathAudio.PlayRandomClip();
            }
            
            OnDeath?.Invoke(gameObject);
            ragdoll.Replace();
        }
        
        public void PlayStep()
        {
            if (footstepAudio != null)
            {
                footstepAudio.PlayRandomClip();
            }
        }

        public void Grunt()
        {
            if (gruntAudio != null)
            {
                gruntAudio.PlayRandomClip();
            }
        }

        public abstract bool Attack(Transform target);
        public abstract bool CanAttack();
        
        public event Action<GameObject> OnDeath;

        public void TriggerDeath()
        {
            Die();
        }
    }
}