using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public int wave;
    public List<string> unlockedAbilities;

    public PlayerData(int wave, List<string> unlockedAbilities)
    {
        this.wave = wave;
        this.unlockedAbilities = unlockedAbilities;
    }
}