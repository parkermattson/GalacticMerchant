using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	public string itemName = "Name";
	public string itemDesc = "Item Description";
	public Sprite icon = null;
	public int itemValue = 0;
	public int itemWeight = 0;
	
	public string GetName()
	{
		return itemName;
	}
	
	public string GetDescription()
	{
		return itemDesc;
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
