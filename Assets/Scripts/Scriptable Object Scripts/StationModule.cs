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
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		for (int i =0; i < drainItems.Count; i++)
		{
			tempStack.Init(drainItems[i], drainBase[i] + drainInc[i] * (int)moduleLevel);
			if (!tempStack.FindInList(station.marketInv))
			{
				Debug.Log("Not enough of " + drainItems[i].GetName());
				enoughRes = false;
			} else Debug.Log("Enough of " + drainItems[i].GetName());
		}
		
		enoughRes = true;
		if (moneyNeeded*10 <= station.stationMoney && enoughRes)
		{
			Debug.Log("Enough money. Money at: " + station.stationMoney.ToString());
			station.stationMoney-=moneyNeeded;
			for (int i =0; i < drainItems.Count; i++)
			{
				tempStack.Init(drainItems[i], drainBase[i] + drainInc[i] * (int)moduleLevel);
				tempStack.RemoveFromList(station.marketInv);
				tempStack.Init(drainItems[i], eqBase[i] + eqInc[i] * (int)moduleLevel);
				if (tempStack.FindInList(station.marketInv))
				{
					moduleLevel += .25f;
				}
			}
			for (int i =0; i < gainItems.Count; i++)
			{
				tempStack.Init(gainItems[i], gainBase[i] + gainInc[i] * (int)moduleLevel);
				tempStack.AddToList(station.marketInv);
			}
			
			moduleLevel+=.25f;
		}
		else {
			if (moduleLevel > 1) moduleLevel-=.25f;
			Debug.Log("Not enough of money or resources. Money at: " + station.stationMoney.ToString());
		}
		
		Debug.Log("Module Level" + moduleLevel.ToString());
	}
	
}
