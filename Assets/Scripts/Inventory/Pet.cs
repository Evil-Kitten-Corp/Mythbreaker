using System.Collections.Generic;
using Ability_Behaviours;
using Ability_Behaviours.UI;
using AYellowpaper.SerializedCollections;
using BrunoMikoski.ServicesLocation;
using Characters.Interfaces;
using ExtEvents;
using Stats;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class Pet: MonoBehaviour, IHit
    {
        [Header("[ Abilities ]")]
        [Space]
        [SerializedDictionary("Slot To Substitute", "Ability")] 
        public SerializedDictionary<AbilitySlot, AbilityData> abilities;
        [Space]

        [Header("[ Input ]")]
        public InputActionMap map;
        [Space]
        
        [Header("[ Properties ]")]
        public bool targetable;
        public List<Stat> stats;
        public Vital health;

        private ExtEvent _damaged;

        public ExtEvent OnDamaged => _damaged;
        
        public float GetHealth()
        {
            return health.Current;
        }

        public int GetTeam()
        {
            return 0;
        }

        public void TakeDamage(int amount)
        {
            health.Decrease(amount);
            OnDamaged.Invoke();
            
            Vector3 randomness = new Vector3(Random.Range(0f, 0.25f), 
                Random.Range(0f, 0.25f), Random.Range(0f, 0.25f));
            
            ServiceLocator.Instance.GetInstance<DamagePopUpGenerator>().CreatePopUp(transform.position + 
                randomness, amount.ToString(), Color.yellow);
        }
    }
}