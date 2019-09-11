using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketSlot : MonoBehaviour {

    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI tierText;
    public TextMeshProUGUI typeText;
	public TMP_InputField quantityInput;
    public ItemStack itemStack;
	public float price = 1;
	public int slotQuant = 1;
    
    public void AddItemStack(ItemStack newItem, Station station, bool buySlot)
    {
        itemStack = newItem;
        price = station.priceTable.AdjustPrice(itemStack);
		if (!buySlot) price *=.9f;
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
            priceText.SetText("{0}", price * slotQuant);
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
		if (priceText != null)
            priceText.SetText("{0}", price * slotQuant);
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
