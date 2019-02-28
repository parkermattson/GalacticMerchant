using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSlot : MonoBehaviour {

	public Image icon;
	
	public Equipment equipment;
	
	public void AddEquipment (Equipment newEquip)
	{
		equipment = newEquip;
		
		icon.sprite = equipment.icon;
		icon.enabled = true;
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
