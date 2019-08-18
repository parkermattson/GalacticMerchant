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
	
	void Start()
	{
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += CargoScreenUpdate;
		
		cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
	}
	
	void OnEnable()
	{
		inventory = Inventory.instance;
		CargoScreenUpdate();
		
		Slot slotScript = selectedSlot.GetComponent<Slot>();
		if (slotScript.GetItem() != null)
		{
			itemName.SetText(slotScript.GetItem().GetName());
			itemDesc.SetText(slotScript.GetItem().GetDescription());
			itemValue.SetText("Value: " + slotScript.GetItem().GetValue());
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
		cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
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
			ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
			tempStack.Init(inventory.equipments[i], 1);
			cargoSlots[i].AddItem(tempStack);
		}
		for ( int i = inventory.equipments.Count; i < inventory.items.Count + inventory.equipments.Count; i++)
		{
			cargoSlots[i].AddItem(inventory.items[i-inventory.equipments.Count]);
		}
		
		for (int i = inventory.items.Count + inventory.equipments.Count; i < cargoSlots.Length; i ++)
		{
			cargoSlots[i].ClearSlot();
		}
		
		SelectSlot(selectedSlot);
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
			int tempInt = slotScript.GetItem().GetValue() * slotScript.GetStack().GetQuantity();
			itemValue.SetText("Value: " + tempInt);
			tempInt = slotScript.GetItem().GetWeight() * slotScript.GetStack().GetQuantity();
			itemWeight.SetText("Wt: " + tempInt);
			tempInt = slotScript.GetStack().GetQuantity();
			itemQuantity.SetText("Qt: " + tempInt);
		} else
		{
			itemName.SetText("");
			itemDesc.SetText("");
			itemValue.SetText("");
			itemWeight.SetText("");
			itemQuantity.SetText("");
		}
	}
}
