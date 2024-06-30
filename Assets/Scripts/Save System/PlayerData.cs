[System.Serializable]
public class PlayerData
{
    public int wave;
    public float speed;
    public float health;
    public int attack;

    public PlayerData(int wave, float speed, float health, int attack)
    {
        this.wave = wave;
        this.attack = attack;
        this.health = health;
        this.speed = speed;
    }
}