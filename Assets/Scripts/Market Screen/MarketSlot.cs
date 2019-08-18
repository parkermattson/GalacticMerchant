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
    public ItemStack itemStack;
    
    public void AddItemStack(ItemStack newItem)
    {
        itemStack = newItem;
        
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
            priceText.SetText("Price: {0}", itemStack.GetItem().GetValue());
    }
    
    public ItemStack GetItemStack()
    {
        return itemStack;
    }
}
