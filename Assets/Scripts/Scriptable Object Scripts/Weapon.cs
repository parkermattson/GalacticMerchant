using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {Kinetic, Missile, Beam, Energy, Hybrid}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Equipment {
	
	public Weapon() {
		equipSlot = EquipmentSlot.Weapons;
	}
	
	public List<WeaponType> weaponParts = new List<WeaponType>(), defenseParts = new List<WeaponType>();
	public List<int> weaponPower = new List<int>(), defensePower = new List<int>(), weaponSpeed = new List<int>(), defenseSpeed = new List<int>(), weaponCooldown = new List<int>(), defenseCooldown = new List<int>();
	
	
	
}
