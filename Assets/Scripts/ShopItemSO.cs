using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Objects/New Shop Item", order = 1)]
public class ShopItemSo : ScriptableObject
{
    public Sprite icon;
    public Sprite render;
    public string title;
    public string description;
    public int baseCost;
    public int maxLv;

    public ItemType type;
    public float typeValueRestore;
}

public enum ItemType
{
    Consumable,
    Healable
}

