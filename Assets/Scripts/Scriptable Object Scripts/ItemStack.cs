using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New ItemStack", menuName = "Inventory/ItemStack")]
public class ItemStack : ScriptableObject {

	public Item item;
	public int quantity;
	
	public int GetQuantity()
	{
		return quantity;
	}
	
	public void AddQuantity(int newQuantity)
	{
		quantity+=newQuantity;
	}

	
}
