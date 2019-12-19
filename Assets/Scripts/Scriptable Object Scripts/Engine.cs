using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Inventory/Engine")]
public class Engine : Equipment {

	public Engine() {
		equipSlot = EquipmentSlot.Engine;
	}
	
	public int warpRange = 400;
	public float warpSpeed = 1, fuelEfficiency = 1, encounterBonus = 0;
	
	public int GetWarpRange() {
		return warpRange;
	}
	
	public float GetWarpSpeed() {
		return warpSpeed;
	}
	
	public float GetFuelEfficiency() {
		return fuelEfficiency;
	}
	
	public float GetEncounterBonus() {
		return encounterBonus;
	}
	
}
