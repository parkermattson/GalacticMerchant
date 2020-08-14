using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
	float enemyThinkProgress = 0,  enemyThinkTime = .25f;
		
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
		for (int i = 0; i< combatList.Count; i++)
		{
			if (combatList[i].isWeapon)
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
	
	public void playerAttack(int damage, WeaponType attackingType) {
		int defense = 0;
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
		enemyShip.currentHull-= Mathf.CeilToInt(damage / Mathf.Pow(1.055f, defense));
		enemyHealthBar.localScale = new Vector2((float)enemyShip.currentHull/enemyShip.maxHull, 1);
		if (enemyShip.currentHull < 1)
		{
			StopCombat(true);
		}
	}
	
	public void enemyAttack(int damage, WeaponType attackingType) {
		int defense = 0;
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
		GameControl.instance.playerShip.currentHull -= Mathf.CeilToInt(damage / Mathf.Pow(1.055f, defense));
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, 1);
		if (GameControl.instance.playerShip.currentHull < 1)
			StopCombat(false);
	}
	
	void EnemyAI() {
		enemyThinkProgress+= Time.deltaTime;
		
		if (enemyThinkProgress >= enemyThinkTime)
		{
			enemyThinkProgress = 0;
			
			for (int i = 0; i < 5; i++)
			{
				if (playerIcons[i].GetTimer() >= .5f && enemyIcons[i+5].isAvailable && enemyIcons[i+5].state == SlotState.Ready)
				{
					enemyIcons[i+5].Charge();
					break;
				}
			}
			for (int i = 0; i < 5; i++)
			{
				
				if (enemyIcons[i].isAvailable && enemyIcons[i].state == SlotState.Ready)
				{
					enemyIcons[i].Charge();
					break;
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
			if (enemyShip.combatList[i].isWeapon)
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
