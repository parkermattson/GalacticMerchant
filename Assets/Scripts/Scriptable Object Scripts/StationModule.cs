using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Station Module", menuName = "Station Module")]
public class StationModule : ScriptableObject {

	public float initLevel = 1f;
	float moduleLevel = 1f;
	public int moneyBase, moneyInc;
	public List<Item> drainItems, gainItems;
	public List<int> drainBase, drainInc, gainBase, gainInc, eqBase, eqInc;
	
	public void Init()
	{
		moduleLevel = initLevel;
	}

	public void Refresh(Station station)
	{		
		bool enoughRes = true;
		int moneyNeeded = moneyBase + moneyInc * (int)moduleLevel;
		int productValue = 0, productCost = 0;
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		for (int i =0; i < drainItems.Count; i++)
		{
			tempStack.Init(drainItems[i], drainBase[i] + drainInc[i] * (int)moduleLevel);
			if (!tempStack.FindInList(station.marketInv))
			{
				Debug.Log("Not enough of " + drainItems[i].GetName());
				enoughRes = false;
			} else Debug.Log("Enough of " + drainItems[i].GetName());
			
			int index = station.marketInv.FindIndex(x => x.GetItem() == drainItems[i]);
			if (index != -1)
				productCost += Mathf.CeilToInt((drainBase[i] + drainInc[i] * (int)moduleLevel) * station.marketInv[index].GetPrice(-(drainBase[i] + drainInc[i] * (int)moduleLevel)));
			else {
				tempStack.Init(drainItems[i], 1);
				productCost += Mathf.CeilToInt((drainBase[i] + drainInc[i] * (int)moduleLevel) * tempStack.GetPrice(-(drainBase[i] + drainInc[i] * (int)moduleLevel)));
			}
			
		}
		
		for (int i =0; i < gainItems.Count; i++)
		{
			int index = station.marketInv.FindIndex(x => x.GetItem() == gainItems[i]);
			if (index != -1)
				productValue += Mathf.CeilToInt((gainBase[i] + gainInc[i] * (int)moduleLevel) * station.marketInv[index].GetPrice(gainBase[i] + gainInc[i] * (int)moduleLevel));
			else {
				tempStack.Init(gainItems[i], 1);
				productValue += Mathf.CeilToInt((gainBase[i] + gainInc[i] * (int)moduleLevel) * tempStack.GetPrice(gainBase[i] + gainInc[i] * (int)moduleLevel));
			}
		}
		
		
		if (moneyNeeded*10 <= station.stationMoney && enoughRes)
		{
			Debug.Log("Enough money. Money at: " + station.stationMoney.ToString());
			station.stationMoney-=moneyNeeded;
			for (int i =0; i < drainItems.Count; i++)
			{
				tempStack.Init(drainItems[i], drainBase[i] + drainInc[i] * (int)moduleLevel);
				tempStack.RemoveFromList(station.marketInv);
				tempStack.Init(drainItems[i], eqBase[i] + eqInc[i] * (int)moduleLevel);
			}
			for (int i =0; i < gainItems.Count; i++)
			{
				tempStack.Init(gainItems[i], gainBase[i] + gainInc[i] * (int)moduleLevel);
				tempStack.AddToList(station.marketInv);
			}
			
			int profitMargin = productValue - (productCost + moneyNeeded);
			
			Debug.Log("Value: " + productValue.ToString() + " Cost: " + productCost.ToString() + " Profit %: " + profitMargin.ToString());
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
			Debug.Log("Not enough of money or resources. Money at: " + station.stationMoney.ToString());
			if (moduleLevel > 1)
				moduleLevel -=.25f;
		}
		
		Debug.Log("Module Level " + moduleLevel.ToString());
	}
	
	
}
