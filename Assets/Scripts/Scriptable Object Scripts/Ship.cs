﻿using System.Collections;
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
		Debug.Log("Need net fuel eff calculation");
		return rawFuelEff;
	}
	
	public float GetNetSensorRange() {
		Debug.Log("Need net sensor range calculation");
		return rawSensorRange;
	}
	
	public int GetSensorLevel() {
		Debug.Log("Need sensor level getter here");
		return 1;
	}
	
	public float GetNetWarpRange() {
		Debug.Log("Need net warp range calculation");
		return rawWarpRange;
	}
	
	public float GetNetSpeed() {
		Debug.Log("Need net speed calculation");
		return rawSpeed;
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
			return power/numWeaps;
		else return 0;
	}
	
	public int GetWeaponSpeed(WeaponType type) {
		int speed = 0, numWeaps = 0;
		for (int i = 0; i < weapon.weaponParts.Count; i++)
			if (weapon.weaponParts[i] == type)
			{
				speed += weapon.weaponSpeed[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return speed/numWeaps;
		else return 0;
	}
	
	public int GetWeaponCooldown(WeaponType type) {
		int cooldown = 0, numWeaps = 0;
		for (int i = 0; i < weapon.weaponParts.Count; i++)
			if (weapon.weaponParts[i] == type)
			{
				cooldown += weapon.weaponCooldown[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return cooldown/numWeaps;
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
			return power/numWeaps;
		else return 0;
	}
	
	public int GetDefenseSpeed(WeaponType type) {
		int speed = 0, numWeaps = 0;
		for (int i = 0; i < weapon.defenseParts.Count; i++)
			if (weapon.defenseParts[i] == type)
			{
				speed += weapon.defenseSpeed[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return speed/numWeaps;
		else return 0;
	}
	
	public int GetDefenseCooldown(WeaponType type) {
		int cooldown = 0, numWeaps = 0;
		for (int i = 0; i < weapon.defenseParts.Count; i++)
			if (weapon.defenseParts[i] == type)
			{
				cooldown += weapon.defenseCooldown[i];
				numWeaps++;
			}
		if (numWeaps > 0)
			return cooldown/numWeaps;
		else return 0;
	}
	
}
