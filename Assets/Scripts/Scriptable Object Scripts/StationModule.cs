using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Station Module", menuName = "Station Module")]
public class StationModule : ScriptableObject {

	float moduleLevel = 1f;
	public List<Item> drainItems, gainItems;
	public List<int> drainBase, drainInc, gainBase, gainInc, eqBase, eqInc;
	

	public void Refresh(Station station)
	{
		int moneyNeeded = 0;
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		for (int i =0; i < drainItems.Count; i++)
		{
			tempStack.Init(drainItems[i], drainBase[i] + drainInc[i] * (int)moduleLevel);
			if (!tempStack.FindInList(station.marketInv))
			{
				moneyNeeded += drainItems[i].GetValue() * tempStack.GetQuantity();
			}
		}
		
		if (moneyNeeded <= station.stationMoney)
		{
			station.stationMoney-=moneyNeeded;
			for (int i =0; i < drainItems.Count; i++)
			{
				tempStack.Init(drainItems[i], drainBase[i] + drainInc[i] * (int)moduleLevel);
				tempStack.RemoveFromList(station.marketInv);
			}
			for (int i =0; i < gainItems.Count; i++)
			{
				tempStack.Init(gainItems[i], gainBase[i] + gainInc[i] * (int)moduleLevel);
				tempStack.AddToList(station.marketInv);
			}
			moduleLevel+=.25f;
		}
		else if (moduleLevel > 1) moduleLevel-=.25f;
	}
	
}
