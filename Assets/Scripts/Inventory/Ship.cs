using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Inventory/Ship")]
public class Ship : ScriptableObject {

	public string shipName = "Ship Name";
	public string shipDesc = "Ship description";
	public Sprite graphic = null;
	public int price = 1000;
	public int maxHull = 10;
	public int maxFuel = 100;
	public int maxCargo = 1000;
	public float rawFuelEff = 1;
	public int rawSensorRange = 20;
	public float rawSpeed = 1;
	
	public string GetName() {
		return shipName;
	}
	
	public string GetDesc() {
		return shipDesc;
	}
	
	public int GetPrice()
	{
		return price;
	}
	
	public int GetHullMax() {
		return maxHull;
	}
	
	public int GetFuelMax() {
		return maxFuel;
	}
	
	public int GetCargoMax() {
		return maxCargo;
	}
	
	public float GetFuelEff() {
		return rawFuelEff;
	}
	
	public int GetRange() {
		return rawSensorRange;
	}
	
	public float GetSpeed() {
		return rawSpeed;
	}
	
}
