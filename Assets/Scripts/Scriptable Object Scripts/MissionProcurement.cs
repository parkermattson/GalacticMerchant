using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionProcurement : Mission {

	public List<ItemStack>  cargoList = new List<ItemStack>();
	public Station destination;
	
	/*public string GetDesc() {
		string description = "Acquire ";
		for (int i = 0; i < cargoList.Count - 1; i ++)
		{
			description = description + cargoList[i].GetQuantity().ToString() + " " + cargoList[i].GetItem().GetName() + ", ";
		}
		description = description + "and " + cargoList.Last().GetQuantity().ToString() + " " + cargoList.Last().GetItem().GetName() + " and bring to " + destination.GetName();
	return description; }
	*/
	
}
