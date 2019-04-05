using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Stock", menuName = "LootTable/EquipmentStock")]
public class EquipmentStockTable : ScriptableObject {
	
	public List<Equipment> equipment;
	public List<float> availableChance;
	
	public List<Equipment> GetEquipmentList() {
		return equipment;
	}
	
	public List<float> GetChance() {
		return availableChance;
	}
}
