using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {RawMat, RefMat, Component, IndGood, ConsGood, Agricultural, Pharma, ExoticGood, Artifact, Combat, Equipment}

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
	public int priceRange, medianQuant, quantityRange;
	
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
	
	public ItemType GetItemType()
	{
		return itemType;
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
	
	public int CalculatePrice(int quantity)
	{
		int price = (int)(itemValue + priceRange/(1+Mathf.Pow(3,(float)(medianQuant - quantity)/(quantityRange/5f))));
		return price;
	} 
}
