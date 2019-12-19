using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandBonus {WarpRange, WarpSpeed, FuelEff, SensorRange, WeaponSpeed, WeaponCooldown, DefenseDuration, DefenseCooldown, QuestMoney}

[CreateAssetMenu(fileName = "New Command", menuName = "Inventory/Command")]
public class Command : Equipment {
	
	public Command() {
		equipSlot = EquipmentSlot.Command;
	}
	
	public List<CommandBonus> bonusTypes = new List<CommandBonus>();
	public List<float> bonusMults = new List<float>();
	
	public float GetBonus(CommandBonus desiredBonusType) {
		for (int i = 0; i < bonusTypes.Count; i++)
		{
			if (bonusTypes[i] == desiredBonusType)
			{
				return bonusMults[i];
			}
		}
		return 0;
	}
	
	
}
