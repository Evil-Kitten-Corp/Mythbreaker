using Ability_Behaviours;

public static class Utils
{
    public static float GetChance(this Rarities rarity)
    {
        return rarity switch
        {
            Rarities.Normal => 0.70f,
            Rarities.Rare => 0.20f,
            Rarities.SuperRare => 0.8f,
            Rarities.Ultra => 0.2f,
            _ => 0
        };
    }
}