using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {
	
	public EquipmentSlot equipSlot = 0;
	
	public int equipTier = 1;
}

public enum EquipmentSlot {Navigation, Weapons, Comms, Engine}

	
