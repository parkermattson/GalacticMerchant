using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum SlotState {Ready, Charging, Cooling}

public class CombatIconScript : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
	
	public SlotState state = SlotState.Ready;
	public CombatScreenScript combatScript;
	public GameObject availableOverlay;
	public Image cooldownImage;
	public TextMeshProUGUI typeText;
	public bool isAvailable = false, isAttack = true, isPlayer = false;
	public Combat currentCombat;
	public float timer, timerMax = 3, speedMult = 1, cooldownMult = 1, enemyDelay;
	public bool mouseDown = false;
	
	void Update() {
		if (isAvailable)
		{
			if (state == SlotState.Ready)
			{
				if (timer > 0) timer-= Time.deltaTime * speedMult / 3;
				else if (timer < 0) timer = 0;
				
				cooldownImage.fillAmount = timer/timerMax;
			}
			if (state == SlotState.Charging)
			{
				if (currentCombat.chargeType == CombatChargeType.TapCharge || (currentCombat.chargeType == CombatChargeType.HoldClick && mouseDown))
				{
					timer+= Time.deltaTime  * speedMult;
					cooldownImage.fillAmount = timer/timerMax;
				}
				if (timer > timerMax) 
				{
					timer = 0;
					state = SlotState.Cooling;
					mouseDown = false;
					cooldownImage.color = new Color(126, 190, 255, 166);
					switch (currentCombat.combatType)
					{
						case CombatType.Weapon:
							combatScript.DoAttack(currentCombat.power, currentCombat.weaponType, isPlayer);
						break;
						case CombatType.Shield:
							for (int i = 0; i < currentCombat.shieldTypes.Count; i++)
							{
								combatScript.AddShield(-currentCombat.boostAmt[i], currentCombat.shieldTypes[i], isPlayer);
							}
						break;
						case CombatType.ActiveBoost:
							for (int i = 0; i < currentCombat.boostTypes.Count; i++)
							{
								combatScript.AddActiveBoost(currentCombat.boostAmt[i], currentCombat.boostTypes[i], isPlayer);
							}
						break;
						case CombatType.DmgControl:
						
						break;
					}
				}
			}
			else if (state == SlotState.Cooling)
			{
				timer+=Time.deltaTime  * cooldownMult;
				cooldownImage.fillAmount = (timerMax - timer) / timerMax;
				if (timer > timerMax)
				{
					cooldownImage.fillAmount = 0;
					state = SlotState.Ready;
					timer = 0;
				}
			}
		}
	}
	
	public void OnPointerClick(PointerEventData eventData) {
		if (isPlayer)
		{
			switch (currentCombat.chargeType)
			{
				case CombatChargeType.TapCharge:
					if (isAvailable && state == SlotState.Ready)
					{
						if (currentCombat.combatType == CombatType.Shield)
						{
							for (int i = 0; i < currentCombat.shieldTypes.Count; i++)
							{
								combatScript.AddShield(currentCombat.boostAmt[i], currentCombat.shieldTypes[i], isPlayer);
							}
						}
						if (currentCombat.chargeType == CombatChargeType.TapCharge)
						{
							for (int i = 0; i < currentCombat.boostTypes.Count; i++)
							{
								combatScript.AddActiveBoost(-currentCombat.boostAmt[i], currentCombat.boostTypes[i], isPlayer);
							}
						}
						state = SlotState.Charging;
						timer = 0;
						cooldownImage.color = Color.blue;
					}
				break;
				
				case CombatChargeType.SpamClick:
					if (isAvailable && state == SlotState.Ready)
					{
						state = SlotState.Charging;
						timer = .12f * speedMult;
						cooldownImage.color = Color.blue;
						cooldownImage.fillAmount = timer/timerMax;
					}else if (isAvailable && state == SlotState.Charging)
					{
						state = SlotState.Charging;
						timer += .12f * speedMult;
						cooldownImage.fillAmount = timer/timerMax;
					}
				break;
				
				case CombatChargeType.InstantCharge:
					if (isAvailable && state == SlotState.Ready)
					{
						state = SlotState.Charging;
						timer = timerMax+1;
					}
				break;
			}
		}
	}
	
	public void OnPointerDown(PointerEventData eventData) {
		if (isAvailable && isPlayer && state == SlotState.Ready && currentCombat.chargeType == CombatChargeType.HoldClick)
		{
			mouseDown = true;
			state = SlotState.Charging;
			cooldownImage.color = Color.blue;
			if (currentCombat.combatType == CombatType.Shield)
			{
				for (int i = 0; i < currentCombat.shieldTypes.Count; i++)
				{
					combatScript.AddShield(currentCombat.boostAmt[i], currentCombat.shieldTypes[i], isPlayer);
				}
			}
			if (currentCombat.combatType == CombatType.ActiveBoost)
			{
				for (int i = 0; i < currentCombat.boostTypes.Count; i++)
				{
					combatScript.AddActiveBoost(-currentCombat.boostAmt[i], currentCombat.boostTypes[i], isPlayer);
				}
			}
		}
	}
	
	public void OnPointerUp(PointerEventData eventData) {
		if (isAvailable && isPlayer && state == SlotState.Charging && currentCombat.chargeType == CombatChargeType.HoldClick)
		{
			mouseDown=false;
			state = SlotState.Ready;
			if (isAvailable && isPlayer)
			{
				if (currentCombat.chargeType == CombatChargeType.HoldClick)
				{
					if (currentCombat.combatType == CombatType.Shield)
					{
						for (int i = 0; i < currentCombat.shieldTypes.Count; i++)
						{
							combatScript.AddShield(-currentCombat.boostAmt[i], currentCombat.shieldTypes[i], isPlayer);
						}
					}
					if (currentCombat.combatType == CombatType.ActiveBoost)
					{
						for (int i = 0; i < currentCombat.boostTypes.Count; i++)
						{
							combatScript.AddActiveBoost(currentCombat.boostAmt[i], currentCombat.boostTypes[i], isPlayer);
						}
					}
				}
			}
		}
	}
	
	public void SetAvailable(bool newAvailability) {
		isAvailable = newAvailability;
		availableOverlay.SetActive(!newAvailability);
		timer = 0;
		cooldownImage.fillAmount = 0;
		state = SlotState.Ready;
		typeText.SetText("X");
	}
	
	public void SetCombat(Combat newCombat) {
		currentCombat = newCombat;
		cooldownMult = .5f;
		speedMult = 1;
		if (!isAttack)
		{
			speedMult/=2f;
		}
		switch (currentCombat.combatType)
		{
			case CombatType.Weapon:
				switch (currentCombat.weaponType)
				{
					case WeaponType.Kinetic:
						typeText.SetText("K");
					break;
					case WeaponType.Missile:
						typeText.SetText("M");
					break;
					case WeaponType.Beam:
						typeText.SetText("B");
					break;
					case WeaponType.Energy:
						typeText.SetText("E");
					break;
					case WeaponType.Hybrid:
						typeText.SetText("H");
					break;
				}
				break;
			
			case CombatType.Shield:
				typeText.SetText("S");
				break;
			
			case CombatType.ActiveBoost:
				typeText.SetText("A");
				break;
				
			case CombatType.DmgControl:
				typeText.SetText("D");
				break;
			
			case CombatType.PassiveBoost:
				typeText.SetText("P");
				break;
		}
	}
	
	public float GetTimer() {
		if (state == SlotState.Charging)
			return timer/timerMax;
		else if (state == SlotState.Cooling)
			return (timerMax - timer)/timerMax;
		else return 0;
	}
	
	public void EnemyHoldDown(bool holding) {
		if (holding && state == SlotState.Ready)
		{
			mouseDown = true;
			state = SlotState.Charging;
			cooldownImage.color = Color.blue;
			if (currentCombat.combatType == CombatType.Shield)
			{
				for (int i = 0; i < currentCombat.shieldTypes.Count; i++)
				{
					combatScript.AddShield(currentCombat.boostAmt[i], currentCombat.shieldTypes[i], isPlayer);
				}
			}
			if (currentCombat.combatType == CombatType.ActiveBoost)
			{
				for (int i = 0; i < currentCombat.boostTypes.Count; i++)
				{
					combatScript.AddActiveBoost(-currentCombat.boostAmt[i], currentCombat.boostTypes[i], isPlayer);
				}
			}
		} else if (state == SlotState.Charging)
		{
			mouseDown=false;
			state = SlotState.Ready;
			if (currentCombat.chargeType == CombatChargeType.HoldClick)
			{
				if (currentCombat.combatType == CombatType.Shield)
				{
					for (int i = 0; i < currentCombat.shieldTypes.Count; i++)
					{
						combatScript.AddShield(-currentCombat.boostAmt[i], currentCombat.shieldTypes[i], isPlayer);
					}
				}
				if (currentCombat.combatType == CombatType.ActiveBoost)
				{
					for (int i = 0; i < currentCombat.boostTypes.Count; i++)
					{
						combatScript.AddActiveBoost(currentCombat.boostAmt[i], currentCombat.boostTypes[i], isPlayer);
					}
				}
			}
		}
	}
	
	public void EnemyTapMouse() {
		switch (currentCombat.chargeType)
		{
			case CombatChargeType.TapCharge:
				if (isAvailable && state == SlotState.Ready)
				{
					if (currentCombat.combatType == CombatType.Shield)
					{
						for (int i = 0; i < currentCombat.shieldTypes.Count; i++)
						{
							combatScript.AddShield(currentCombat.boostAmt[i], currentCombat.shieldTypes[i], isPlayer);
						}
					}
					if (currentCombat.chargeType == CombatChargeType.TapCharge)
					{
						for (int i = 0; i < currentCombat.boostTypes.Count; i++)
						{
							combatScript.AddActiveBoost(-currentCombat.boostAmt[i], currentCombat.boostTypes[i], isPlayer);
						}
					}
					state = SlotState.Charging;
					timer = 0;
					cooldownImage.color = Color.blue;
				}
				break;
			
			case CombatChargeType.SpamClick:
				if (isAvailable && state == SlotState.Ready)
				{
					state = SlotState.Charging;
					timer = .12f * speedMult;
					cooldownImage.color = Color.blue;
					cooldownImage.fillAmount = timer/timerMax;
				}else if (isAvailable && state == SlotState.Charging)
				{
					state = SlotState.Charging;
					timer += .12f * speedMult;
					cooldownImage.fillAmount = timer/timerMax;
				}
				break;
			
			case CombatChargeType.InstantCharge:
				if (isAvailable && state == SlotState.Ready)
				{
					state = SlotState.Charging;
					timer = timerMax+1;
				}
				break;
		}
	}
	
	public void MultiplySpeed(float amount) {
		speedMult *= amount;
	}
	
	public void MultiplyCooldown(float amount) {
		cooldownMult *= amount;
	}
	
}
