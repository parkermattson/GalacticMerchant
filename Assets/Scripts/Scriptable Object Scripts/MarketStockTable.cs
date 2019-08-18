﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Market Stock", menuName = "LootTable/MarketStock")]
public class MarketStockTable : ScriptableObject {
	
	public List<Item> items;
	public List<float> availableChance;
	public List<int> minQuantity, maxQuantity;
	
	public List<ItemStack> GenerateStock()
	{
		List<ItemStack> stock = new List<ItemStack>();
		for (int i =0; i < items.Count; i++)
		{
			if (Random.value < availableChance[i])
			{
				ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
				stock.Add(tempStack.Init(items[i], minQuantity[i] + (int)(Random.value * maxQuantity[i])));
			}
		}
		return stock;
	}
}
