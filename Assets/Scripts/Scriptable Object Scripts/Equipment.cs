﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot {Command, Weapons, Sensors, Engine}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {
	
	public Equipment() {
		itemType = ItemType.Equipment;
	}
	
	public EquipmentSlot equipSlot = EquipmentSlot.Command;
	
	public EquipmentSlot GetSlotType()
	{
		return equipSlot;
	}
	
	public string GetTypeName()
	{
		if (equipSlot == EquipmentSlot.Command) return "Command";
		else if (equipSlot == EquipmentSlot.Weapons) return "Weapons";
		else if (equipSlot == EquipmentSlot.Sensors) return "Sensors";
		else return "Engine";
	}
}


	
