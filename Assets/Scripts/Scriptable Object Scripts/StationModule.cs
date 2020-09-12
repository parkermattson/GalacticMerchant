using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Station Module", menuName = "Station Module")]
public class StationModule : ScriptableObject {

	public float initLevel = 1f;
	public float moduleLevel = 1f;
	public int moneyBase;
	public List<Item> drainItems, gainItems;
	public List<int> drainBase, gainBase;
	
	public void Init() {
		moduleLevel = initLevel;
	}

	public bool Refresh(Station station) {		
		bool enoughRes = true;
		int moneyNeeded = (int)(moneyBase * (1 + (int)moduleLevel*.1f));
		int productValue = 0, productCost = 0;
		float oldLevel = moduleLevel;
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		for (int i =0; i < drainItems.Count; i++)
		{
			tempStack.Init(drainItems[i], (int)(drainBase[i] * (1+(int)moduleLevel*.1f)));
			if (!tempStack.FindInList(station.marketInv))
			{
				Debug.Log("Not enough of " + drainItems[i].GetName());
				enoughRes = false;
			} //else Debug.Log("Enough of " + drainItems[i].GetName());
			
			int index = station.marketInv.FindIndex(x => x.GetItem() == drainItems[i]);
			if (index != -1)
				productCost += Mathf.CeilToInt((int)(drainBase[i] * (1+(int)moduleLevel*.1f)) * station.marketInv[index].GetPrice(-(int)(drainBase[i] * (1+(int)moduleLevel*.1f))));
			else {
				tempStack.Init(drainItems[i], 1);
				productCost += Mathf.CeilToInt((int)(drainBase[i] * (1+(int)moduleLevel*.1f)) * tempStack.GetPrice(-(int)(drainBase[i] * (1+(int)moduleLevel*.1f))));
			}
			
		}
		
		for (int i =0; i < gainItems.Count; i++)
		{
			int index = station.marketInv.FindIndex(x => x.GetItem() == gainItems[i]);
			if (index != -1)
				productValue += Mathf.CeilToInt((int)(gainBase[i] * (1+(int)moduleLevel*.1f)) * station.marketInv[index].GetPrice((int)(gainBase[i] * (1+(int)moduleLevel*.1f))));
			else {
				tempStack.Init(gainItems[i], 1);
				productValue += Mathf.CeilToInt((int)(gainBase[i] * (1+(int)moduleLevel*.1f)) * tempStack.GetPrice((int)(gainBase[i] * (1+(int)moduleLevel*.1f))));
			}
		}
		
		
		if (moneyNeeded*5 <= station.stationMoney && enoughRes)
		{
			//Debug.Log("Enough money. Money at: " + station.stationMoney.ToString());
			station.stationMoney-=moneyNeeded;
			for (int i =0; i < drainItems.Count; i++)
			{
				tempStack.Init(drainItems[i], (int)(drainBase[i] * (1+(int)moduleLevel*.1f)));
				tempStack.RemoveFromList(station.marketInv);
			}
			for (int i =0; i < gainItems.Count; i++)
			{
				tempStack.Init(gainItems[i], (int)(gainBase[i] * (1+(int)moduleLevel*.1f)));
				tempStack.AddToList(station.marketInv);
			}
			
			int profitMargin = productValue - (productCost + moneyNeeded);
			
			//Debug.Log("Value: " + productValue.ToString() + " Cost: " + productCost.ToString() + " Profit: " + profitMargin.ToString());
			if (moneyNeeded > 0)
			{
				if ((float)(profitMargin)/(float)productValue > .15f)
				moduleLevel +=.25f;
				else if ((float)(profitMargin)/(float)productValue < 0 && moduleLevel > 1)
				 moduleLevel -=.25f;
			} else {
				if ((float)(profitMargin)/(float)(productValue - moneyNeeded) > .15f)
				moduleLevel +=.25f;
			 else if ((float)(profitMargin)/(float)(productValue - moneyNeeded) < 0 && moduleLevel > 1)
				 moduleLevel -=.25f;
			}
			 
		} else {
			//Debug.Log("Not enough of money or resources. Money at: " + station.stationMoney.ToString());
			if (moduleLevel > 1)
				moduleLevel -=.25f;
		}
		
		//Debug.Log("Module Level " + moduleLevel.ToString());
		
		if (Mathf.Floor(moduleLevel) - Mathf.Floor(oldLevel) != 0)
			return true;
		
		return false;
		
	}
	
	
}
