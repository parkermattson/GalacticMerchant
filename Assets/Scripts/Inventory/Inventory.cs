using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	
	public static Inventory instance;
	
	void Awake ()
	{
		instance = this;
		
	}
	
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	public List<Item> items = new List<Item>();
	public List<Equipment> equipments = new List<Equipment>();
	public Equipment[] shipEquipment = new Equipment[8];
	
	public void AddItem(Item item)
	{
		items.Add(item);
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
	
	public void RemoveItem(Item item)
	{
		items.Remove(item);
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
	
	public void AddEquipment(Equipment equipment)
	{
		equipments.Add(equipment);
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
	
	public void RemoveEquipment(Equipment equipment)
	{
		equipments.Remove(equipment);
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
	
	public void SwapEquipment(Equipment equipment, int slot)
	{
		Debug.Log("Swapping equipment");
		equipments.Add(shipEquipment[slot]);
		shipEquipment[slot] = equipment;
		equipments.Remove(equipment);
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
	}
}
