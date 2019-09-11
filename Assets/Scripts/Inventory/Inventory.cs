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
		if (items.Exists(x => x.GetItem().GetID() == newItem.GetItem().GetID()))
		{
			items.Find(x => x.GetItem().GetID() == newItem.GetItem().GetID()).AddQuantity(newItem.GetQuantity());
		}
		else {
			ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
			tempStack.Init(newItem.GetItem(), newItem.GetQuantity());
			items.Add(tempStack);
		}
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
	
	public void RemoveItem(ItemStack remItem)
	{
		int index = items.FindIndex(x => x.GetItem().GetID() == remItem.GetItem().GetID());
		if (index != -1)
		{
			if (items[index].GetQuantity() > remItem.GetQuantity())
			{
				items[index].AddQuantity(-remItem.GetQuantity());
			}
			else items.RemoveAt(index);
		}
	}
	
	public bool FindItem(ItemStack refItem)
	{
		int index = items.FindIndex(x => x.GetItem() == refItem.GetItem());
		if (index != -1)
		{
			if (items[index].GetQuantity() >= refItem.GetQuantity())
			{
				return true;
			}
			else return false;
		}
		else {
			return false;
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
