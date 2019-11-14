using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	
	public static Inventory instance;
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public OnItemChanged onEquipmentChangedCallback;

	public int currentCargo = 0;
	public List<ItemStack> items = new List<ItemStack>();
	public List<Equipment> equipments = new List<Equipment>();
	public Equipment[] shipEquipment = new Equipment[8];
	
	void Awake ()
	{
		instance = this;
		foreach (ItemStack stack in items)
		{
			currentCargo += stack.GetWeight();
		}
	}
	
	public void AddItem(ItemStack stack)
	{
		stack.AddToList(items);
		currentCargo += stack.GetWeight();
	}
	
	public void RemoveItem(ItemStack stack)
	{
		stack.RemoveFromList(items);
		currentCargo -= stack.GetWeight();
	}
	
	public bool FindItem(ItemStack stack)
	{
		return stack.FindInList(items);
	}
	
	public void AddEquipment(Equipment equipment)
	{
		equipments.Add(equipment);
		currentCargo += equipment.GetWeight();
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
		if (onEquipmentChangedCallback != null)
			onEquipmentChangedCallback.Invoke();
	}
	
	public void RemoveEquipment(Equipment equipment)
	{
		equipments.Remove(equipment);
		currentCargo -= equipment.GetWeight();
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
		if (onEquipmentChangedCallback != null)
			onEquipmentChangedCallback.Invoke();
	}
	
	public void SwapEquipment(Equipment equipment, int slot)
	{
		if (shipEquipment[slot] != null)
		{
			equipments.Add(shipEquipment[slot]);
			currentCargo += shipEquipment[slot].GetWeight();
		}
		
		shipEquipment[slot] = equipment;
		equipments.Remove(equipment);
		currentCargo -= equipment.GetWeight();
		
		if (onItemChangedCallback != null)
			onItemChangedCallback.Invoke();
		if (onEquipmentChangedCallback != null)
			onEquipmentChangedCallback.Invoke();
	}
	
	public bool EnoughSpace(ItemStack stack)
	{
		if (stack.GetWeight() > GameControl.instance.playerShip.GetCargoMax() - currentCargo)
		{
			return false;
		}
		else return true;
	}
}
