using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot {Navigation, Weapons, Comms, Engine}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {
	
	public EquipmentSlot equipSlot = EquipmentSlot.Navigation;
	
	public int equipTier = 1;
	
	public EquipmentSlot GetSlotType()
	{
		return equipSlot;
	}
	
	public string GetTypeName()
	{
		if (equipSlot == EquipmentSlot.Navigation) return "Navigation";
		else if (equipSlot == EquipmentSlot.Weapons) return "Weapons";
		else if (equipSlot == EquipmentSlot.Comms) return "Comms";
		else return "Engine";
	}
	
	public int GetTier()
	{
		return equipTier;
	}
}


	
