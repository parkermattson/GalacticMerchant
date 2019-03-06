using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipSlot : MonoBehaviour {

	public Image icon;
	public GameObject nameText;
	public GameObject typeText;
	public GameObject tierText;
	
	public Equipment equipment;
	
	public void AddEquipment (Equipment newEquip)
	{
		equipment = newEquip;
		
		icon.sprite = equipment.icon;
		icon.enabled = true;
		nameText.GetComponent<TextMeshProUGUI>().SetText(equipment.itemName);
		typeText.GetComponent<TextMeshProUGUI>().text = "Type: " + equipment.equipSlot;
		tierText.GetComponent<TextMeshProUGUI>().text = "Tier: " + equipment.equipTier;
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
