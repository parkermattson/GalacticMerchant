using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {RawMat, RefMat, Component, IndGood, ConsGood, Agricultural, Pharma, ExoticGood, Artifact, Combat, Equipment}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	public string itemName = "Name";
	public string itemDesc = "Item Description";
	public Sprite icon = null;
	public int itemTier = 1;
	public ItemType itemType = ItemType.RawMat;
	public int itemValue = 0;
	public int itemWeight = 0;
	public bool isStackable = true;
	public int priceRange, medianQuant, quantityRange;
	
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
	
	public float CalculatePrice(int quantity, int startQuantity)
	{
		int newPriceRange = Mathf.CeilToInt(itemValue *.5f);
		//int price = (int)(itemValue + newPriceRange - 2*newPriceRange/(1+Mathf.Pow(3,(float)(medianQuant - quantity)/(quantityRange/5f))));
		if (startQuantity + quantity < 1)
		{
			quantity = 1-startQuantity;
		}
		float priceA = startQuantity * (itemValue + newPriceRange) - (2f*newPriceRange*(quantityRange/5f)*Mathf.Log(Mathf.Pow(3f, medianQuant/(quantityRange/5f)) + Mathf.Pow(3f, startQuantity/(quantityRange/5f)))/Mathf.Log(3f));
		float priceB = (startQuantity+quantity) * (itemValue + newPriceRange) - (2f*newPriceRange*(quantityRange/5f)*Mathf.Log(Mathf.Pow(3f, medianQuant/(quantityRange/5f)) + Mathf.Pow(3f, (startQuantity + quantity)/(quantityRange/5f)))/Mathf.Log(3f));
		
		if (quantity == 0) quantity=1;
		
		float price = (priceB-priceA)/(float)quantity;
		return price;
	} 
}
