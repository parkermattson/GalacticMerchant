using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EquipSlotScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	
	private MenuControl menuControl;
	
	[TextArea]
	[SerializeField]
	private string itemDescription;
	
	void Start()
	{
		menuControl = GameObject.FindObjectOfType(typeof(MenuControl)) as MenuControl;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		menuControl.ShowTooltip(transform.position, itemDescription);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		menuControl.HideTooltip();
	}

	
}
