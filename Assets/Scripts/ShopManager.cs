using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Tooltip("Debug")] public int coins;
    public TMP_Text coinUI;
    public LayoutGroup layoutGroup;
    public GameObject shopPanelPrefab;
    public ShopItemSo[] shopItemsSO;
    public InventoryWindow inventory;

    [Header("Previewer")] 
    public ShopPreviewer previewer;
    
    
    public List<ShopPanel> shopPanels; 
    public List<Button> myPurchaseBtns;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            var go = Instantiate(shopPanelPrefab, layoutGroup.transform);
            shopPanels.Add(go.GetComponent<ShopPanel>());
            myPurchaseBtns.Add(go.GetComponent<Button>());
        }
        
        coinUI.text = coins.ToString();
        LoadPanels();
        SetUpButtons();
        CheckPurchasable();
    }

    public void AddCoins() // simple script to add coins.
    {
        coins++;
        coinUI.text = coins.ToString();
        CheckPurchasable();
    }

    private void SetUpButtons()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            var i1 = i;
            myPurchaseBtns[i].onClick.AddListener(() => GetSelected(shopPanels[i1]));
        }
    }

    private void CheckPurchasable()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            myPurchaseBtns[i].interactable = coins >= shopItemsSO[i].baseCost;
        }
    }

    public void PurchaseItem(int btnNo)
    {
        if (coins >= shopItemsSO[btnNo].baseCost)
        {
            coins -= shopItemsSO[btnNo].baseCost;
            coinUI.text = coins.ToString();
            CheckPurchasable();
            //Unlock Item.
        }
    }

    public void PurchaseSelected()
    {
        switch (previewer.isActiveAndEnabled)
        {
            case true when coins >= previewer.so.baseCost:
                coins -= previewer.so.baseCost;
                inventory.AddItem(previewer.so); 
                coinUI.text = coins.ToString();
                CheckPurchasable();
                //Unlock Item.
                break;
        }
    }

    private void LoadPanels()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].so = shopItemsSO[i];
            shopPanels[i].titleTxt.text = shopItemsSO[i].title;
            shopPanels[i].icon.sprite = shopItemsSO[i].icon;
            shopPanels[i].costTxt.text = shopItemsSO[i].baseCost.ToString();
        }
    }

    private void GetSelected(ShopPanel panel)
    {
        previewer.gameObject.SetActive(true);
        previewer.so = panel.so;
        previewer.title.text = panel.so.title;
        previewer.description.text = panel.so.description;
        previewer.render.sprite = panel.so.render;
    }
}
