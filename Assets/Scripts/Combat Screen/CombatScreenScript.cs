using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum EnemyAiState {Thinking, Holding, Spamming}

public class CombatScreenScript : MonoBehaviour {

	public CombatIconScript[] enemyIcons, playerIcons;
	CombatIconScript switchedPlayerAttackIcon, switchedPlayerDefenseIcon, switchedEnemyAttackIcon, switchedEnemyDefenseIcon;
	public GameObject winPopup, losePopup;
	public Transform  playerHealthBar, enemyHealthBar;
	bool combatStarted = false;
	Ship enemyShip;
	public List<Ship> enemyShipList;
	public List<Command> enemyCommandList;
	public List<Sensor> enemySensorList;
	public List<Engine> enemyEngineList;
	public List<Combat> enemyCombatList;
	public List<Item>  lootList;
	public EnemyAiState aiState = EnemyAiState.Thinking;
	int enemyClickSlot = -1, enemyTargetSlot = -1;
	float enemyThinkProgress = 0,  enemyThinkTime = .1f;
	float[] playerActiveBoosts = new float[5], enemyActiveBoosts = new float[5], playerPassiveBoosts = new float[5], enemyPassiveBoosts = new float[5], playerShields = new float[5], enemyShields = new float[5];
		
	void Update() {
		if (combatStarted)
		{	
			EnemyAI();
		}
	}
	
	public void StartCombat() {
		combatStarted = true;
		int numWeaps = 0, numDefense = 5;
		List<Combat> combatList = GameControl.instance.playerShip.combatList;
		playerPassiveBoosts[0] = GameControl.instance.playerShip.GetWeaponPowerMult(true);
		playerPassiveBoosts[1] = GameControl.instance.playerShip.GetWeaponCooldownMult(true);
		playerPassiveBoosts[2] = GameControl.instance.playerShip.GetWeaponSpeedMult(true);
		for (int i = 0; i< combatList.Count; i++)
		{
			
			if (combatList[i].combatType == CombatType.Weapon)
			{
				playerIcons[numWeaps].SetCombat(combatList[i]);
				playerIcons[numWeaps].SetAvailable(true);
				numWeaps++;
			}
			else 
			{
				playerIcons[numDefense].SetCombat(combatList[i]);
				playerIcons[numDefense].SetAvailable(true);
				numDefense++;
			}
		}
		
		while (numWeaps < 5)
		{
			playerIcons[numWeaps].SetAvailable(false);
			numWeaps++;
		}
		while (numDefense < 10)
		{
			playerIcons[numDefense].SetAvailable(false);
			numDefense++;
		}
		
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, playerHealthBar.localScale.y);
		
