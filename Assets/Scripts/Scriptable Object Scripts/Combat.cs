using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType{Kinetic, Missile, Beam, Energy, Hybrid}
public enum CombatType{Weapon, Shield, PassiveBoost, ActiveBoost, DmgControl}
public enum CombatBoostType{Power, Speed, Cooldown, HitChance, Dodge}
public enum CombatChargeType{HoldClick, TapCharge, SpamClick, InstantCharge}

[CreateAssetMenu(fileName = "New Combat", menuName = "Inventory/Combat")]
public class Combat : Equipment {
	
	public Combat() {
		equipSlot = EquipmentSlot.Combat;
	}
	
	public WeaponType weaponType = WeaponType.Kinetic;
	public CombatType combatType = CombatType.Weapon;
	public List<CombatBoostType> boostTypes;
	public CombatChargeType chargeType;
	public List<WeaponType> shieldTypes;
	public List<int> boostAmt;
	public int power = 1, speed = 1, cooldown = 1;
	
	public int GetPassiveBoost(CombatBoostType boost)
	{
		if (combatType == CombatType.PassiveBoost)
		{
			for (int i = 0; i < boostTypes.Count; i++)
			{
				if (boostTypes[i] == CombatBoostType.Cooldown)
				{
					return boostAmt[i];
				}
			}
		}
		return 0;
	}
	
	public int GetActiveBoost(CombatBoostType boost)
	{
		if (combatType == CombatType.ActiveBoost)
		{
			for (int i = 0; i < boostTypes.Count; i++)
			{
				if (boostTypes[i] == CombatBoostType.Cooldown)
				{
					return boostAmt[i];
				}
			}
		}
		return 0;
	}
}
