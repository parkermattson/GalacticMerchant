using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatScreenScript : MonoBehaviour {

	public CombatIconScript selectedPlayerAttackIcon, selectedPlayerDefenseIcon, selectedEnemyAttackIcon,selectedEnemyDefenseIcon;
	public CombatIconScript[] enemyIcons = new CombatIconScript[10], playerIcons = new CombatIconScript[10];
	CombatIconScript switchedPlayerAttackIcon, switchedPlayerDefenseIcon, switchedEnemyAttackIcon, switchedEnemyDefenseIcon;
	public Transform playerTimerBar, enemyTimerBar, playerHealthBar, enemyHealthBar;
	bool combatStarted = false, playerAttackSwitching = false, playerDefenseSwitching = false, enemyAttackSwitching = false, enemyDefenseSwitching = false;
	int  enemyHealth = 10, enemyHealthMax = 10;
	int[] playerWeaponPower = new int[5], playerWeaponSpeed = new int[5], playerDefensePower = new int[5], playerDefenseSpeed = new int[5];
	float enemyAttackMaxSwitch = 2f, enemyDefenseMaxSwitch = 1.5f, enemyThinkProgress = 0,  enemyThinkTime = 1,
			playerMaxTimer = 10f, enemyMaxTimer = 10f, playerAttackMaxSwitch = 2f, playerDefenseMaxSwitch = 1.5f,
			playerTimer = 0, enemyTimer = 0, playerAttackSwitchTimer = 0, playerDefenseSwitchTimer = 0, enemyAttackSwitchTimer = 0, enemyDefenseSwitchTimer = 0,
			playerTimerSpeed = 1, playerAttackSwitchSpeed = 1, playerDefenseSwitchSpeed = 1;
	
	void OnEnable()
	{
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, playerHealthBar.localScale.y);
		enemyHealthBar.localScale = new Vector2((float)enemyHealth/enemyHealthMax, enemyHealthBar.localScale.y);
		
		int[] numOfWeaps = {0,0,0,0,0,0,0,0,0,0};
		foreach (Weapon weap in GameControl.instance.playerShip.weaponsList) {
			foreach (WeaponType type in weap.weaponParts) {
				switch (type) {
					case WeaponType.Kinetic: 
						playerIcons[0].SetAvailable(true);
						playerWeaponPower[0] += weap.weaponPower[0];
						playerWeaponSpeed[0] += weap.weaponSpeed[0];
						numOfWeaps[0]++;
						break;
					
					case WeaponType.Missile: 
						playerIcons[1].SetAvailable(true);
						playerWeaponPower[1] += weap.weaponPower[1];
						playerWeaponSpeed[1] += weap.weaponSpeed[1];
						numOfWeaps[1]++;
						break;
					
					case WeaponType.Beam: 
						playerIcons[2].SetAvailable(true);
						playerWeaponPower[2] += weap.weaponPower[2];
						playerWeaponSpeed[2] += weap.weaponSpeed[2];
						numOfWeaps[2]++;
						break;
					
					case WeaponType.Energy: 
						playerIcons[3].SetAvailable(true);
						playerWeaponPower[3] += weap.weaponPower[3];
						playerWeaponSpeed[3] += weap.weaponSpeed[3];
						numOfWeaps[3]++;
						break;
					
					case WeaponType.Hybrid: 
						playerIcons[4].SetAvailable(true);
						playerWeaponPower[4] += weap.weaponPower[4];
						playerWeaponSpeed[4] += weap.weaponSpeed[4];
						numOfWeaps[4]++;
						break;
				}
			}
			
			foreach (WeaponType type in weap.defenseParts) {
				switch (type) {
					case WeaponType.Kinetic: 
						playerIcons[5].SetAvailable(true);
						playerDefensePower[0] += weap.defensePower[0];
						playerDefenseSpeed[0] += weap.defenseSpeed[0];
						numOfWeaps[5]++;
						break;
					
					case WeaponType.Missile:
						playerIcons[6].SetAvailable(true);
						playerDefensePower[1] += weap.defensePower[1];
						playerDefenseSpeed[1] += weap.defenseSpeed[1];
						numOfWeaps[6]++;
						break;
					
					case WeaponType.Energy:
						playerIcons[7].SetAvailable(true);
						playerDefensePower[2] += weap.defensePower[2];
						playerDefenseSpeed[2] += weap.defenseSpeed[2];
						numOfWeaps[7]++;
						break;
					
					case WeaponType.Beam:
						playerIcons[8].SetAvailable(true);
						playerDefensePower[3] += weap.defensePower[3];
						playerDefenseSpeed[3] += weap.defenseSpeed[3];
						numOfWeaps[8]++;
						break;
					
					case WeaponType.Hybrid:
						playerIcons[9].SetAvailable(true);
						playerDefensePower[4] += weap.defensePower[4];
						playerDefenseSpeed[4] += weap.defenseSpeed[4];
						numOfWeaps[9]++;
						break;
				}
			}
		}
		
		for (int i = 0; i < 5; i++)
		{
			if (numOfWeaps[i] != 0)
				playerWeaponSpeed[i] /=numOfWeaps[i];
			
			if (selectedPlayerAttackIcon == null && playerIcons[i].isAvailable)
			{
				switchedPlayerAttackIcon = playerIcons[i];
				SwitchPlayerSelectedAttack();
			}
		}
		
		for (int i = 5; i < 10; i++)
		{
			if (numOfWeaps[i] != 0)
				playerDefenseSpeed[i-5] /=numOfWeaps[i];
			
			if (selectedPlayerDefenseIcon == null && playerIcons[i].isAvailable)
			{
				switchedPlayerDefenseIcon = playerIcons[i];
				SwitchPlayerSelectedDefense();
			}
		}
		
		GenerateEnemy();
	}
	
	void Update() {
		if (combatStarted)
		{
			if (playerAttackSwitching)
			{
				playerAttackSwitchTimer+= Time.deltaTime*playerAttackSwitchSpeed;
				if (playerAttackSwitchTimer >= playerAttackMaxSwitch)
				{
					playerAttackSwitchTimer = 0;
					SwitchPlayerSelectedAttack();
					playerAttackSwitching = false;
				}
				switchedPlayerAttackIcon.FillCooldown(playerAttackSwitchTimer/playerAttackMaxSwitch);
			}
			
			if (playerDefenseSwitching)
			{
				playerDefenseSwitchTimer+= Time.deltaTime*playerDefenseSwitchSpeed;
				if (playerDefenseSwitchTimer >= playerDefenseMaxSwitch)
				{
					playerDefenseSwitchTimer = 0;
					SwitchPlayerSelectedDefense();
					playerDefenseSwitching = false;
				}
				switchedPlayerDefenseIcon.FillCooldown(playerDefenseSwitchTimer/playerDefenseMaxSwitch);
			}
			
			if (enemyAttackSwitching)
			{
				enemyAttackSwitchTimer+= Time.deltaTime;
				if (enemyAttackSwitchTimer >= enemyAttackMaxSwitch)
				{
					enemyAttackSwitchTimer = 0;
					SwitchEnemySelectedAttack();
					enemyAttackSwitching = false;
				}
				switchedEnemyAttackIcon.FillCooldown(enemyAttackSwitchTimer/enemyAttackMaxSwitch);
			}
			
			if (enemyDefenseSwitching)
			{
				enemyDefenseSwitchTimer+= Time.deltaTime;
				if (enemyDefenseSwitchTimer >= enemyDefenseMaxSwitch)
				{
					enemyDefenseSwitchTimer = 0;
					SwitchEnemySelectedDefense();
					enemyDefenseSwitching = false;
				}
				switchedEnemyDefenseIcon.FillCooldown(enemyDefenseSwitchTimer/enemyDefenseMaxSwitch);
			}
			
			playerTimer+= Time.deltaTime*playerTimerSpeed;
			enemyTimer+= Time.deltaTime;
			
			if (playerTimer >= playerMaxTimer)
			{
				playerTimer = 0;
				if (selectedPlayerAttackIcon.weapon != selectedEnemyDefenseIcon.weapon)
				{
					enemyHealth--;
					enemyHealthBar.localScale = new Vector2((float)enemyHealth/enemyHealthMax, enemyHealthBar.localScale.y);
					
				}
			}
			if (enemyTimer >= enemyMaxTimer)
			{
				enemyTimer = 0;
				if (selectedEnemyAttackIcon.weapon != selectedPlayerDefenseIcon.weapon)
				{
					GameControl.instance.playerShip.currentHull--;
					playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, playerHealthBar.localScale.y);
					if (GameControl.instance.playerShip.currentHull < 1)
					{
						Debug.Log("Game Over");						//Add Game Over State Here
					}
				}
			}
			
			playerTimerBar.localScale = new Vector2 (playerTimer/playerMaxTimer, playerTimerBar.transform.localScale.y);
			enemyTimerBar.localScale = new Vector2 (enemyTimer/enemyMaxTimer, enemyTimerBar.transform.localScale.y);
			
			EnemyAI();
			
		}
	}
	
	public void StartCombat() {
		combatStarted = true;
		playerTimer = 0;
		enemyTimer = 0;	
	}
	
	void StopCombat() {
		combatStarted = false;
	}
	
	public void SelectAttackIcon(CombatIconScript newSelected) {
		if (!playerAttackSwitching)
		{
			if (newSelected.isPlayer)
			{
				switchedPlayerAttackIcon = newSelected;
				playerAttackSwitching = true;
			} else {
				switchedEnemyAttackIcon = newSelected;
				enemyAttackSwitching = true;
			}
		}
	}
	
	public void SelectDefenseIcon(CombatIconScript newSelected) {
		if (!playerDefenseSwitching)
		{
			if (newSelected.isPlayer)
			{
				switchedPlayerDefenseIcon = newSelected;
				playerDefenseSwitching = true;
			} else {
				switchedEnemyDefenseIcon = newSelected;
				enemyDefenseSwitching = true;
			}
		}
	}
	
	void SwitchPlayerSelectedAttack() {
		if (selectedPlayerAttackIcon != null)
			selectedPlayerAttackIcon.Deselect();
		selectedPlayerAttackIcon = switchedPlayerAttackIcon;
		selectedPlayerAttackIcon.isSelected = true;
		selectedPlayerAttackIcon.border.SetActive(true);
		playerTimerSpeed= 1f+ (playerWeaponSpeed[(int)selectedPlayerAttackIcon.weapon]-1)/4f;
	}
	
	void SwitchPlayerSelectedDefense() {
		if (selectedPlayerDefenseIcon != null)
			selectedPlayerDefenseIcon.Deselect();
		selectedPlayerDefenseIcon = switchedPlayerDefenseIcon;
		selectedPlayerDefenseIcon.isSelected = true;
		selectedPlayerDefenseIcon.border.SetActive(true);
		playerDefenseSwitchSpeed = 1f + (playerDefenseSpeed[(int)selectedPlayerDefenseIcon.weapon]-1)/4f;
	}
	
	void SwitchEnemySelectedAttack() {
		selectedEnemyAttackIcon.Deselect();
		selectedEnemyAttackIcon = switchedEnemyAttackIcon;
		selectedEnemyAttackIcon.isSelected = true;
		selectedEnemyAttackIcon.border.SetActive(true);
	}
	
	void SwitchEnemySelectedDefense() {
		selectedEnemyDefenseIcon.Deselect();
		selectedEnemyDefenseIcon = switchedEnemyDefenseIcon;
		selectedEnemyDefenseIcon.isSelected = true;
		selectedEnemyDefenseIcon.border.SetActive(true);
	}
	
	void EnemyAI() {
		enemyThinkProgress+= Time.deltaTime;
		
		if (enemyThinkProgress >= enemyThinkTime)
		{
			enemyThinkProgress = 0;
			
			switch (selectedPlayerAttackIcon.weapon)
			{
				case WeaponType.Kinetic:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Kinetic)
					{
						if (enemyIcons[5].isAvailable)
							SelectDefenseIcon(enemyIcons[5]);
					}
					break;
				
				case WeaponType.Missile:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Missile)
					{
						if (enemyIcons[6].isAvailable)
							SelectDefenseIcon(enemyIcons[6]);
					}
					break;
				
				case WeaponType.Beam:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Beam)
					{
						if (enemyIcons[7].isAvailable)
							SelectDefenseIcon(enemyIcons[7]);
					}
					break;
				
				case WeaponType.Energy:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Energy)
					{
						if (enemyIcons[8].isAvailable)
							SelectDefenseIcon(enemyIcons[8]);
					}
					break;
				
				case WeaponType.Hybrid:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Hybrid)
					{
						if (enemyIcons[9].isAvailable)
							SelectDefenseIcon(enemyIcons[9]);
					}
					break;
			}
			
			switch (selectedPlayerDefenseIcon.weapon)
			{
				case WeaponType.Kinetic:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Kinetic)
					{
						if (enemyIcons[2].isAvailable)
							SelectAttackIcon(enemyIcons[2]);
					}
					break;
				
				case WeaponType.Missile:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Missile)
					{
						if (enemyIcons[3].isAvailable)
							SelectAttackIcon(enemyIcons[3]);
					}
					break;
				
				case WeaponType.Beam:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Beam)
					{
						if (enemyIcons[1].isAvailable)
							SelectAttackIcon(enemyIcons[1]);
					}
					break;
				
				case WeaponType.Energy:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Energy)
					{
						if (enemyIcons[0].isAvailable)
							SelectAttackIcon(enemyIcons[0]);
					}
					break;
				
				case WeaponType.Hybrid:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Hybrid)
					{
						if (enemyIcons[4].isAvailable)
							SelectAttackIcon(enemyIcons[4]);
					}
					break;
			}
			
			
		}
	}
	
	public void GenerateEnemy() {
		int difficulty = Mathf.FloorToInt(Random.value * 5);
		enemyHealthMax = 10 * (int)Mathf.Pow(2, difficulty);
		enemyHealth = enemyHealthMax;
		
		enemyThinkTime = 1.5f - difficulty*.15f;
		enemyMaxTimer = 12f - difficulty*2;
		enemyAttackMaxSwitch = 3f - difficulty*.4f;
		enemyAttackMaxSwitch = 2.5f - difficulty*.35f;
		
		int needed = difficulty;
		for (int i = 0; i < 4; i++)
		{
			if (Random.value < needed/(5-i))
			{
				enemyIcons[i].SetAvailable(true);
			}
		}
		
		needed = difficulty;
		for (int i = 0; i < 4; i++)
		{
			if (Random.value < needed/(5-i))
			{
				enemyIcons[i+5].SetAvailable(true);
			}
		}
	}
	
}
