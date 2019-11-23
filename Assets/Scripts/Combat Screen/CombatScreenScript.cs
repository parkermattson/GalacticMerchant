using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CombatScreenScript : MonoBehaviour {

	public CombatIconScript selectedPlayerAttackIcon, selectedPlayerDefenseIcon, selectedEnemyAttackIcon,selectedEnemyDefenseIcon;
	public CombatIconScript[] enemyIcons = new CombatIconScript[10], playerIcons = new CombatIconScript[10];
	CombatIconScript switchedPlayerAttackIcon, switchedPlayerDefenseIcon, switchedEnemyAttackIcon, switchedEnemyDefenseIcon;
	public GameObject stopCombatPopup;
	public Transform playerTimerBar, enemyTimerBar, playerHealthBar, enemyHealthBar;
	bool combatStarted = false, playerAttackSwitching = false, playerDefenseSwitching = false, enemyAttackSwitching = false, enemyDefenseSwitching = false;
	int  enemyHealth = 10, enemyHealthMax = 10;
	int[] playerWeaponPower = new int[5], playerWeaponSpeed = new int[5], playerDefensePower = new int[5], playerDefenseSpeed = new int[5], playerWeaponCooldown = new int[5], playerDefenseCooldown = new int[5];
	float[] enemyWeaponThinkProgress = {0,0,0,0,0};
	float enemyAttackMaxSwitch = 2f, enemyDefenseMaxSwitch = 1.5f, enemyThinkProgress = 0,  enemyThinkTime = 1,
			playerMaxTimer = 10f, enemyMaxTimer = 10f, playerAttackMaxSwitch = 2f, playerDefenseMaxSwitch = 1.5f,
			playerTimer = 0, enemyTimer = 0, playerAttackSwitchTimer = 0, playerDefenseSwitchTimer = 0, enemyAttackSwitchTimer = 0, enemyDefenseSwitchTimer = 0,
			playerTimerSpeed = 1, playerAttackSwitchSpeed = 1, playerDefenseSwitchSpeed = 1;
	
	void OnEnable()
	{
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, playerHealthBar.localScale.y);
		enemyHealthBar.localScale = new Vector2((float)enemyHealth/enemyHealthMax, enemyHealthBar.localScale.y);
		
		
	}
	
	void Update() {
		if (combatStarted)
		{	
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
			
			playerTimerBar.localScale = new Vector2 (playerTimer/playerMaxTimer, playerTimerBar.transform.localScale.y);
			enemyTimerBar.localScale = new Vector2 (enemyTimer/enemyMaxTimer, enemyTimerBar.transform.localScale.y);
			
			EnemyAI();
			
		}
	}
	
	public void StartCombat() {
		combatStarted = true;
		playerTimer = 0;
		enemyTimer = 0;	
		
		int[] numOfWeaps = {0,0,0,0,0,0,0,0,0,0};
		foreach (Weapon weap in GameControl.instance.playerShip.weaponsList) {
			for (int i =0; i < weap.weaponParts.Count; i++) {
				int partType = (int)weap.weaponParts[i];
				playerIcons[partType].SetAvailable(true);
				playerWeaponPower[partType] += weap.weaponPower[i];
				playerWeaponSpeed[partType] += weap.weaponSpeed[i];
				playerWeaponCooldown[partType] += weap.weaponCooldown[i];
				numOfWeaps[partType]++;
			}
			
			for (int i =0; i < weap.defenseParts.Count; i++) {
				int partType = (int)weap.defenseParts[i];
				playerIcons[partType+5].SetAvailable(true);
				playerDefensePower[partType] += weap.defensePower[i];
				playerDefenseSpeed[partType] += weap.defenseSpeed[i];
				playerDefenseCooldown[partType] += weap.defenseCooldown[i];
				numOfWeaps[partType+5]++;
			}
		}
		
		for (int i = 0; i < 5; i++)
		{
			if (numOfWeaps[i] != 0)
			{
				playerWeaponSpeed[i] /=numOfWeaps[i];
				playerWeaponCooldown[i] /= numOfWeaps[i];
				playerIcons[i].power = playerWeaponPower[i];
				playerIcons[i].speed = playerWeaponSpeed[i];
				playerIcons[i].cooldown = playerWeaponCooldown[i];
			}
		}
		
		for (int i = 5; i < 10; i++)
		{
			if (numOfWeaps[i] != 0)
			{
				playerDefenseSpeed[i-5] /=numOfWeaps[i];
				playerDefenseCooldown[i-5] /= numOfWeaps[i];
				playerIcons[i].power = playerDefensePower[i-5];
				playerIcons[i].speed = playerDefenseSpeed[i-5];
				playerIcons[i].cooldown = playerDefenseCooldown[i-5];
			}
			
			if (selectedPlayerDefenseIcon == null && playerIcons[i].isAvailable)
			{
				switchedPlayerDefenseIcon = playerIcons[i];
				SwitchPlayerSelectedDefense();
			}
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
	
	public void playerAttack(int damage, WeaponType attackingType) {
		enemyHealth-= damage;
		enemyHealthBar.localScale = new Vector2((float)enemyHealth/enemyHealthMax, 1);
		if (enemyHealth < 1)
		{
			StopCombat(true);
		}
	}
	
	public void enemyAttack(int damage, WeaponType attackingType) {
		GameControl.instance.playerShip.currentHull -= Mathf.CeilToInt(damage / Mathf.Pow(1.055f, playerDefensePower[(int)attackingType]));
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.playerShip.currentHull/GameControl.instance.playerShip.maxHull, 1);
		if (GameControl.instance.playerShip.currentHull < 1)
			StopCombat(false);
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
			
			for (int i = 0; i < 5; i++)
			{
				enemyWeaponThinkProgress[i] -= Time.deltaTime;
				if (enemyWeaponThinkProgress[i] <= 0 && enemyIcons[i+5].isAvailable)
				{
					enemyIcons[i+5].Charge();
				}
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
