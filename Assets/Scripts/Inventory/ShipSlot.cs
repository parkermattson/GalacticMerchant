using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSlot : EquipSlot, IPointerEnterHandler, IPointerExitHandler, IDropHandler {
	
	private MenuControl menuControl;
	
	Inventory inventory;
	
	[SerializeField]
	private int slotNumber;
	
	[SerializeField]
	private int subslotNumber;
	
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
		menuControl.ShowTooltip(transform.position, equipment.GetName() +"\nType: "+ equipment.GetTypeName() +"\n"+ equipment.GetDescription());
	}
	
	public void OnPointerExit(PointerEventData eventData)
	{
		menuControl.HideTooltip();
	}
	
	public void AddShipEquipment (Equipment newEquip)
	{
		equipment = newEquip;
		
		icon.sprite = equipment.icon;
		icon.enabled = true;
	}
	
	public void OnDrop (PointerEventData eventData)
	{
		GameObject slotBox = SlotDragScript.slotBox;
		EquipSlot draggedSlot = slotBox.GetComponent<EquipSlot>();
		if (draggedSlot.GetEquipment().GetSlotType() == slotType)
		{
			inventory.SwapEquipment(draggedSlot.GetEquipment(), slotNumber, subslotNumber);
			GetComponent<ShipSlot>().AddShipEquipment(draggedSlot.GetEquipment());
			Debug.Log(SlotDragScript.slot.name);
			Destroy(SlotDragScript.slot);
		}
	}
	
}
