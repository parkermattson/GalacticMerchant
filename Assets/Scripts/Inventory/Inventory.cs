using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	
	public static Inventory instance;
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public OnItemChanged onEquipmentChangedCallback;

	public List<Item> items = new List<Item>();
	public List<Equipment> equipments = new List<Equipment>();
	public Equipment[] shipEquipment = new Equipment[8];
	
	void Awake ()
	{
		instance = this;
		
	}
	
	
	
	public void AddItem(Item item)
	{
		if (items.Exists(x => x.itemID == item.itemID))
		{
			items.Find(x => x.itemID == item.itemID).AddQuantity(item.GetQuantity());
		}
		else {
			items.Add(item);
		}
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
	
	public void RemoveItem(Item item)
	{
		int i = items.FindIndex(x => x.itemID == item.itemID);
		if (items[i].GetQuantity() > item.GetQuantity())
			items[i].AddQuantity(-item.GetQuantity());
		else items.Remove(item);
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
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
		Debug.Log("Swapping equipment");
		equipments.Add(shipEquipment[slot]);
		shipEquipment[slot] = equipment;
		equipments.Remove(equipment);
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
		if (onEquipmentChangedCallback != null)
			onEquipmentChangedCallback.Invoke();
	}
}
