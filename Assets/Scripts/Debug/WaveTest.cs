using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SolidUtilities.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveTest : MonoBehaviour
{
    public List<string> rewards;
    public string finalLoopReward;
    public int waves;
    public SerializableDictionary<string, int> enemiesToSpawn;

    private bool _isSpawning;
    private bool _isEndable;
    private bool _isChoosingRewards;
    
    private int _currentWave = 0;
    private float _countdownTimer;
    private float _previousTimerValue;

    private List<string> _availableRewards;

    private List<string> _currentEnemies = new();
    private List<string> _possibleRewards = new();

    private Action<int> _chooseReward;

    private void Start()
    {
        _availableRewards = rewards;
        SpawnWave();
        _chooseReward += OnRewardChosen;
    }

    private void OnRewardChosen(int obj)
    {
        Debug.Log($"You chose {_possibleRewards[obj-1]}!");
        _availableRewards.Remove(_possibleRewards[obj - 1]);
        _possibleRewards.Clear();
        Debug.Log("Next wave will start in...");
        StartCoroutine(WaveCooldown());
    }

    private IEnumerator WaveCooldown()
    {
        _countdownTimer = 5;
        _previousTimerValue = Mathf.Ceil(_countdownTimer);
                
        while (_countdownTimer > 0)
        {
            float currentTimerValue = Mathf.Ceil(_countdownTimer);
            if (currentTimerValue != _previousTimerValue)
            {
                Debug.Log(currentTimerValue);
                _previousTimerValue = currentTimerValue;
            }
            _countdownTimer -= Time.deltaTime;
            yield return null;
        }
        
        SpawnWave();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var removedEnemy = _currentEnemies.ElementAt(Random.Range(0, _currentEnemies.Count));
            _currentEnemies.Remove(removedEnemy);
            Debug.Log($"{removedEnemy} was killed.");
        }

        if (_currentEnemies.Count == 0 && _isEndable)
        {
            EndWave();
            _isEndable = false;
        }

        if (_isChoosingRewards)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _chooseReward.Invoke(1);
                _isChoosingRewards = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _chooseReward.Invoke(2);
                _isChoosingRewards = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _chooseReward.Invoke(3);
                _isChoosingRewards = false;
            }
        }
    }

    void EndWave()
    {
        GetRewards();
    }

    private void GetRewards()
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
        
        Debug.Log($"Possible rewards: {_possibleRewards[0]}, {_possibleRewards[1]}, {_possibleRewards[2]}.");
    }

    void SpawnWave()
    {
        _currentWave++;

        if (_currentWave > waves)
        {
            Debug.Log("Waves have finished.");
            return;
        }
        
        _isEndable = true;
        Debug.Log($"Wave number {_currentWave} just begun.");
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        foreach (var kvp in enemiesToSpawn)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                Debug.Log($"Spawned {kvp.Key}.");
                _currentEnemies.Add(kvp.Key);
            }
        }
    }
}
