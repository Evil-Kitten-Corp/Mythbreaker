using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using AYellowpaper.SerializedCollections;
using FinalScripts;
using FinalScripts.Refactored;
using FinalScripts.Specials;
using SolidUtilities.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class WaveFinal : MonoBehaviour
    {
        [Header("Wave")]
        public List<WaveData> waves;
        public AttributesManager player;
        public GameObject finalWaveTeleporter;
        
        [Header("Rewards")]
        [SerializedDictionary("Abilities", "Script")]
        public SerializableDictionary<AbilityData, AbilityController> rewards;
        public AbilityData finalLoopReward;
        
        [Header("References")]
        public GameObject rewardsParent;
        public TMP_Text[] rewardsView;
        public TMP_Text[] rewardsDescription;
        public TMP_Text waveAnnouncer;
        public TMP_Text waveCount;
        public Image[] littleGuys;
        
        public HealthPowerup healthPrefab;
        public Transform healthSpawnPt;
        
        private bool _isSpawning;
        private bool _isEndable;
        private bool _isChoosingRewards;
    
        private int _currentWave = -1;
        private float _countdownTimer;
        private float _previousTimerValue;

        private List<AbilityData> _availableRewards;

        private List<GameObject> _currentEnemies = new();
        private List<AbilityData> _possibleRewards = new();

        private Action<int> _chooseReward;
        private SaveLoadJsonFormatter _saveLoadSystem;

        public Action<GameObject> EnemySpawn;

        void Start()
        {
            _saveLoadSystem = FindObjectOfType<SaveLoadJsonFormatter>();
            _availableRewards = rewards.Keys.ToList();
            
            LoadWave();
            _chooseReward += OnRewardChosen;
        }
        
        void Update()
        {
            if (_currentEnemies.Count == 0 && _isEndable)
            {
                EndWave();
                _isEndable = false;
            }
        }

        void OnRewardChosen(int obj)
        {
            var chosenReward = _possibleRewards[obj - 1];  // Store the chosen reward
            _availableRewards.Remove(chosenReward);  // Remove the chosen reward from available rewards

            _possibleRewards.Clear();  // Clear possible rewards

            waveAnnouncer.text = "Next wave will start in...";
            StartCoroutine(WaveCooldown());
        }

        IEnumerator WaveCooldown()
        {
            _countdownTimer = 5;
            _previousTimerValue = Mathf.Ceil(_countdownTimer);
                
            while (_countdownTimer > 0)
            {
                float currentTimerValue = Mathf.Ceil(_countdownTimer);
                
                if (currentTimerValue != _previousTimerValue)
                {
                    waveAnnouncer.text = currentTimerValue.ToString();
                    _previousTimerValue = currentTimerValue;
                }
                
                _countdownTimer -= Time.deltaTime;
                yield return null;
            }

            waveAnnouncer.text = "";
            SpawnWave();
        }
        
        void SpawnWave()
        {
            _currentWave++;
            
            if (_currentWave == 8)
            {
                waveAnnouncer.gameObject.SetActive(false);
                finalWaveTeleporter.SetActive(true);
                waveCount.text = $"Final Round";
                _saveLoadSystem.SaveGame(new PlayerData(_currentWave, player.moveController.freeSpeed.walkSpeed, 
                    player.health.Amount, player.Attack));
                player.OnDefeatLastWave?.Invoke();
                return;
            } 
            
            littleGuys[_currentWave].color = Color.white;
            StartCoroutine(SpawnEnemies());
            waveCount.text = $"Wave {_currentWave + 1}";
        }

        IEnumerator SpawnEnemies()
        {
            foreach (var entry in waves[_currentWave].spawnData)
            {
                EnemyStats enemyStats = entry.enemyType;
                int count = entry.amountToSpawn;

                for (int i = 0; i < count; i++)
                {
                    Vector3 spawnPosition = entry.spawnPoint;
                    GameObject enemy = Instantiate(enemyStats.prefab, spawnPosition, Quaternion.identity);
                    EnemySpawn?.Invoke(enemy);
            
                    enemy.GetComponent<EnemyBT>().OnDeath += KillEnemy;

                    AdequateEnemyStat(enemy, enemyStats);
                    _currentEnemies.Add(enemy);
                    yield return new WaitForSeconds(entry.delayBetweenSpawns);
                }
            }
    
            _isEndable = true;
        }

        private void AdequateEnemyStat(GameObject enemy, EnemyStats enemyStats)
        {
            var x = enemy.GetComponent<EnemyBT>();
            
            // Initial amount (P0)
            float P0 = enemyStats.health;
        
            // Growth rate (r)
            float r = 0.4f;
        
            // Time (t)
            float t = _currentWave;

            x.health.MaximumAmount = CalculateExponentialGrowth(P0, r, t);
            x.health.Amount = x.health.MaximumAmount;
            
            switch (x)
            {
                case RangedEnemyBT b:
                    b.projectile.baseDamage = CalculateExponentialGrowth(enemyStats.attackDamage, r, t);
                    break;
                case MeleeEnemyBT m:
                    m.damage = (int)CalculateExponentialGrowth(m.damage, r, t);
                    break;
            }
        }
        
        private float CalculateExponentialGrowth(float initialAmount, float growthRate, float time)
        {
            return (float)(initialAmount * Math.Exp(growthRate * time));
        }

        void EndWave()
        {
            GetRewards();
            
            _saveLoadSystem.SaveGame(new PlayerData(_currentWave, player.moveController.freeSpeed.walkSpeed, 
                player.health.Amount, player.Attack));
            
            Instantiate(healthPrefab, healthSpawnPt);
            Time.timeScale = 0;
        }

        void KillEnemy(GameObject enemy)
        {
            _currentEnemies.Remove(enemy);
        }

        void GetRewards()
        {
            /*_possibleRewards.Clear();
            
            _availableRewards = rewards.Keys.Where(reward => 
                reward.canGetMoreThanOnce || !_inv.abilities.Keys.Contains(reward)).ToList();
            
            HashSet<AbilityData> selectedRewards = new HashSet<AbilityData>();
        
            while (selectedRewards.Count < 3)
            {
                if (_availableRewards.Count > 0)
                {
                    AbilityData reward = _availableRewards[Random.Range(0, _availableRewards.Count)];
                    if (selectedRewards.Add(reward))
                    {
                        _possibleRewards.Add(reward);
                    }
                }
                else
                {
                    _availableRewards.Add(finalLoopReward);
                    _possibleRewards.Add(finalLoopReward);
                }
            }
        
            _isChoosingRewards = true;

            rewardsView[0].text = _possibleRewards[0].abName;
            rewardsView[1].text = _possibleRewards[1].abName;
            rewardsView[2].text = _possibleRewards[2].abName;
            
            rewardsDescription[0].text = _possibleRewards[0].abDescription;
            rewardsDescription[1].text = _possibleRewards[1].abDescription;
            rewardsDescription[2].text = _possibleRewards[2].abDescription;*/
            
            rewardsParent.SetActive(true);
            MythUIElement.IsInUI = true;
        }

        public void ChooseReward(int i)
        {
            //_chooseReward.Invoke(i);
            _isChoosingRewards = false;
            rewardsParent.SetActive(false);
            MythUIElement.IsInUI = false; 
            Time.timeScale = 1;
            
            waveAnnouncer.text = "Next wave will start in...";
            StartCoroutine(WaveCooldown());
        }
        
        void LoadWave()
        {
            Debug.Log("Loading wave.");
            waveAnnouncer.text = "Next wave will start in...";
            _saveLoadSystem.LoadGame(out _currentWave);
            _currentWave--;

            for (int i = 0; i < _currentWave; i++)
            {
                littleGuys[i].color = Color.white;
            }
            
            StartCoroutine(WaveCooldown());
        }

        public void SpeedReward()
        {
            player.moveController.freeSpeed.walkSpeed += 1f;
            player.health.Amount += player.health.MaximumAmount / 0.3f;
            ChooseReward(0);
        }

        public void MaxHealthReward()
        {
            player.health.MaximumAmount += 25;
            ChooseReward(0);
        }

        public void AttackReward()
        {
            player.Attack += 20;
            ChooseReward(0);
        }
    }
}