using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	
	public static Inventory instance;
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public OnItemChanged onEquipmentChangedCallback;

	public List<ItemStack> items = new List<ItemStack>();
	public List<Equipment> equipments = new List<Equipment>();
	public Equipment[] shipEquipment = new Equipment[8];
	
	void Awake ()
	{
		instance = this;
		
	}
	
	
	
	public void AddItem(ItemStack newItem)
	{
		if (items.Exists(x => x.item.GetID() == newItem.item.GetID()))
		{
			items.Find(x => x.item.GetID() == newItem.item.GetID()).AddQuantity(newItem.GetQuantity());
		}
		else {
			ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
			tempStack.item = newItem.item;
			tempStack.quantity = newItem.quantity;
			items.Add(tempStack);
		}
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
	
	public void RemoveItem(ItemStack remItem)
	{
		int index = items.FindIndex(x => x.item.GetID() == remItem.item.GetID());
		if (index != -1)
		{
			if (items[index].GetQuantity() > remItem.GetQuantity())
			{
				items[index].AddQuantity(-remItem.GetQuantity());
			}
			else items.RemoveAt(index);
		}
	}
	
	public void AddEquipment(Equipment equipment)
	{
		equipments.Add(equipment);
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
		if (onEquipmentChangedCallback != null)
			onEquipmentChangedCallback.Invoke();
	}
	
	public void RemoveEquipment(Equipment equipment)
	{
		equipments.Remove(equipment);
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
		if (onEquipmentChangedCallback != null)
			onEquipmentChangedCallback.Invoke();
	}
	
	public void SwapEquipment(Equipment equipment, int slot)
	{
		equipments.Add(shipEquipment[slot]);
		shipEquipment[slot] = equipment;
		equipments.Remove(equipment);
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
		if (onEquipmentChangedCallback != null)
			onEquipmentChangedCallback.Invoke();
	}
}
