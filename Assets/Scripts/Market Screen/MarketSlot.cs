using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketSlot : MonoBehaviour {

    public Image icon;
    public TextMeshProUGUI nameText, priceText, quantityText, descText, tierText, typeText;
	public TMP_InputField quantityInput;
	public Button buySellButton;
    public ItemStack itemStack;
	public float price = 1;
	public int slotQuant = 1;
	bool isBuySlot = false;
	Station station;
    
    public void AddItemStack(ItemStack newItem, Station newStation, bool buySlot)
    {
		station = newStation;
        itemStack = newItem;
		isBuySlot = buySlot;
		CheckQuant();
		
        icon.sprite = itemStack.GetItem().icon;
        icon.enabled = true;
        nameText.text = itemStack.GetItem().GetName();
		quantityText.text = itemStack.GetQuantity().ToString();
        if (typeText != null)
            typeText.text = "Type: " + itemStack.GetItem().itemType.ToString();
        if (tierText != null)
            tierText.text = "Tier: " + itemStack.GetItem().GetTier();
        if (descText != null)
            descText.text = "Description: " + itemStack.GetItem().GetDescription();
        if (priceText != null)
            priceText.SetText("{0}(" + price.ToString("#.00") + ")", price * slotQuant);
    }
    
    public ItemStack GetItemStack()
    {
        return itemStack;
    }
	
	public void CheckQuant()
	{
		if (!int.TryParse(quantityInput.text, out slotQuant))
		{
			quantityInput.text = "0";
			slotQuant = 0;
		}
		if (slotQuant < 0)
		{
			slotQuant = 0;
			quantityInput.text = "0";
		} else if (slotQuant > itemStack.GetQuantity())
		{
				slotQuant = itemStack.GetQuantity();
				quantityInput.text = slotQuant.ToString();
		}
		UpdatePrice();
	}
	
	public void UpdatePrice()
	{
		if (!isBuySlot)
		{
			int index = station.marketInv.FindIndex(x => x.GetItem() == itemStack.GetItem());
			if (index != -1)
				price = station.priceTable.AdjustPrice(station.marketInv[index], slotQuant);
			else price = station.priceTable.AdjustPrice(itemStack, slotQuant);
			price *=.9f;
		} else price = station.priceTable.AdjustPrice(itemStack, -slotQuant);
		
		if (priceText != null)
            priceText.SetText("{0}(" + price.ToString("#.00") + ")", price * slotQuant);
		
		if (isBuySlot)
		{
			if (price*slotQuant > GameControl.instance.playerMoney)
			{
				buySellButton.interactable = false;
				priceText.color = Color.red;
			} else {
				buySellButton.interactable = true;
				priceText.color = Color.black;
			}
		}  else {
			if (price*slotQuant > station.stationMoney)
			{
				buySellButton.interactable = false;
				priceText.color = Color.red;
			} else {
				buySellButton.interactable = true;
				priceText.color = Color.black;
			}
		}
	}
	
	public void PlusMinusButton(bool plus)
	{
		int addQuant = 1;
		if (plus)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				addQuant *= 10;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				addQuant *= 100;
			}
		} else 
		{
			addQuant *= -1;
			if (Input.GetKey(KeyCode.LeftControl))
			{
				addQuant *= 10;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				addQuant *= 100;
			}
		}
		
		slotQuant += addQuant;
		quantityInput.text = slotQuant.ToString();
		CheckQuant();
	}
}
