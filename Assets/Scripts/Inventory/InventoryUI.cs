using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	Inventory inventory;
	
	public Transform slotPrefab;
	public Transform shipSlotPrefab;
	public Transform cargoSlotBox;
	public Transform shipEquipSlotBox;
	
	Slot[] cargoSlots;

	// Use this for initialization
	void Start () {
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
		
		cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
		
		inventory.onItemChangedCallback.Invoke();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void UpdateUI()
	{
		//Cargo Screen Update
		if (inventory.items.Count > 25)
		{
			while (cargoSlots.Length < inventory.items.Count)
			{
				for (int i = 0; i < 5; i++)
				{
					Instantiate(slotPrefab, cargoSlotBox);
				}
				cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
			}
		}
		else if (cargoSlots.Length > 25)
		{
			for (int i = cargoSlots.Length; i >= 25; i --)
			{
				Destroy(cargoSlotBox.GetChild(i));
			}
			cargoSlots = cargoSlotBox.GetComponentsInChildren<Slot>();
		}
		
		for (int i = 0; i < cargoSlots.Length; i ++)
		{
			if (i <inventory.items.Count)
			{
				cargoSlots[i].AddItem(inventory.items[i]);
			}
			else
			{
				cargoSlots[i].ClearSlot();
			}
		}
		
		//Ship Screen Update
		
	}
}
