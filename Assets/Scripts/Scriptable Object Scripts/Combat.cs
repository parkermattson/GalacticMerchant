using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType{Kinetic, Missile, Beam, Energy, Hybrid}
public enum DefenseType{Shield, PassiveBoost, ActiveBoost, DmgControl}

[CreateAssetMenu(fileName = "New Combat", menuName = "Inventory/Combat")]
public class Combat : Equipment {
	
	public Combat() {
		equipSlot = EquipmentSlot.Combat;
	}
	
	public bool isWeapon = true;
	public WeaponType weaponType = WeaponType.Kinetic;
	public int power = 1, speed = 1, cooldown = 1;
	
}
