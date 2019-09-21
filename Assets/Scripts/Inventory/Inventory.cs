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
	
	public void AddItem(ItemStack stack)
	{
		stack.AddToList(items);
	}
	
	public void RemoveItem(ItemStack stack)
	{
		stack.RemoveFromList(items);
	}
	
	public bool FindItem(ItemStack stack)
	{
		return stack.FindInList(items);
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
