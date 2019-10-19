using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Price Table", menuName = "LootTable/PriceTable")]
public class MarketPriceTable : ScriptableObject {

	Dictionary<string, float> adjustedItems = new Dictionary<string, float>();

	public float AdjustPrice(ItemStack stack, int quantity)
	{
		float multiplier = 1f;
		if (adjustedItems.ContainsKey(stack.GetItem().GetName()))
			multiplier = adjustedItems[stack.GetItem().GetName()];
		
		float price = stack.GetPrice(quantity) * multiplier;
		return price;
	}
	
	public void AddDrain(Item drain)
	{
		if (adjustedItems.ContainsKey(drain.GetName()))
		{
			if (adjustedItems[drain.GetName()] == .9f)
				adjustedItems[drain.GetName()] = 1f;
		}
		else {
			adjustedItems[drain.GetName()] = 1.1f;
		}
	}
	
	public void AddGain(Item gain)
	{
		if (adjustedItems.ContainsKey(gain.GetName()))
		{
			if (adjustedItems[gain.GetName()] == 1.1f)
				adjustedItems[gain.GetName()] = 1f;
		}
		else {
			adjustedItems[gain.GetName()] = 1f;
		}
	}
	
}
