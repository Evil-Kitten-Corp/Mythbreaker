using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPreviewer : MonoBehaviour
{
    [HideInInspector] public ShopItemSo so;
    [HideInInspector] public InventoryItem inv;
    public TMP_Text title;
    public TMP_Text description;
    public Image render;
}