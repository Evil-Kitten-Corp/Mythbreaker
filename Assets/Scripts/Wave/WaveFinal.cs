using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class WaveFinal : MonoBehaviour
    {
        [Header("Wave")]
        public List<WaveDefinition> waves;
        public Transform spawnPoint;
        
        [Header("Rewards")]
        public List<string> rewards;
        public string finalLoopReward;
        
        [Header("References")]
        public GameObject rewardsParent;
        public TMP_Text[] rewardsView;
        public TMP_Text waveAnnouncer;
        public TMP_Text waveCount;
        
        private bool _isSpawning;
        private bool _isEndable;
        private bool _isChoosingRewards;
    
        private int _currentWave = -1;
        private float _countdownTimer;
        private float _previousTimerValue;

        private List<string> _availableRewards;

        private List<GameObject> _currentEnemies = new();
        private List<string> _possibleRewards = new();

        private Action<int> _chooseReward;
        private SaveLoadJsonFormatter _saveLoadSystem;


        void Start()
        {
            _saveLoadSystem = FindObjectOfType<SaveLoadJsonFormatter>();
            _availableRewards = rewards;
            
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
            _availableRewards.Remove(_possibleRewards[obj - 1]);
            _possibleRewards.Clear();
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

            if (_currentWave > waves.Count)
            {
                return;
            }
        
            SpawnEnemies();
            waveCount.text = $"Wave {_currentWave + 1}";
        }
        
        void SpawnEnemies()
        {
            foreach (var kvp in waves[_currentWave].enemiesToSpawn)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    var en = Instantiate(kvp.Key, spawnPoint.position, Quaternion.identity);
                    en.GetComponent<EnemyBehaviour>().OnDeath += KillEnemy;
                    _currentEnemies.Add(en);
                }
            }
            
            _isEndable = true;
        }
        
        void EndWave()
        {
            GetRewards();
            _saveLoadSystem.SaveGame(new PlayerData(_currentWave));
            Time.timeScale = 0;
        }

        void KillEnemy(GameObject enemy)
        {
            _currentEnemies.Remove(enemy);
        }

        void GetRewards()
        {
            _possibleRewards.Clear();
            HashSet<string> selectedRewards = new HashSet<string>();
        
            while (selectedRewards.Count < 3)
            {
                if (_availableRewards.Count > 0)
                {
                    string reward = _availableRewards[Random.Range(0, _availableRewards.Count)];
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

            rewardsView[0].text = _possibleRewards[0];
            rewardsView[1].text = _possibleRewards[1];
            rewardsView[2].text = _possibleRewards[2];
            
            rewardsParent.SetActive(true);
        }

        public void ChooseReward(int i)
        {
            _chooseReward.Invoke(i);
            _isChoosingRewards = false;
            rewardsParent.SetActive(false);
            Time.timeScale = 1;
        }
        
        void LoadWave()
        {
            Debug.Log("Loading wave.");
            waveAnnouncer.text = "Next wave will start in...";
            _saveLoadSystem.LoadGame(out _currentWave);
            StartCoroutine(WaveCooldown());
        }
    }
}