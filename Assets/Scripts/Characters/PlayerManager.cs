using System.Collections.Generic;
using Ability_Behaviours.UI;
using BrunoMikoski.ServicesLocation;
using Characters.Interfaces;
using ExtEvents;
using Quests.Definitions;
using Stats;
using UnityEngine;

namespace Characters
{
    public class PlayerManager : MonoBehaviour, IHit
    {
        [Header("[ Stats ]")]
        public List<Stat> stats;
        public Vital health;
        
        [Header("[ Quests ]")]
        public QuestDatabase questDatabase;
        
        [Header("[ References ]")]
        public Animator anim;
        public float speed;
        
        [Header("[ Combat ]")]
        public bool hasWeapon;
        public GameObject weapon;
        
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