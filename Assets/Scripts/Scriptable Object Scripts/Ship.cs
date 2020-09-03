using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ShipStatBuffs {MaxHull, MaxFuel, MaxCargo, SensorRange, WarpRange, FuelEff, Speed}

[CreateAssetMenu(fileName = "New Ship", menuName = "Inventory/Ship")]
public class Ship : ScriptableObject {

	public string shipName = "Ship Name";
	public string shipDesc = "Ship description";
	public Sprite graphic = null;
	public int price = 1000, maxHull = 10, currentHull = 10, maxFuel = 100, currentFuel = 100, maxCargo = 1000,  rawSensorRange = 20, rawWarpRange = 350;
	public float rawFuelEff = 1, rawSpeed = 1;
	public int[] equipCapacities = {1, 1, 1, 1};
	public List<Command> commandList;
	public List<Sensor> sensorList;
	public List<Engine> engineList;
	public List<Combat> combatList;
	
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
	
	public float GetNetFuelEff(bool isPlayer) {
		float engineEff = 0, engineBonus = 0;
		for (int i = 0; i < engineList.Count; i++)
		{
			engineEff += engineList[i].GetFuelEfficiency();
		}
		for (int i = 0; i < commandList.Count; i++)
		{
			engineBonus += commandList[i].GetBonus(CommandBonus.FuelEff);
		}
		if (isPlayer)
			engineBonus += GameControl.instance.GetFuelEfficiencyBonus();
		
		float netEff = (rawFuelEff + engineEff) * (1 + engineBonus);
		return netEff;
	}
	
	public float GetNetSensorRange(bool isPlayer) {
		float sensorRange = 0, sensorBonus = 0;
		for (int i = 0; i < sensorList.Count; i++)
		{
			sensorRange += sensorList[i].GetSensorRange();
		}
		for (int i = 0; i < commandList.Count; i++)
		{
			sensorBonus += commandList[i].GetBonus(CommandBonus.SensorRange);
		}
		if (isPlayer)
			sensorBonus += GameControl.instance.GetSensorRangeBonus();
		
		float netRange = (rawSensorRange + sensorRange) * (1 + sensorBonus);
		return netRange;
	}
	
	public int GetSensorLevel(bool isPlayer) {
		int level = 0;
		for (int i = 0; i < sensorList.Count; i++)
		{
			if (isPlayer)
				level = Mathf.Max(level, sensorList[i].GetSensorLevel() + GameControl.instance.GetSensorLevelBonus());
			else
				level = Mathf.Max(level, sensorList[i].GetSensorLevel());
		}
		return level;
	}
	
	public float GetNetWarpRange(bool isPlayer) {
		float engineRange = 0, warpBonus = 0;
		for (int i = 0; i < engineList.Count; i++)
		{
			engineRange += engineList[i].GetWarpRange();
		}
		for (int i = 0; i < commandList.Count; i++)
		{
			warpBonus += commandList[i].GetBonus(CommandBonus.WarpRange);
		}
		if (isPlayer)
			warpBonus += GameControl.instance.GetWarpRangeBonus();
		
		float netWarpRange = (rawWarpRange + engineRange) * (1 + warpBonus);
		return netWarpRange;
	}
	
	public float GetNetSpeed(bool isPlayer) {
		float engineSpeed = 0, speedBonus = 0;
		for (int i = 0; i < engineList.Count; i++)
		{
			engineSpeed += engineList[i].GetWarpSpeed();
		}
		for (int i = 0; i < commandList.Count; i++)
		{
			speedBonus += commandList[i].GetBonus(CommandBonus.WarpSpeed);
		}
		if (isPlayer)
			speedBonus += GameControl.instance.GetWarpSpeedBonus();
		float netSpeed = (rawSpeed + engineSpeed) * (1 + speedBonus);
		return netSpeed;
	}
	
	public List<Combat> GetWeapons() {
		List<Combat> guns = new List<Combat>();
		for (int i = 0; i < combatList.Count; i++)
		{
			if (combatList[i].isWeapon)
				guns.Add(combatList[i]);
		}
		return guns;
	}
	
	public List<Combat> GetDefenses() {
		List<Combat> defenses = new List<Combat>();
		for (int i = 0; i < combatList.Count; i++)
		{
			if (!combatList[i].isWeapon)
				defenses.Add(combatList[i]);
		}
		return defenses;	
	}
	
	/*public int GetWeaponPower(WeaponType type) {
		int power = 0, numWeaps = 0;
		for (int i = 0; i < weapon.weaponParts.Count; i++)
			if (weapon.weaponParts[i] == type)
			{
				power += weapon.weaponPower[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return power / numWeaps;
		else return 0;
	}
	
	public float GetWeaponSpeed(WeaponType type) {
		float speed = 0, numWeaps = 0;
		for (int i = 0; i < weapon.weaponParts.Count; i++)
			if (weapon.weaponParts[i] == type)
			{
				speed += weapon.weaponSpeed[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return speed *(1 + GameControl.instance.GetWeaponChargeBonus() + command.GetBonus(CommandBonus.WeaponSpeed)) / numWeaps;
		else return 0;
	}
	
	public float GetWeaponCooldown(WeaponType type) {
		float cooldown = 0, numWeaps = 0;
		for (int i = 0; i < weapon.weaponParts.Count; i++)
			if (weapon.weaponParts[i] == type)
			{
				cooldown += weapon.weaponCooldown[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return cooldown * (1 + GameControl.instance.GetWeaponCooldownBonus() + command.GetBonus(CommandBonus.WeaponCooldown)) / numWeaps;
		else return 0;
	}
	
	public int GetDefensePower(WeaponType type) {
		int power = 0, numWeaps = 0;
		for (int i = 0; i < weapon.defenseParts.Count; i++)
			if (weapon.defenseParts[i] == type)
			{
				power += weapon.defensePower[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return power / numWeaps;
		else return 0;
	}
	
	public float GetDefenseSpeed(WeaponType type) {
		float speed = 0, numWeaps = 0;
		for (int i = 0; i < weapon.defenseParts.Count; i++)
			if (weapon.defenseParts[i] == type)
			{
				speed += weapon.defenseSpeed[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return speed  * (1 + GameControl.instance.GetDefenseDurationBonus() + command.GetBonus(CommandBonus.DefenseDuration)) / numWeaps;
		else return 0;
	}
	
	public float GetDefenseCooldown(WeaponType type) {
		float cooldown = 0, numWeaps = 0;
		for (int i = 0; i < weapon.defenseParts.Count; i++)
			if (weapon.defenseParts[i] == type)
			{
				cooldown += weapon.defenseCooldown[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return cooldown  * (1 + GameControl.instance.GetDefenseCooldownBonus() + command.GetBonus(CommandBonus.DefenseCooldown))/ numWeaps;
		else return 0;
	}
	*/
	public void AddHealth(int hpAdded) {
		currentHull += hpAdded;
		if (currentHull > maxHull)
			currentHull = maxHull;
		
		if (currentHull < 1)
		{
			currentHull = maxHull;
			Debug.Log("Game Over. Ran out of health");
		}
	}
	
}