		GenerateEnemy();
	}
	
	void StopCombat(bool didWin) {
		if (didWin)
		{
			string popupString = GenerateCombatLoot();
			winPopup.GetComponentsInChildren<TextMeshProUGUI>()[1].SetText(popupString);
			winPopup.SetActive(true);
		}
		else losePopup.SetActive(true);
		
		for (int i =0; i < 10; i++)
		{
			playerIcons[i].SetAvailable(false);
			enemyIcons[i].SetAvailable(false);
		}
		
		combatStarted = false;
	}
	
	public void DoAttack(int damage, WeaponType attackingType, bool isPlayer) {
		if (isPlayer)
		{
			/*switch (attackingType)
			{
				case WeaponType.Kinetic:
					
					break;
				case WeaponType.Missile:
					
					break;
				case WeaponType.Beam:
					
					break;
				case WeaponType.Energy:
					
					break;
				case WeaponType.Hybrid:
					
					break;
			}*/
			enemyShip.currentHull-= Mathf.CeilToInt(damage * (1 - enemyShields[(int)attackingType]/10f));
			enemyHealthBar.localScale = new Vector2((float)enemyShip.currentHull/enemyShip.maxHull, 1);
			if (enemyShip.currentHull < 1)
			{
				StopCombat(true);
			}
		}
		else
		{
		/*switch (attackingType)
		{
			case WeaponType.Kinetic:
				
				break;
			case WeaponType.Missile:
				
				break;
			case WeaponType.Beam:
				
				break;
			case WeaponType.Energy:
				
				break;
			case WeaponType.Hybrid:
				
				break;
		}*/
		GameControl.instance.playerShip.currentHull -= Mathf.CeilToInt(damage * (1 - playerShields[(int)attackingType]/10f));
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, 1);
		if (GameControl.instance.playerShip.currentHull < 1)
			StopCombat(false);
		}
	}
	
	public void AddShield(int amount, WeaponType shieldType, bool isPlayer) {
		if (isPlayer)
		{
			playerShields[(int)shieldType] += amount;
			if (playerShields[(int)shieldType] < 0) playerShields[(int)shieldType] = 0;
		}
		else
		{
			enemyShields[(int)shieldType] += amount;
			if (enemyShields[(int)shieldType] < 0) enemyShields[(int)shieldType] = 0;
		}
	}
	
	public void AddActiveBoost(float amount, CombatBoostType boostType, bool isPlayer) {
		switch (boostType)
		{
			case CombatBoostType.Speed:
				if (amount < 0) amount = 1 - amount / 20f;
				else amount = 1 / (1 + amount / 20f);
				if (isPlayer)
				{
					for (int i = 0; i < playerIcons.Length; i++)
					{
						if (playerIcons[i].isAvailable && playerIcons[i].currentCombat.combatType == CombatType.Weapon)
						{
							playerIcons[i].MultiplySpeed(amount);
						}
					}
				}
				else
				{
					for (int i = 0; i < enemyIcons.Length; i++)
					{
						if (enemyIcons[i].isAvailable && enemyIcons[i].currentCombat.combatType == CombatType.Weapon)
						{
							enemyIcons[i].MultiplySpeed(amount);
						}
					}
				}
			break;
			
			case CombatBoostType.Cooldown:
				if (amount < 0) amount = 1 / (1 + amount / 20f);
				else amount = 1 + amount / 20f;
				if (isPlayer)
				{
					for (int i = 0; i < playerIcons.Length; i++)
					{
						if (playerIcons[i].isAvailable && playerIcons[i].currentCombat.combatType == CombatType.Weapon)
						{
							playerIcons[i].MultiplyCooldown(amount);
						}
					}
				} else
				{
					for (int i = 0; i < enemyIcons.Length; i++)
					{
						if (enemyIcons[i].isAvailable && enemyIcons[i].currentCombat.combatType == CombatType.Weapon)
						{
							enemyIcons[i].MultiplyCooldown(amount);
						}
					}
				}
			break;
			
			default:
				if (isPlayer)
				{
					playerActiveBoosts[(int)boostType] += amount;
					if (playerActiveBoosts[(int)boostType] < 0) playerActiveBoosts[(int)boostType] = 0;
				} else
				{
					enemyActiveBoosts[(int)boostType] += amount;
					if (enemyActiveBoosts[(int)boostType] < 0) enemyActiveBoosts[(int)boostType] = 0;
				}
			break;
		}
	}
	
	void EnemyAI() {
		if (aiState == EnemyAiState.Thinking)
		{
			enemyThinkProgress+= Time.deltaTime;
			
			if (enemyThinkProgress >= enemyThinkTime)
			{
				enemyThinkProgress = 0;
				enemyClickSlot = -1;
				
				for (int i = 0; i < 5; i++)
				{
					if (playerIcons[i].GetTimer() >= .5f)
					{
						for (int j = 5; j < 10; j++)
						{
							if (enemyIcons[j].currentCombat.combatType == CombatType.Shield)
							{
								foreach (WeaponType wType in enemyIcons[j].currentCombat.shieldTypes)
								{
									if (wType == playerIcons[i].currentCombat.weaponType)
									{
										enemyClickSlot = j;
										enemyTargetSlot = i;
										break;
									}
								}
							}
							if (enemyClickSlot != -1) break;
						}
						if (enemyClickSlot != -1) break;
					}
				}
				if (enemyClickSlot == -1)
				{
					for (int i = 0; i < 5; i++)
					{
						
						if (enemyIcons[i].isAvailable && enemyIcons[i].state == SlotState.Ready && enemyIcons[i].currentCombat.chargeType == CombatChargeType.InstantCharge)
						{
							enemyClickSlot = i;
							break;
						}
					}
					if (enemyClickSlot == -1)
					{
						for (int i = 5; i < 10; i++)
						{
							
							if (enemyIcons[i].isAvailable && enemyIcons[i].state == SlotState.Ready && enemyIcons[i].currentCombat.combatType == CombatType.ActiveBoost)
							{
								enemyClickSlot = i;
								break;
							}
						}
						if (enemyClickSlot == -1)
						{
							for (int i = 0; i < 5; i++)
							{
								
								if (enemyIcons[i].isAvailable && enemyIcons[i].state == SlotState.Ready && enemyIcons[i].currentCombat.chargeType == CombatChargeType.TapCharge)
								{
									enemyClickSlot = i;
									break;
								}
							}
							if (enemyClickSlot == -1)
							{
								for (int i = 0; i < 5; i++)
								{
									
									if (enemyIcons[i].isAvailable && enemyIcons[i].state == SlotState.Ready && enemyIcons[i].currentCombat.chargeType == CombatChargeType.SpamClick)
									{
										enemyClickSlot = i;
										break;
									}
								}
								if (enemyClickSlot == -1)
								{
									for (int i = 0; i < 5; i++)
									{
										
										if (enemyIcons[i].isAvailable && enemyIcons[i].state == SlotState.Ready && enemyIcons[i].currentCombat.chargeType == CombatChargeType.HoldClick)
										{
											enemyClickSlot = i;
											break;
										}
									}
								}
							}
						}
					}
				}
				if (enemyClickSlot != -1)
				{
					if (enemyIcons[enemyClickSlot].currentCombat.chargeType == CombatChargeType.HoldClick)
					{
						aiState = EnemyAiState.Holding;
						enemyIcons[enemyClickSlot].EnemyHoldDown(true);
					} else if (enemyIcons[enemyClickSlot].currentCombat.chargeType == CombatChargeType.SpamClick)
					{
						aiState = EnemyAiState.Spamming;
						enemyIcons[enemyClickSlot].EnemyTapMouse();
					} else
					{
						enemyIcons[enemyClickSlot].EnemyTapMouse();
					}
				}
			}
		}else if (aiState == EnemyAiState.Holding)
		{
			if (enemyIcons[enemyClickSlot].state != SlotState.Charging)
			{
				aiState = EnemyAiState.Thinking;
				enemyIcons[enemyClickSlot].EnemyHoldDown(false);
				enemyClickSlot = -1;
				enemyTargetSlot = -1;
			} else if (enemyIcons[enemyClickSlot].currentCombat.combatType == CombatType.Shield && playerIcons[enemyTargetSlot].state != SlotState.Charging)
			{
				aiState = EnemyAiState.Thinking;
				enemyIcons[enemyClickSlot].EnemyHoldDown(false);
				enemyClickSlot = -1;
				enemyTargetSlot = -1;
			}
		} else if (aiState == EnemyAiState.Spamming)
		{
			enemyThinkProgress+= Time.deltaTime;
			if (enemyThinkProgress >= .2)
			{
				enemyThinkProgress = 0;
				enemyIcons[enemyClickSlot].EnemyTapMouse();
				if (enemyIcons[enemyClickSlot].state == SlotState.Cooling)
				{
					aiState = EnemyAiState.Thinking;
					enemyClickSlot = -1;
				}
			}
		}
	}
	
	public void GenerateEnemy() {
		int difficulty = Mathf.FloorToInt(Random.value * 5);
		enemyThinkTime = 1.5f - difficulty*.15f;
		
		enemyShip = Instantiate(enemyShipList[(int)(Random.value * difficulty * enemyShipList.Count / 5)]);
		
		enemyHealthBar.localScale = new Vector2((float)enemyShip.currentHull/enemyShip.maxHull, enemyHealthBar.localScale.y);
		/*enemyShip.command = commList[(int)(Random.value * difficulty * commList.Count / 5)];
		enemyShip.sensor = sensorList[(int)(Random.value * difficulty * sensorList.Count / 5)];
		enemyShip.engine = engineList[(int)(Random.value * difficulty * engineList.Count / 5)];
		enemyShip.weapon = weaponList[(int)(Random.value * difficulty * weaponList.Count / 5)];
		*/
		int enemyWeaps = 0, enemyDefs = 5;
		for (int i = 0; i< enemyShip.combatList.Count; i++)
		{
			if (enemyShip.combatList[i].combatType == CombatType.Weapon)
			{
				enemyIcons[enemyWeaps].SetCombat(enemyShip.combatList[i]);
				enemyIcons[enemyWeaps].SetAvailable(true);
				enemyWeaps++;
			}
			else 
			{
				enemyIcons[enemyDefs].SetCombat(enemyShip.combatList[i]);
				enemyIcons[enemyDefs].SetAvailable(true);
				enemyDefs++;
			}
		}
		while (enemyWeaps < 5)
		{
			enemyIcons[enemyWeaps].SetAvailable(false);
			enemyWeaps++;
		}
		while (enemyDefs < 10)
		{
			enemyIcons[enemyDefs].SetAvailable(false);
			enemyDefs++;
		}
		
		enemyPassiveBoosts[0] = enemyShip.GetWeaponPowerMult(false);
		enemyPassiveBoosts[1] = enemyShip.GetWeaponCooldownMult(false);
		enemyPassiveBoosts[2] = enemyShip.GetWeaponSpeedMult(false);
		
	}
	
	public void ResetPlayerHealth() {
		GameControl.instance.playerShip.currentHull = GameControl.instance.playerShip.maxHull;
	}
	
	string GenerateCombatLoot() {
		string lootString = "Loot collected:\n";
		int scrapAmnt = (int)(enemyShip.maxHull * Random.value * 5);
		ItemStack tempStack = ItemStack.CreateInstance<ItemStack>().Init(lootList[0], scrapAmnt);
		Inventory.instance.AddItem(tempStack);
		lootString = lootString + "- " + scrapAmnt.ToString() + " Salvaged Scrap\n";
		int subslot = 0;
		if (Random.value > .9f)
		{
			subslot = Mathf.FloorToInt(enemyShip.commandList.Count * Random.value);
			Inventory.instance.AddEquipment(enemyShip.commandList[subslot]);
			lootString = lootString  + "- " + enemyShip.commandList[subslot].GetName() + "\n";
		}
		
		if (Random.value > .9f)
		{
			subslot = Mathf.FloorToInt(enemyShip.combatList.Count * Random.value);
			Inventory.instance.AddEquipment(enemyShip.combatList[subslot]);
			lootString = lootString  + "- " + enemyShip.combatList[subslot].GetName() + "\n";
		}
		
		if (Random.value > .9f)
		{
			subslot = Mathf.FloorToInt(enemyShip.sensorList.Count * Random.value);
			Inventory.instance.AddEquipment(enemyShip.sensorList[subslot]);
			lootString = lootString  + "- " + enemyShip.sensorList[subslot].GetName() + "\n";
		}
		
		if (Random.value > .9f)
		{
			subslot = Mathf.FloorToInt(enemyShip.commandList.Count * Random.value);
			Inventory.instance.AddEquipment(enemyShip.engineList[subslot]);
			lootString = lootString  + "- " + enemyShip.engineList[subslot].GetName() + "\n";
		}
		
		return lootString;
		
	}
	
}
