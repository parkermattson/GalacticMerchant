using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketScreenScript : MonoBehaviour {

    public GameObject buyBoxPrefab;
    public GameObject sellBoxPrefab;
    public GameObject marketBuyBox;
    public GameObject marketSellBox;
    Inventory inventory;
    public MarketStockTable marketStock;
    List<ItemStack> buyList;
    public GameControl gameControl;

    private void Start()
    {
        inventory = Inventory.instance;
    }
    private void OnEnable()
    {
        inventory = Inventory.instance;
        GenerateStock();
        UpdateMarketStore();
    }

    public void BuyItem(GameObject itemBox)
    {
        gameControl.playerMoney -= itemBox.GetComponent<MarketSlot>().itemStack.item.itemValue;
        inventory.AddItem(itemBox.GetComponent<MarketSlot>().itemStack);
        itemBox.GetComponent<MarketSlot>().itemStack.quantity--;
        UpdateMarketStore();
    }

    public void SellItem(GameObject itemBox)
    {
        gameControl.playerMoney += itemBox.GetComponent<MarketSlot>().itemStack.item.itemValue;
        ItemStack tempStack = itemBox.GetComponent<MarketSlot>().itemStack;
        tempStack.quantity = 1;
        inventory.RemoveItem(tempStack);
        buyList.Find(x => x.item == itemBox.GetComponent<MarketSlot>().itemStack.item).quantity++;
        UpdateMarketStore();
    }

    public void UpdateMarketStore()
    {
        GameObject tempBox;
        inventory = Inventory.instance;
        for (int i = 0; i < marketBuyBox.transform.childCount; i++)
        {
            Destroy(marketBuyBox.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < buyList.Count; i++)
        {
            tempBox = Instantiate(buyBoxPrefab, marketBuyBox.transform);
            tempBox.GetComponent<MarketSlot>().AddItemStack(buyList[i]);
            tempBox.GetComponent<MarketBuySellButton>().SetMksScript(this);
            if (gameControl.playerMoney < buyList[i].item.itemValue) tempBox.GetComponentInChildren<Button>().interactable = false;
        }

        for (int i = 0; i < marketSellBox.transform.childCount; i++)
        {
            Destroy(marketSellBox.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < inventory.items.Count; i++)
        {
            tempBox = Instantiate(sellBoxPrefab, marketSellBox.transform);
            tempBox.GetComponent<MarketSlot>().AddItemStack(inventory.items[i]);
            tempBox.GetComponent<MarketBuySellButton>().SetMksScript(this);
        }
    }

    void GenerateStock()
	{
       buyList = marketStock.GenerateStock();
	}
}
