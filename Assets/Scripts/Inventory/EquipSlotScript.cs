using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class EquipSlotScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler {
	
	private MenuControl menuControl;
	
	Inventory inventory;
	
	[SerializeField]
	private int slotNumber;
	
	[SerializeField]
	private EquipmentSlot slotType;
	
	[TextArea]
	[SerializeField]
	private string slotDescription;
	
	void Start()
	{
		menuControl = GameObject.FindObjectOfType(typeof(MenuControl)) as MenuControl;
		inventory = Inventory.instance;
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		menuControl.ShowTooltip(transform.position, slotDescription);
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		menuControl.HideTooltip();
	}
	
	public void OnDrop (PointerEventData eventData)
	{
		GameObject draggedItem = SlotDragScript.itemBeingDragged;
		if (!GetComponent<EquipSlot>().GetEquipment() && draggedItem.GetComponent<EquipSlot>().GetEquipment().equipSlot == slotType)
		{
			inventory.SwapEquipment(draggedItem.GetComponent<EquipSlot>().GetEquipment(), slotNumber);
			GetComponent<EquipSlot>().AddEquipment(draggedItem.GetComponent<EquipSlot>().GetEquipment());
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	
}
