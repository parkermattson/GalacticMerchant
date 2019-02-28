using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot {Navigation, Weapons, Comms, Engine}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {
	
	public EquipmentSlot equipSlot = EquipmentSlot.Navigation;
	
	public int equipTier = 1;
}


	
