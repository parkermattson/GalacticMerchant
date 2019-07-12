using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {RawMat, RefMat, Component, IndGood, ConsGood, ExoticGood,Equipment}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	public int itemID = 0;
	public string itemName = "Name";
	public string itemDesc = "Item Description";
	public Sprite icon = null;
	public int itemTier = 1;
	public ItemType itemType = ItemType.RawMat;
	public int itemValue = 0;
	public int itemWeight = 0;
	public bool isStackable = true;
	
	public int GetID()
	{
		return itemID;
	}
	
	public string GetName()
	{
		return itemName;
	}
	
	public string GetDescription()
	{
		return itemDesc;
	}
	
	public int GetTier()
	{
		return itemTier;
	}
	
	public int GetValue()
	{
		return itemValue;
	}
	
	public int GetWeight()
	{
		return itemWeight;
	}
}
