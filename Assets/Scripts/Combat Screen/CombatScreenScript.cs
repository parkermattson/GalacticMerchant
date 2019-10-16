﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatScreenScript : MonoBehaviour {

	public CombatIconScript selectedPlayerAttackIcon, selectedPlayerDefenseIcon, selectedEnemyAttackIcon,selectedEnemyDefenseIcon;
	public CombatIconScript[] enemyIcons = new CombatIconScript[10];
	CombatIconScript switchedPlayerAttackIcon, switchedPlayerDefenseIcon, switchedEnemyAttackIcon, switchedEnemyDefenseIcon;
	public Transform playerTimerBar, enemyTimerBar, playerHealthBar, enemyHealthBar;
	bool combatStarted = false, playerAttackSwitching = false, playerDefenseSwitching = false, enemyAttackSwitching = false, enemyDefenseSwitching = false;
	int playerMaxTimer = 1000, enemyMaxTimer = 1000, playerAttackMaxSwitch = 120, playerDefenseMaxSwitch = 90, 
			enemyAttackMaxSwitch = 120, enemyDefenseMaxSwitch = 90, enemyThinkTime = 0, enemyHealth = 10, enemyHealthMax = 10;
	float playerTimer = 0, enemyTimer = 0, playerAttackSwitchTimer = 0, playerDefenseSwitchTimer = 0, enemyAttackSwitchTimer = 0, enemyDefenseSwitchTimer = 0;
	
	void OnEnable()
	{
		playerHealthBar.localScale = new Vector2((float)GameControl.instance.shipState.currentHull/GameControl.instance.shipState.playerShip.maxHull, playerHealthBar.localScale.y);
		enemyHealthBar.localScale = new Vector2((float)enemyHealth/enemyHealthMax, enemyHealthBar.localScale.y);
	}
	
	void Update()
	{
		if (combatStarted)
		{
			if (playerAttackSwitching)
			{
				playerAttackSwitchTimer+= Mathf.Lerp(0, 1, 1f/playerAttackMaxSwitch);
				if (playerAttackSwitchTimer >= 1)
				{
					playerAttackSwitchTimer = 0;
					SwitchPlayerSelectedAttack();
					playerAttackSwitching = false;
				}
				switchedPlayerAttackIcon.FillCooldown(playerAttackSwitchTimer);
			}
			
			if (playerDefenseSwitching)
			{
				playerDefenseSwitchTimer+= Mathf.Lerp(0, 1, 1f/playerDefenseMaxSwitch);
				if (playerDefenseSwitchTimer >= 1)
				{
					playerDefenseSwitchTimer = 0;
					SwitchPlayerSelectedDefense();
					playerDefenseSwitching = false;
				}
				switchedPlayerDefenseIcon.FillCooldown(playerDefenseSwitchTimer);
			}
			
			if (enemyAttackSwitching)
			{
				enemyAttackSwitchTimer+= Mathf.Lerp(0, 1, 1f/enemyAttackMaxSwitch);
				if (enemyAttackSwitchTimer >= 1)
				{
					enemyAttackSwitchTimer = 0;
					SwitchEnemySelectedAttack();
					enemyAttackSwitching = false;
				}
				switchedEnemyAttackIcon.FillCooldown(enemyAttackSwitchTimer);
			}
			
			if (enemyDefenseSwitching)
			{
				enemyDefenseSwitchTimer+= Mathf.Lerp(0, 1, 1f/enemyDefenseMaxSwitch);
				if (enemyDefenseSwitchTimer >= 1)
				{
					enemyDefenseSwitchTimer = 0;
					SwitchEnemySelectedDefense();
					enemyDefenseSwitching = false;
				}
				switchedEnemyDefenseIcon.FillCooldown(enemyDefenseSwitchTimer);
			}
			
			playerTimer+= Mathf.Lerp(0, 1, 1f/playerMaxTimer);
			enemyTimer+= Mathf.Lerp(0, 1, 1f/enemyMaxTimer);
			
			if (playerTimer >= 1)
			{
				playerTimer = 0;
				if (selectedPlayerAttackIcon.weapon != selectedEnemyDefenseIcon.weapon)
				{
					enemyHealth--;
					enemyHealthBar.localScale = new Vector2((float)enemyHealth/enemyHealthMax, enemyHealthBar.localScale.y);
				}
			}
			if (enemyTimer >= 1)
			{
				enemyTimer = 0;
				if (selectedEnemyAttackIcon.weapon != selectedPlayerDefenseIcon.weapon)
				{
					GameControl.instance.shipState.currentHull--;
					playerHealthBar.localScale = new Vector2((float)GameControl.instance.shipState.currentHull/GameControl.instance.shipState.playerShip.maxHull, playerHealthBar.localScale.y);
					if (GameControl.instance.shipState.currentHull < 1)
					{
						Debug.Log("Game Over");						//Add Game Over State Here
					}
				}
			}
			
			playerTimerBar.localScale = new Vector2 (playerTimer, playerTimerBar.transform.localScale.y);
			enemyTimerBar.localScale = new Vector2 (enemyTimer, enemyTimerBar.transform.localScale.y);
			
			EnemyAI();
			
		}
	}
	
	public void StartCombat()
	{
		combatStarted = true;
		playerTimer = 0;
		enemyTimer = 0;
	}
	
	public void SelectAttackIcon(CombatIconScript newSelected)
	{
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
	
	public void SelectDefenseIcon(CombatIconScript newSelected)
	{
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
	
	void SwitchPlayerSelectedAttack()
	{
		selectedPlayerAttackIcon.Deselect();
		selectedPlayerAttackIcon = switchedPlayerAttackIcon;
		selectedPlayerAttackIcon.isSelected = true;
		selectedPlayerAttackIcon.border.SetActive(true);
	}
	
	void SwitchPlayerSelectedDefense()
	{
		selectedPlayerDefenseIcon.Deselect();
		selectedPlayerDefenseIcon = switchedPlayerDefenseIcon;
		selectedPlayerDefenseIcon.isSelected = true;
		selectedPlayerDefenseIcon.border.SetActive(true);
	}
	
	void SwitchEnemySelectedAttack()
	{
		selectedEnemyAttackIcon.Deselect();
		selectedEnemyAttackIcon = switchedEnemyAttackIcon;
		selectedEnemyAttackIcon.isSelected = true;
		selectedEnemyAttackIcon.border.SetActive(true);
	}
	
	void SwitchEnemySelectedDefense()
	{
		selectedEnemyDefenseIcon.Deselect();
		selectedEnemyDefenseIcon = switchedEnemyDefenseIcon;
		selectedEnemyDefenseIcon.isSelected = true;
		selectedEnemyDefenseIcon.border.SetActive(true);
	}
	
	void EnemyAI()
	{
		enemyThinkTime++;
		
		if (enemyThinkTime >= 60)
		{
			enemyThinkTime = 0;
			
			switch (selectedPlayerAttackIcon.weapon)
			{
				case WeaponType.Kinetic:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Kinetic)
					{
						SelectDefenseIcon(enemyIcons[5]);
					}
					break;
				
				case WeaponType.Missile:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Missile)
					{
						SelectDefenseIcon(enemyIcons[6]);
					}
					break;
				
				case WeaponType.Beam:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Beam)
					{
						SelectDefenseIcon(enemyIcons[7]);
					}
					break;
				
				case WeaponType.Energy:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Energy)
					{
						SelectDefenseIcon(enemyIcons[8]);
					}
					break;
				
				case WeaponType.Hybrid:
					if (selectedEnemyDefenseIcon.weapon != WeaponType.Hybrid)
					{
						SelectDefenseIcon(enemyIcons[9]);
					}
					break;
			}
			
			switch (selectedPlayerDefenseIcon.weapon)
			{
				case WeaponType.Kinetic:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Kinetic)
					{
						SelectAttackIcon(enemyIcons[2]);
					}
					break;
				
				case WeaponType.Missile:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Missile)
					{
						SelectAttackIcon(enemyIcons[3]);
					}
					break;
				
				case WeaponType.Beam:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Beam)
					{
						SelectAttackIcon(enemyIcons[1]);
					}
					break;
				
				case WeaponType.Energy:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Energy)
					{
						SelectAttackIcon(enemyIcons[0]);
					}
					break;
				
				case WeaponType.Hybrid:
					if (selectedEnemyAttackIcon.weapon == WeaponType.Hybrid)
					{
						SelectAttackIcon(enemyIcons[4]);
					}
					break;
			}
			
			
		}
	}
	
}
