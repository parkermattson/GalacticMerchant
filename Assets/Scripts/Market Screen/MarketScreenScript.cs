using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketScreenScript : MonoBehaviour {

	Inventory inventory;
	GameControl gameControl;
    
	public TextMeshProUGUI playerMoneyText, merchantMoneyText;
	public GameObject buyBoxPrefab;
    public GameObject sellBoxPrefab;
    public GameObject marketBuyBox;
    public GameObject marketSellBox;
    
    List<ItemStack> buyList;
    
	

    private void Awake()
    {
        inventory = Inventory.instance;
		gameControl = GameControl.instance;
    }
    private void OnEnable()
    {
        GenerateStock();
        UpdateMarketStore();
    }

    public void BuyItem(GameObject itemBox)
    {
        gameControl.playerMoney -= itemBox.GetComponent<MarketSlot>().itemStack.GetItem().itemValue;
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
        inventory.AddItem(tempStack.Init(itemBox.GetComponent<MarketSlot>().itemStack.GetItem(), 1));
        int index = buyList.FindIndex(x => x.GetItem() == itemBox.GetComponent<MarketSlot>().itemStack.GetItem());
		if (index != -1)
		{
			if (buyList[index].GetQuantity() > 1)
			{
				buyList[index].AddQuantity(-1);
			}
			else buyList.RemoveAt(index);
		}
        UpdateMarketStore();
    }

    public void SellItem(GameObject itemBox)
    {
        gameControl.playerMoney += itemBox.GetComponent<MarketSlot>().itemStack.GetItem().itemValue;
        ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
        inventory.RemoveItem(tempStack.Init(itemBox.GetComponent<MarketSlot>().itemStack.GetItem(), 1));
		if (buyList.Exists(x => x.GetItem() == itemBox.GetComponent<MarketSlot>().itemStack.GetItem()))
		{
			buyList.Find(x => x.GetItem() == itemBox.GetComponent<MarketSlot>().itemStack.GetItem()).AddQuantity(itemBox.GetComponent<MarketSlot>().itemStack.GetQuantity());
		}
		else {
			ItemStack tempStack2 = ScriptableObject.CreateInstance<ItemStack>();
			buyList.Add(tempStack2.Init(itemBox.GetComponent<MarketSlot>().itemStack.GetItem(), itemBox.GetComponent<MarketSlot>().itemStack.GetQuantity()));
		}
        UpdateMarketStore();
    }

    public void UpdateMarketStore()
    {
		playerMoneyText.text = "Your Money: " + gameControl.playerMoney.ToString();
		
        GameObject tempBox;
        for (int i = 0; i < marketBuyBox.transform.childCount; i++)
        {
            Destroy(marketBuyBox.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < buyList.Count; i++)
        {
            tempBox = Instantiate(buyBoxPrefab, marketBuyBox.transform);
            tempBox.GetComponent<MarketSlot>().AddItemStack(buyList[i]);
            tempBox.GetComponent<MarketBuySellButton>().SetMksScript(this);
            if (gameControl.playerMoney < buyList[i].GetItem().itemValue) tempBox.GetComponentInChildren<Button>().interactable = false;
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
		Station station = (Station)gameControl.playerLocation;
       buyList = station.GetStockTable().GenerateStock();
	}
}
