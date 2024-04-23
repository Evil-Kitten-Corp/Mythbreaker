using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindow : MonoBehaviour
{
    public LayoutGroup layoutGroup;
    public GameObject itemPrefab;
    public List<ShopItemSo> inventoryItemsSo = new();
    public Button consumeBtn;
    
    [Header("Previewer")] 
    public ShopPreviewer previewer;
    
    [HideInInspector] public List<InventoryItem> invItems;
    
    private void Start()
    {
        for (int i = 0; i < inventoryItemsSo.Count; i++)
        {
            var go = Instantiate(itemPrefab, layoutGroup.transform);
            invItems.Add(go.GetComponent<InventoryItem>());
        }
        
        LoadPanels();
    }

    public void Init()
    {
        Start();
    }
    
    private void LoadPanels()
    {
        for (int i = 0; i < inventoryItemsSo.Count; i++)
        { 
            invItems[i].so = inventoryItemsSo[i];
            invItems[i].icon.sprite = inventoryItemsSo[i].icon;
            var i1 = i;
            invItems[i].button.onClick.AddListener(() => GetSelected(invItems[i1]));
        }
    }  
    
    private void GetSelected(InventoryItem panel)
    {
        previewer.gameObject.SetActive(true);
        consumeBtn.gameObject.SetActive(true);
        previewer.inv = panel;
        previewer.so = panel.so;
        previewer.title.text = panel.so.title;
        previewer.description.text = panel.so.description;
        previewer.render.sprite = panel.so.render;
    }

    public void AddItem(ShopItemSo itm)
    {
        inventoryItemsSo.Add(itm);
        Init(); 
    }

    private void RemoveItem(InventoryItem itm)
    {
        inventoryItemsSo.Remove(itm.so); 
        invItems.Remove(itm);
        Destroy(itm.gameObject);
    }

    private void UseItem(InventoryItem itm)
    {
        itm.Use();
        RemoveItem(itm);
    }

    public void UseSelected()
    {
        switch (previewer.isActiveAndEnabled)
        {
            case true:
                UseItem(previewer.inv);
                break;
        }
    }
}
