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
        
        icon.sprite = itemStack.item.icon;
        icon.enabled = true;
        nameText.text = itemStack.item.GetName();
        if (typeText != null)
            typeText.text = "Type: " + itemStack.item.itemType.ToString();
        if (tierText != null)
            tierText.text = "Tier: " + itemStack.item.GetTier();
        if (descText != null)
            descText.text = "Description: " + itemStack.item.GetDescription();
        if (priceText != null)
            priceText.SetText("Price: {0}", itemStack.item.GetValue());
    }
    
    public ItemStack GetItemStack()
    {
        return itemStack;
    }
}
