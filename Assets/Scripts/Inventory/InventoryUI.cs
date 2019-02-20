using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	Inventory inventory;
	
	public Transform slotBox;
	
	Slot[] slots;

	// Use this for initialization
	void Start () {
		inventory = Inventory.instance;
		inventory.onItemChangedCallback += UpdateUI;
		
		slots = slotBox.GetComponentsInChildren<Slot>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void UpdateUI()
	{
		for (int i = 0; i < slots.Length; i ++)
		{
			if (i <inventory.items.Count)
			{
				slots[i].AddItem(inventory.items[i]);
			}
			else
			{
				slots[i].ClearSlot();
			}
		}
	}
}
