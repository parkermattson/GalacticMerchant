using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Price Table", menuName = "LootTable/PriceTable")]
public class MarketPriceTable : ScriptableObject {

	public List<int> itemTypeMultiplier;

	public int AdjustPrice(ItemStack stack)
	{
		int i = (int)stack.GetItem().GetItemType();
		int price = stack.GetPrice() * itemTypeMultiplier[i];
		return price;
	}
	
}
