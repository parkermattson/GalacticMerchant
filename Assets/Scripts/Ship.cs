using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Inventory/Ship")]
public class Ship : ScriptableObject {

	public string shipName = "Ship Name";
	public string shipDesc = "Ship description";
	public Sprite icon = null;
	public int price = 1000;
	public int maxHull = 10;
	public int maxFuel = 100;
	public int maxCargo = 1000;
	public float rawFuelEff = 1;
	public int rawSensorRange = 20;
	
	public string getName() {
		return shipName;
	}
	
	public string getDesc() {
		return shipDesc;
	}
	
	public int getHullMax() {
		return maxHull;
	}
	
	public int getFuelMax() {
		return maxFuel;
	}
	
	public int getCargoMax() {
		return maxCargo;
	}
	
	public float getFuelEff() {
		return rawFuelEff;
	}
	
	public int getSensor() {
		return rawSensorRange;
	}
	
}
