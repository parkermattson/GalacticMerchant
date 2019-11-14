using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CombatIconScript : MonoBehaviour, IPointerClickHandler {

	public GameObject border, availableOverlay;
	public Image cooldownImage;
	public bool isSelected = false, isAvailable = false, isAttack = true, isPlayer = false;
	public WeaponType weapon = WeaponType.Kinetic;
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (isAvailable && !isSelected)
		{
			if (isAttack)
				GetComponentInParent<CombatScreenScript>().SelectAttackIcon(this);
			else
				GetComponentInParent<CombatScreenScript>().SelectDefenseIcon(this);
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
		if (newAvailability)
		{
			isAvailable = true;
			availableOverlay.SetActive(false);
		}
		else {
			isAvailable = false;
			availableOverlay.SetActive(true);
		}
	}
	
}
