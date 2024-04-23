using Survival;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [HideInInspector] public ShopItemSo so;
    public Button button;
    public Image icon;

    public void Use()
    {
        if (so.type == ItemType.Consumable)
        {
            GameObject.FindWithTag("Player").GetComponent<SurvivalAttributes>().currentHunger += so.typeValueRestore;
        }
        else if (so.type == ItemType.Healable)
        {
            GameObject.FindWithTag("Player").GetComponent<AttributesManager>().health.Amount += so.typeValueRestore;
        }
    }
}
