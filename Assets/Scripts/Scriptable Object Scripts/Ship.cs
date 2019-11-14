using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Inventory/Ship")]
public class Ship : ScriptableObject {

	public string shipName = "Ship Name";
	public string shipDesc = "Ship description";
	public Sprite graphic = null;
	public int price = 1000, maxHull = 10, currentHull = 10, maxFuel = 100, currentFuel = 100, maxCargo = 1000,  rawSensorRange = 20, rawWarpRange = 350;
	public float rawFuelEff = 1, rawSpeed = 1;
	public int[] equipCapacities = {1, 1, 1, 1};
	public List<Equipment> commandList = new List<Equipment>(), sensorList = new List<Equipment>(), engineList = new List<Equipment>();
	public List<Weapon> weaponsList = new List<Weapon>();
	
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
	
	public float GetRawFuelEff() {
		return rawFuelEff;
	}
	
	public int GetRawSensorRange() {
		return rawSensorRange;
	}
	
	public float GetRawSpeed() {
		return rawSpeed;
	}
	
	public float GetNetFuelEff() {
		Debug.Log("Need net fuel eff calculation");
		return rawFuelEff;
	}
	
	public float GetNetSensorRange() {
		Debug.Log("Need net sensor range calculation");
		return rawSensorRange;
	}
	
	public float GetNetWarpRange() {
		Debug.Log("Need net warp range calculation");
		return rawWarpRange;
	}
	
	public float GetNetSpeed() {
		Debug.Log("Need net speed calculation");
		return rawSpeed;
	}
	
}
