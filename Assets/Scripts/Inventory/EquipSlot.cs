using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSlot : MonoBehaviour {

	public Image icon;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI typeText;
	public TextMeshProUGUI tierText;
	public TextMeshProUGUI descText;
	public TextMeshProUGUI priceText;
	
	public Equipment equipment;
	
	public void AddEquipment (Equipment newEquip)
	{
		equipment = newEquip;
		
		icon.sprite = equipment.icon;
		icon.enabled = true;
		nameText.text = equipment.GetName();
		if (typeText != null)
			typeText.text = "Type: " + equipment.GetSlotType();
		if (tierText != null)
			tierText.text = "Tier: " + equipment.GetTier();
		if (descText != null)
			descText.text = "Description: " + equipment.GetDescription();
		if (priceText != null)
			priceText.SetText("Price: {0}",equipment.GetValue());
	}
	
	public void ClearSlot ()
	{
		equipment = null;
		
		icon.sprite=null;
		icon.enabled = false;
	}
	
	public Equipment GetEquipment()
	{
		return equipment;
	}
}
