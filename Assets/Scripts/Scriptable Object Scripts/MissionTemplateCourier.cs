using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Courier Mission Template", menuName = "Missions/Courier Mission Template")]
public class MissionTemplateCourier : MissionTemplate {

	public int minRange, maxRange;
	public List<Item> cargoItems;
	public List<int> cargoAmt;
	public StationType destinationType;
	
	
	public override Mission GenerateMission(Station startStation) {
		MissionCourier tempMission = MissionCourier.CreateInstance<MissionCourier>();
		tempMission.missionName = missionName;
		tempMission.source = startStation;
		tempMission.missionType = MissionType.Courier;
		tempMission.expireDate = GameControl.instance.gameTime.AddHours(timeToComplete);
		GenerateRewards(tempMission);
		
		for (int i = 0; i < cargoItems.Count; i++)
		{
			ItemStack tempStack = ItemStack.CreateInstance<ItemStack>();
			tempStack.item = cargoItems[i];
			tempStack.quantity = Mathf.FloorToInt(cargoAmt[i] * (1f + (UnityEngine.Random.value - 0.5f) * .5f));
			tempMission.cargoList.Add(tempStack);
		}
		
		
		List<Station> possibleDestinations = new List<Station>();
		foreach (Station s in GameControl.instance.stations)
		{
			float distance = Vector2.Distance(s.mapPosition, startStation.mapPosition);
			if (distance > minRange && distance < maxRange && s.stationType == destinationType)
				possibleDestinations.Add(s);
		}
		int j = 0;
		while (possibleDestinations.Count < 1)
		{
			if (GameControl.instance.stations[j].stationType == destinationType)
			{
				possibleDestinations.Add(GameControl.instance.stations[j]);
			}
			j++;
		}
		tempMission.destination = possibleDestinations[0];
		for (int i = 1; i < possibleDestinations.Count; i++)
		{
			if (UnityEngine.Random.value < 1f/possibleDestinations.Count)
			{
				tempMission.destination = possibleDestinations[i];
				break;
			}
		}
		
		string description = "Bring ";
		for (int i = 0; i < tempMission.cargoList.Count - 1; i ++)
			description = description + tempMission.cargoList[i].GetQuantity().ToString() + " " + tempMission.cargoList[i].GetItem().GetName() + ", ";
		
		tempMission.missionDesc = description + tempMission.cargoList.Last().GetQuantity().ToString() + " " + tempMission.cargoList.Last().GetItem().GetName() + " to " + tempMission.destination.GetName();
		
		return tempMission;
		
	}
	
	
}
