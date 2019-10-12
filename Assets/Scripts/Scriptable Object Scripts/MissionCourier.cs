using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Courier Mission", menuName = "Courier Mission")]
public class MissionCourier : Mission {

	public List<ItemStack>  cargoList = new List<ItemStack>();
	
	public override string GetDesc() {
		string description = "Bring ";
		for (int i = 0; i < cargoList.Count - 1; i ++)
		{
			description = description + cargoList[i].GetQuantity().ToString() + " " + cargoList[i].GetItem().GetName() + ", ";
		}
		description = description + cargoList.Last().GetQuantity().ToString() + " " + cargoList.Last().GetItem().GetName() + " to " + destination.GetName();
	return description; }
	
	
}
