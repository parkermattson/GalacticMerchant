using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New ItemStack", menuName = "Inventory/ItemStack")]
public class ItemStack : ScriptableObject {

	public Item item;
	public int quantity;
	
	public Item GetItem()
	{
		return item;
	}
	
	public int GetQuantity()
	{
		return quantity;
	}
	
	public ItemStack Init(Item itemInit, int quantityInit)
	{
		item = itemInit;
		quantity = quantityInit;
		return this;
	}
	
	public void AddQuantity(int newQuantity)
	{
		quantity+=newQuantity;
	}
	
	public float GetPrice(int addedQuantity)
	{
		float price = item.CalculatePrice(addedQuantity, quantity);
		return price;
	}
	
	public void AddToList(List<ItemStack> itemList)
	{
		if (itemList.Exists(x => x.GetItem() == item))
		{
			itemList.Find(x => x.GetItem() == item).AddQuantity(quantity);
		}
		else {
			ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
			tempStack.Init(item, quantity);
			itemList.Add(tempStack);
		}
	}
		
	public void RemoveFromList(List<ItemStack> itemList)
	{
		int index = itemList.FindIndex(x => x.GetItem() == item);
		if (index != -1)
		{
			if (itemList[index].GetQuantity() > quantity)
			{
				itemList[index].AddQuantity(-quantity);
			}
			else itemList.RemoveAt(index);
		}
	}
	
	public bool FindInList(List<ItemStack> itemList)
	{
		int index = itemList.FindIndex(x => x.GetItem() == item);
		if (index != -1)
		{
			if (itemList[index].GetQuantity() >= quantity)
			{
				return true;
			}
			else return false;
		}
		else {
			return false;
		}
	}
	
	public int GetWeight()
	{
		return item.GetWeight() * quantity;
	}

	
}
