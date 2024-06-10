using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FinalScripts;
using FinalScripts.Specials;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public HealthPowerup healthPrefab;
    public Transform healthSpawnPt;
    
    public WaveData[] waves;
    public TMP_Text waveAnnouncer;
    
    public float delayBetweenWaves;
    
    private float _countdownTimer;
    private int _currentWaveIndex;
    
    private readonly List<GameObject> _currentEnemies = new();
    
    private bool _isSpawning;
    private bool _isEndable;
        
    private SaveLoadJsonFormatter _saveLoadSystem;
    private PlayerInv _inv;
    
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
        _saveLoadSystem.SaveGame(new PlayerData(_currentWaveIndex, 
            _inv.abilities.Keys.Select(ab => ab.id).ToList()));

        Instantiate(healthPrefab, healthSpawnPt);
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
            waveAnnouncer.gameObject.SetActive(false);
            AttributesManager.OnDefeatLastWave?.Invoke();
            return;
        }
        
        StartCoroutine(SpawnEnemies(waves[_currentWaveIndex]));
    }

    IEnumerator SpawnEnemies(WaveData wave)
    {
        _isSpawning = true;

        foreach (var entry in wave.spawnData)
        {
            EnemyStats enemyStats = entry.enemyType;
            int count = entry.amountToSpawn;

            for (int i = 0; i < count; i++)
            {
                Vector3 spawnPosition = entry.spawnPoint;
                GameObject enemy = Instantiate(enemyStats.prefab, spawnPosition, Quaternion.identity);
                enemy.GetComponent<EnemyBehaviour>().stats = enemyStats;
                _currentEnemies.Add(enemy);
                enemy.GetComponent<EnemyBehaviour>().OnDeath += OnEnemyDeath;
                yield return new WaitForSeconds(entry.delayBetweenSpawns);
            }
        }

        _isSpawning = false;
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
        StartCountdown();
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
