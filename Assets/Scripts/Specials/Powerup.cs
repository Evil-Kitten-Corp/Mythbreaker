using System;
using UnityEngine;

namespace FinalScripts.Specials
{
    [RequireComponent(typeof(Collider))]
    public abstract class Powerup : MonoBehaviour
    {
        public Action OnPickup;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPickup?.Invoke();
            }
        }
    }
}