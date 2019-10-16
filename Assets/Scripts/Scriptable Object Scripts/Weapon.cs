using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {Kinetic, Missile, Beam, Energy, Hybrid}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Equipment {
	
	public new EquipmentSlot equipSlot = EquipmentSlot.Weapons;
	public List<WeaponType> weaponParts = new List<WeaponType>();
	
	
	
}
