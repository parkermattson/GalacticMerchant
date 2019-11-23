using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatIconScript : MonoBehaviour, IPointerClickHandler {

	enum SlotState {Ready, Charging, Cooling}
	
	SlotState state = SlotState.Ready;
	public CombatScreenScript combatScript;
	public GameObject border, availableOverlay;
	public Image cooldownImage;
	public bool isSelected = false, isAvailable = false, isAttack = true, isPlayer = false;
	public WeaponType weapon = WeaponType.Kinetic;
	public int power, speed, cooldown;
	float timer = 0;
	
	void Update()
	{
		if (isAttack)
		{
			if (state == SlotState.Charging)
			{
				timer+= Time.deltaTime;
				cooldownImage.fillAmount = (float)timer/(4/Mathf.Pow(1.116f, speed-1));
				if (timer > 4/Mathf.Pow(1.116f, speed-1)) 
				{
					timer = 0;
					state = SlotState.Cooling;
					cooldownImage.color = new Color(126, 190, 255, 166);
					if (isPlayer)
					{
						combatScript.playerAttack(power, weapon);
					}
					else combatScript.enemyAttack(power, weapon);
				}
			}
			else if (state == SlotState.Cooling)
			{
				timer+=Time.deltaTime;
				cooldownImage.fillAmount = ((4/Mathf.Pow(1.116f, cooldown-1))-timer)/(4/Mathf.Pow(1.116f, cooldown-1));
				if (timer > 4/Mathf.Pow(1.116f, cooldown-1))
				{
					state = SlotState.Ready;
					cooldownImage.gameObject.SetActive(false);
					timer = 0;
				}
			}
		}
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (isPlayer)
		{
			if (isAttack)
			{
				if (isAvailable && state == SlotState.Ready)
				{
					state = SlotState.Charging;
					timer = 0;
					cooldownImage.color = Color.blue;
					cooldownImage.gameObject.SetActive(true);
					
				}
			} else if (isAvailable && !isSelected)
				combatScript.SelectDefenseIcon(this);
		}
	}
	
	public void Charge()
	{
		if (isAvailable && state == SlotState.Ready)
			{
				state = SlotState.Charging;
				timer = 0;
				cooldownImage.color = Color.blue;
				cooldownImage.gameObject.SetActive(true);
			}
	}
	
	public void Deselect()
	{
		isSelected = false;
		border.SetActive(false);
		
	}
	
	public void FillCooldown(float fillPercent)
	{
		cooldownImage.fillAmount = fillPercent;
	}
	
	public void SetAvailable(bool newAvailability)
	{
			isAvailable = newAvailability;
			availableOverlay.SetActive(!newAvailability);
	}
	
}
