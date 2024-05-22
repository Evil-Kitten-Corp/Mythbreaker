using System.IO;
using UnityEngine;

public class SaveLoadJsonFormatter : MonoBehaviour
{
    PlayerData _playerData;
    string _saveFilePath;
 
    void Start()
    {
        _playerData = new PlayerData(0);
        _saveFilePath = Application.persistentDataPath + "/PlayerData.json";
    }

    public void SaveGame(PlayerData data)
    {
        string saveData = JsonUtility.ToJson(_playerData);
        File.WriteAllText(_saveFilePath, saveData);
        Debug.Log("Save file created at: " + _saveFilePath);
    }
 
    public void LoadGame(out int currentWaveIndex)
    {
        if (File.Exists(_saveFilePath))
        {
            string loadPlayerData = File.ReadAllText(_saveFilePath);
            _playerData = JsonUtility.FromJson<PlayerData>(loadPlayerData);
 
            Debug.Log("Load game complete!");
            currentWaveIndex = _playerData.wave;
        }
        else
            Debug.Log("There is no save files to load!");

        currentWaveIndex = 0;
    }
 
    public void DeleteSaveFile()
    {
        if (File.Exists(_saveFilePath))
        {
            File.Delete(_saveFilePath);
 
            Debug.Log("Save file deleted!");
        }
        else
            Debug.Log("There is nothing to delete!");
    }
}