using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New ItemStack", menuName = "Inventory/ItemStack")]
public class ItemStack : ScriptableObject {

	Item item;
	int quantity;
	
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
	
	public int GetPrice()
	{
		return item.CalculatePrice(quantity);
	}

	
}
