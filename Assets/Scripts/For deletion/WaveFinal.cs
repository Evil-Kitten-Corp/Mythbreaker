using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Abilities;
using AYellowpaper.SerializedCollections;
using FinalScripts;
using FinalScripts.Specials;
using SolidUtilities.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class WaveFinal : MonoBehaviour
    {
        [Header("Wave")]
        public List<WaveData> waves;
        public Transform spawnPoint;
        
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
        private PlayerInv _inv;

        public Action<GameObject> EnemySpawn;

        void Start()
        {
            _saveLoadSystem = FindObjectOfType<SaveLoadJsonFormatter>();
            _inv = FindObjectOfType<PlayerInv>();
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

            _inv.ReceiveRewards(new Reward
            {
                ability = chosenReward,  // Use the stored chosen reward
                abilityUpgrade = null,
                type = RewardType.Ability
            });

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
            littleGuys[_currentWave].color = Color.white;

            if (_currentWave >= waves.Count)
            {
                waveAnnouncer.gameObject.SetActive(false);
                AttributesManager.OnDefeatLastWave?.Invoke();
                return;
            }

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
            
                    enemy.GetComponent<EnemyBehaviour>().OnDeath += KillEnemy;
                    _currentEnemies.Add(enemy);
                    yield return new WaitForSeconds(entry.delayBetweenSpawns);
                }
            }
    
            _isEndable = true;
        }
        
        void EndWave()
        {
            GetRewards();
            _saveLoadSystem.SaveGame(new PlayerData(_currentWave, _inv.abilities.Keys
                .Select(ab => ab.id).ToList()));
            
            Instantiate(healthPrefab, healthSpawnPt);
            Time.timeScale = 0;
        }

        void KillEnemy(GameObject enemy)
        {
            _currentEnemies.Remove(enemy);
        }

        void GetRewards()
        {
            _possibleRewards.Clear();
            
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
            rewardsDescription[2].text = _possibleRewards[2].abDescription;
            
            rewardsParent.SetActive(true);
            MythUIElement.IsInUI = true;
        }

        public void ChooseReward(int i)
        {
            _chooseReward.Invoke(i);
            _isChoosingRewards = false;
            rewardsParent.SetActive(false);
            MythUIElement.IsInUI = false; 
            Time.timeScale = 1;
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
    }
}