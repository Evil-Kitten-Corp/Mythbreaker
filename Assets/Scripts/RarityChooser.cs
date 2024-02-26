using Ability_Behaviours;
using BrunoMikoski.ScriptableObjectCollections;
using UnityEngine;

public class RarityChooser: ScriptableObjectCollectionItem
{
    public string ItemName { get; }
    public string ItemDescription { get; }
    public Sprite ItemImg { get; }
    public Sprite ItemIcon { get; }
    public Rarities Rarity { get; }
}