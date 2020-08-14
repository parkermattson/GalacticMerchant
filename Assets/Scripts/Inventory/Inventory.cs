using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
	
	public static Inventory instance;
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;
	public OnItemChanged onEquipmentChangedCallback;

	public int currentCargo = 0;
	public List<ItemStack> items;
	public List<Equipment> equipments;
	
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
	
	public void SwapEquipment(Equipment equipment, int slot, int subslot)
	{
		switch (slot)
		{
			case 0:
			if (GameControl.instance.playerShip.commandList[subslot] != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.commandList[subslot]);
				currentCargo += GameControl.instance.playerShip.commandList[subslot].GetWeight();
			}
			
			GameControl.instance.playerShip.commandList[subslot] = (Command)equipment;
			equipments.Remove(equipment);
			currentCargo -= equipment.GetWeight();
			break;
			
			case 1:
			if (GameControl.instance.playerShip.combatList[subslot] != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.combatList[subslot]);
				currentCargo += GameControl.instance.playerShip.combatList[subslot].GetWeight();
			}
			
			GameControl.instance.playerShip.combatList[subslot] = (Combat)equipment;
			equipments.Remove(equipment);
			currentCargo -= equipment.GetWeight();
			break;
			
			case 2:
			if (GameControl.instance.playerShip.sensorList[subslot] != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.sensorList[subslot]);
				currentCargo += GameControl.instance.playerShip.sensorList[subslot].GetWeight();
			}
			
			GameControl.instance.playerShip.sensorList[subslot] = (Sensor)equipment;
			equipments.Remove(equipment);
			currentCargo -= equipment.GetWeight();
			break;
			
			case 3:
			if (GameControl.instance.playerShip.engineList[subslot] != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.engineList[subslot]);
				currentCargo += GameControl.instance.playerShip.engineList[subslot].GetWeight();
			}
			
			GameControl.instance.playerShip.engineList[subslot] = (Engine)equipment;
			equipments.Remove(equipment);
			currentCargo -= equipment.GetWeight();
			break;
		}
		
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
