using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SlotState {Ready, Charging, Cooling}

public class CombatIconScript : MonoBehaviour, IPointerClickHandler {
	
	public SlotState state = SlotState.Ready;
	public CombatScreenScript combatScript;
	public GameObject availableOverlay;
	public Image cooldownImage;
	public bool isAvailable = false, isAttack = true, isPlayer = false;
	public WeaponType weapon = WeaponType.Kinetic;
	public int power, speed, cooldown;
	float timer = 0, cooldownTime, speedTime;
	
	void Update()
	{
		if (state == SlotState.Charging)
		{
			timer+= Time.deltaTime;
			cooldownImage.fillAmount = timer/speedTime;
			if (timer > speedTime) 
			{
				timer = 0;
				state = SlotState.Cooling;
				cooldownImage.color = new Color(126, 190, 255, 166);
				if (isAttack)
				{
					if (isPlayer)
					{
						combatScript.playerAttack(power, weapon);
					}
					else combatScript.enemyAttack(power, weapon);
				}
			}
		}
		else if (state == SlotState.Cooling)
		{
			timer+=Time.deltaTime;
			cooldownImage.fillAmount = (cooldownTime - timer) / cooldownTime;
			if (timer > cooldownTime)
			{
				cooldownImage.fillAmount = 0;
				state = SlotState.Ready;
				timer = 0;
			}
		}
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (isPlayer)
			Charge();
	}
	
	public void Charge()
	{
		if (isAvailable && state == SlotState.Ready)
			{
				state = SlotState.Charging;
				timer = 0;
				cooldownImage.color = Color.blue;
			}
	}
	
	public void SetAvailable(bool newAvailability)
	{
		isAvailable = newAvailability;
		availableOverlay.SetActive(!newAvailability);
		timer = 0;
		cooldownImage.fillAmount = 0;
		state = SlotState.Ready;
		
		if (newAvailability)
		{
			cooldownTime = 4/Mathf.Pow(1.116f, cooldown-1);
			speedTime = 2/Mathf.Pow(1.116f, speed-1);
			if (!isAttack)
			{
				cooldownTime/=2;
				speedTime/=2;
			}
		}
	}
	
	public float GetTimer()
	{
		if (state == SlotState.Charging)
			return timer/speedTime;
		else if (state == SlotState.Cooling)
			return (cooldownTime - timer)/cooldownTime;
		else return 0;
	}
	
}
