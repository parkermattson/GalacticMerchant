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
		switch (slot)
		{
			case 0:
			if (GameControl.instance.playerShip.command != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.command);
				currentCargo += GameControl.instance.playerShip.command.GetWeight();
			}
			
			GameControl.instance.playerShip.command = (Command)equipment;
			equipments.Remove(equipment);
			currentCargo -= equipment.GetWeight();
			break;
			
			case 1:
			if (GameControl.instance.playerShip.weapon != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.weapon);
				currentCargo += GameControl.instance.playerShip.weapon.GetWeight();
			}
			
			GameControl.instance.playerShip.weapon = (Weapon)equipment;
			equipments.Remove(equipment);
			currentCargo -= equipment.GetWeight();
			break;
			
			case 2:
			if (GameControl.instance.playerShip.sensor != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.sensor);
				currentCargo += GameControl.instance.playerShip.sensor.GetWeight();
			}
			
			GameControl.instance.playerShip.sensor = (Sensor)equipment;
			equipments.Remove(equipment);
			currentCargo -= equipment.GetWeight();
			break;
			
			case 3:
			if (GameControl.instance.playerShip.engine != null)
			{
				equipments.Add((Equipment)GameControl.instance.playerShip.engine);
				currentCargo += GameControl.instance.playerShip.engine.GetWeight();
			}
			
			GameControl.instance.playerShip.engine = (Engine)equipment;
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
