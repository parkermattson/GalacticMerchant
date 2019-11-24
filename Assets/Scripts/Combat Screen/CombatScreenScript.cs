using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CombatScreenScript : MonoBehaviour {

	public CombatIconScript[] enemyIcons = new CombatIconScript[10], playerIcons = new CombatIconScript[10];
	CombatIconScript switchedPlayerAttackIcon, switchedPlayerDefenseIcon, switchedEnemyAttackIcon, switchedEnemyDefenseIcon;
	public GameObject stopCombatPopup;
	public Transform  playerHealthBar, enemyHealthBar;
	bool combatStarted = false;
	Ship enemyShip;
	public List<Equipment> commList = new List<Equipment>(), sensorList = new List<Equipment>(), engineList = new List<Equipment>();
	public List<Weapon> weaponList = new List<Weapon>();
	float[] enemyWeaponThinkProgress = {0,0,0,0,0};
	float enemyThinkProgress = 0,  enemyThinkTime = 1;
	
	void Start()
	{
		enemyShip = Ship.CreateInstance<Ship>();
	}
	
	void OnEnable()
	{
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, playerHealthBar.localScale.y);
	}
	
	void Update() {
		if (combatStarted)
		{	
			EnemyAI();
			
		}
	}
	
	public void StartCombat() {
		combatStarted = true;
		
		for (int i = 0; i< 5; i++)
		{
			if (GameControl.instance.playerShip.GetWeaponPower((WeaponType)i) > 0)
			{
				playerIcons[i].SetAvailable(true);
				playerIcons[i].power = GameControl.instance.playerShip.GetWeaponPower((WeaponType)i);
				playerIcons[i].speed = GameControl.instance.playerShip.GetWeaponSpeed((WeaponType)i);
				playerIcons[i].cooldown = GameControl.instance.playerShip.GetWeaponCooldown((WeaponType)i);
			}
			else playerIcons[i].SetAvailable(false);
		
			if (GameControl.instance.playerShip.GetDefensePower((WeaponType)i) > 0)
			{
				playerIcons[i+5].SetAvailable(true);
				playerIcons[i+5].power = GameControl.instance.playerShip.GetDefensePower((WeaponType)i);
				playerIcons[i+5].speed = GameControl.instance.playerShip.GetDefenseSpeed((WeaponType)i);
				playerIcons[i+5].cooldown = GameControl.instance.playerShip.GetDefenseCooldown((WeaponType)i);
			}
			else playerIcons[i+5].SetAvailable(false);
		}
		
		GenerateEnemy();
	}
	
	void StopCombat(bool didWin) {
		if (didWin)
		{
			stopCombatPopup.GetComponentInChildren<TextMeshProUGUI>().SetText("You Win!");
		}
		else stopCombatPopup.GetComponentInChildren<TextMeshProUGUI>().SetText("You Lose!");
		stopCombatPopup.SetActive(true);
		combatStarted = false;
	}
	
	public void playerAttack(int damage, WeaponType attackingType) {
		int defense = 0;
		switch (attackingType)
		{
			case WeaponType.Kinetic:
				if (enemyIcons[5].state == SlotState.Charging)
				{
					defense+= enemyIcons[5].power;
				}
				if (enemyIcons[6].state == SlotState.Charging)
				{
					defense += enemyIcons[6].power/2;
				}
				break;
			case WeaponType.Missile:
				if (enemyIcons[6].state == SlotState.Charging)
				{
					defense+= enemyIcons[6].power;
				}
				if (enemyIcons[5].state == SlotState.Charging)
				{
					defense += enemyIcons[5].power/2;
				}
				break;
			case WeaponType.Beam:
				if (enemyIcons[7].state == SlotState.Charging)
				{
					defense+= enemyIcons[7].power;
				}
				if (enemyIcons[8].state == SlotState.Charging)
				{
					defense += enemyIcons[8].power/2;
				}
				break;
			case WeaponType.Energy:
				if (enemyIcons[8].state == SlotState.Charging)
				{
					defense+= enemyIcons[8].power;
				}
				if (enemyIcons[5].state == SlotState.Charging)
				{
					defense += enemyIcons[5].power / 2;
				}
				break;
			case WeaponType.Hybrid:
				for (int i = 5; i < 10; i++)
				{
					if (enemyIcons[i].state == SlotState.Charging)
					{
						defense+= enemyIcons[i].power / 4;
					}
				}
				break;
		}
		enemyShip.currentHull-= Mathf.CeilToInt(damage / Mathf.Pow(1.055f, defense));
		enemyHealthBar.localScale = new Vector2((float)enemyShip.currentHull/enemyShip.maxHull, 1);
		if (enemyShip.currentHull < 1)
		{
			StopCombat(true);
		}
	}
	
	public void enemyAttack(int damage, WeaponType attackingType) {
		int defense = 0;
		switch (attackingType)
		{
			case WeaponType.Kinetic:
				if (playerIcons[5].state == SlotState.Charging)
				{
					defense+= playerIcons[5].power;
				}
				if (playerIcons[6].state == SlotState.Charging)
				{
					defense += playerIcons[6].power/2;
				}
				break;
			case WeaponType.Missile:
				if (playerIcons[6].state == SlotState.Charging)
				{
					defense+= playerIcons[6].power;
				}
				if (playerIcons[5].state == SlotState.Charging)
				{
					defense += playerIcons[5].power/2;
				}
				break;
			case WeaponType.Beam:
				if (playerIcons[7].state == SlotState.Charging)
				{
					defense+= playerIcons[7].power;
				}
				if (playerIcons[8].state == SlotState.Charging)
				{
					defense += playerIcons[8].power/2;
				}
				break;
			case WeaponType.Energy:
				if (playerIcons[8].state == SlotState.Charging)
				{
					defense+= playerIcons[8].power;
				}
				if (playerIcons[5].state == SlotState.Charging)
				{
					defense += playerIcons[5].power / 2;
				}
				break;
			case WeaponType.Hybrid:
				for (int i = 5; i < 10; i++)
				{
					if (playerIcons[i].state == SlotState.Charging)
					{
						defense+= playerIcons[i].power / 4;
					}
				}
				break;
		}
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
				if (playerIcons[i].state == SlotState.Charging && enemyIcons[i+5].isAvailable)
				{
					enemyIcons[i+5].Charge();
				}
			}
		}
		
		for (int i = 0; i < 5; i++)
		{
			if (enemyIcons[i].state == SlotState.Ready)
				enemyWeaponThinkProgress[i] += Time.deltaTime;
			if (enemyWeaponThinkProgress[i] > .25f && enemyIcons[i].isAvailable)
			{
				enemyIcons[i].Charge();
				enemyWeaponThinkProgress[i] = 0;
			}
		}
	}
	
	public void GenerateEnemy() {
		int difficulty = Mathf.FloorToInt(Random.value * 5);
		enemyThinkTime = 1.5f - difficulty*.15f;
		
		enemyShip.maxHull = 10 * (int)Mathf.Pow(2, difficulty);
		enemyShip.currentHull = enemyShip.maxHull;
		enemyHealthBar.localScale = new Vector2((float)enemyShip.currentHull/enemyShip.maxHull, enemyHealthBar.localScale.y);
		enemyShip.commandList.Add(commList[(int)(Random.value * difficulty * commList.Count / 5)]);
		enemyShip.sensorList.Add(sensorList[(int)(Random.value * difficulty * sensorList.Count / 5)]);
		enemyShip.engineList.Add(engineList[(int)(Random.value * difficulty * engineList.Count / 5)]);
		enemyShip.weaponsList.Add(weaponList[(int)(Random.value * difficulty * weaponList.Count / 5)]);
		
		for (int i = 0; i< 5; i++)
		{
			if (enemyShip.GetWeaponPower((WeaponType)i) > 0)
			{
				enemyIcons[i].SetAvailable(true);
				enemyIcons[i].power = enemyShip.GetWeaponPower((WeaponType)i);
				enemyIcons[i].speed = enemyShip.GetWeaponSpeed((WeaponType)i);
				enemyIcons[i].cooldown = enemyShip.GetWeaponCooldown((WeaponType)i);
			}
			else enemyIcons[i].SetAvailable(false);
		
			if (enemyShip.GetDefensePower((WeaponType)i) > 0)
			{
				enemyIcons[i+5].SetAvailable(true);
				enemyIcons[i+5].power = enemyShip.GetDefensePower((WeaponType)i);
				enemyIcons[i+5].speed = enemyShip.GetDefenseSpeed((WeaponType)i);
				enemyIcons[i+5].cooldown = enemyShip.GetDefenseCooldown((WeaponType)i);
			}
			else enemyIcons[i+5].SetAvailable(false);
		}
		
	}
	
	public void ResetPlayerHealth() {
		GameControl.instance.playerShip.currentHull = GameControl.instance.playerShip.maxHull;
	}
	
}
