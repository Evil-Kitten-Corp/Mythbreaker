using System.Collections;
using System.Collections.Generic;
using FinalScripts;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveData[] waves;
    public Transform[] spawnPoints;
    public float spawnRadius = 10f;
    public TMP_Text waveAnnouncer;
    public float delayBetweenWaves;
    
    private float _countdownTimer;
    private int _currentWaveIndex = 0;
    private readonly List<GameObject> _currentEnemies = new();
    private SaveLoadJsonFormatter _saveLoadSystem;
    private bool _isSpawning;
    private bool _isEndable;
    
    void Start()
    {
        _saveLoadSystem = FindObjectOfType<SaveLoadJsonFormatter>();
        _isEndable = false;
        LoadWave();
    }

    void Update()
    {
        if (_currentEnemies.Count == 0 && !_isSpawning && _isEndable)
        {
            EndWave();
            _isEndable = false;
        }
    }

    private void EndWave()
    {
        GiveRewards();
        _saveLoadSystem.SaveGame(new PlayerData(_currentWaveIndex));
        StartCountdown();
    }
    
    public void StartCountdown()
    {
        _countdownTimer = delayBetweenWaves;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (_countdownTimer > 0)
        {
            waveAnnouncer.text = "Next wave in: " + Mathf.Ceil(_countdownTimer);
            _countdownTimer -= Time.deltaTime;
            yield return null;
        }

        StartWave();
    }

    public void StartWave()
    {
        _currentWaveIndex++;
        _isEndable = true;
        
        if (_currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves completed!");
            return;
        }
        
        StartCoroutine(SpawnEnemies(waves[_currentWaveIndex]));
    }

    IEnumerator SpawnEnemies(WaveData wave)
    {
        _isSpawning = true;

        foreach (var entry in wave.spawnData)
        {
            EnemyStats enemyStats = entry.Key;
            int count = entry.Value;

            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPosition;

                if (spawnPoints.Length > 0)
                {
                    spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                }
                else
                {
                    spawnPosition = GetRandomSpawnPosition();
                }

                GameObject enemy = Instantiate(enemyStats.prefab, spawnPosition, Quaternion.identity);
                enemy.GetComponent<EnemyBehaviour>().stats = enemyStats;
                _currentEnemies.Add(enemy);
                enemy.GetComponent<EnemyBehaviour>().OnDeath += OnEnemyDeath;
                yield return new WaitForSeconds(1f);
            }
        }

        _isSpawning = false;
    }

    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
    }

    void OnEnemyDeath(GameObject enemy)
    {
        _currentEnemies.Remove(enemy);
    }

    void GiveRewards()
    {
        FindObjectOfType<RewardSystem>()?.TriggerRewards();
    }

    void LoadWave()
    {
        _saveLoadSystem.LoadGame(out _currentWaveIndex);
        _currentWaveIndex--;
        StartWave();
    }

    public void OnPlayerDeath()
    {
        RestartWave();
    }

    public void RestartWave()
    {
        StopAllCoroutines();
        
        foreach (var enemy in _currentEnemies)
        {
            Destroy(enemy);
        }
        
        _currentEnemies.Clear();
        
        StartWave();
    }
}
