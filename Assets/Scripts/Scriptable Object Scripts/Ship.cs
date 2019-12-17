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
	public Equipment command, sensor, engine;
	public Weapon weapon;
	
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
		float netEff = rawFuelEff * GameControl.instance.GetFuelEfficiencyBonus();
		return netEff;
	}
	
	public float GetNetSensorRange() {
		float netRange = rawSensorRange * GameControl.instance.GetSensorRangeBonus();
		return netRange;
	}
	
	public int GetSensorLevel() {
		int level = 1 + GameControl.instance.GetSensorLevelBonus();
		return level;
	}
	
	public float GetNetWarpRange() {
		float netWarpRange = rawWarpRange * GameControl.instance.GetWarpRangeBonus();
		return netWarpRange;
	}
	
	public float GetNetSpeed() {
		float netSpeed = rawSpeed * GameControl.instance.GetWarpSpeedBonus();
		return netSpeed;
	}
	
	public int GetWeaponPower(WeaponType type) {
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
			return speed * GameControl.instance.GetWeaponChargeBonus() / numWeaps;
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
			return cooldown * GameControl.instance.GetWeaponCooldownBonus() / numWeaps;
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
			return speed  * GameControl.instance.GetDefenseDurationBonus() / numWeaps;
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
			return cooldown  * GameControl.instance.GetDefenseCooldownBonus() / numWeaps;
		else return 0;
	}
	
}
