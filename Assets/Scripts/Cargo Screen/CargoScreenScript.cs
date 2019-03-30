using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CargoScreenScript : MonoBehaviour {

	Inventory inventory;
	
	public GameObject selectedSlot;
	public TextMeshProUGUI itemName, itemQuantity, itemWeight, itemValue, itemDesc;
	
	public Transform slotPrefab;
	public Transform cargoSlotBox;
	
	Slot[] cargoSlots;
	
	void Awake()
	{
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += CargoScreenUpdate;
		
		cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
	}
	
	void OnEnable()
	{
		CargoScreenUpdate();
		
		Slot slotScript = selectedSlot.GetComponent<Slot>();
		if (slotScript.GetItem() != null)
		{
			itemName.SetText(slotScript.GetItem().GetName());
			itemDesc.SetText(slotScript.GetItem().GetDescription());
			//itemValue.SetText("Qt: " + slotScript.GetItem().GetValue());
			itemWeight.SetText("Wt: " + slotScript.GetItem().GetWeight());
		}
		else {
			itemName.SetText("");
			itemDesc.SetText("");
			itemQuantity.SetText("");
			itemWeight.SetText("");
		}
	}
	
	public void CargoScreenUpdate()
	{
		if (inventory.items.Count + inventory.equipments.Count > 25)
		{
			while (cargoSlots.Length < inventory.items.Count + inventory.equipments.Count)
			{
				for (int i = 0; i < 5; i++)
				{
					Instantiate(slotPrefab, cargoSlotBox);
				}
				cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
			}
		}
		 if (cargoSlots.Length > 25)
		{
			for (int i = cargoSlots.Length; i >= 25; i --)
			{
				Destroy(cargoSlotBox.GetChild(i));
			}
			cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
		}
		
		for (int i = 0; i < inventory.equipments.Count; i ++)
		{
			cargoSlots[i].AddItem(inventory.equipments[i]);
		}
		for ( int i = inventory.equipments.Count; i < inventory.items.Count + inventory.equipments.Count; i++)
		{
			cargoSlots[i].AddItem(inventory.items[i-inventory.equipments.Count]);
		}
		
		for (int i = inventory.items.Count + inventory.equipments.Count; i < cargoSlots.Length; i ++)
		{
			cargoSlots[i].ClearSlot();
		}
	}
	
	public void SelectSlot(GameObject slot)
	{
		selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
		selectedSlot = slot;
		selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
		Slot slotScript = selectedSlot.GetComponent<Slot>();
		if (slotScript.GetItem() != null)
		{
			itemName.SetText(slotScript.GetItem().GetName());
			itemDesc.SetText(slotScript.GetItem().GetDescription());
			//itemValue.SetText("Qt: " + slotScript.GetItem().GetValue());
			//itemWeight.SetText("Wt: " + slotScript.GetItem().GetWeight());
		} else
		{
			itemName.SetText("");
			itemDesc.SetText("");
			//itemValue.SetText("");
			//itemWeight.SetText("");
		}
	}
}
