using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Procurement Mission Template", menuName = "Missions/Procurement Mission Template")]
public class MissionTemplateProcurement : MissionTemplate {

	public int minRange, maxRange;
	public List<Item> cargoItems;
	public List<int> cargoAmt;
	public StationType destinationType;

	public override Mission GenerateMission(Station startStation) {
		MissionProcurement tempMission = MissionProcurement.CreateInstance<MissionProcurement>();
		tempMission.missionName = missionName;
		tempMission.source = startStation;
		tempMission.missionType = MissionType.Procurement;
		tempMission.expireDate = GameControl.instance.gameTime.AddHours(timeToComplete);
		GenerateRewards(tempMission);
		
		int cargoValue = 0;
		for (int i = 0; i < cargoItems.Count; i++)
		{
			ItemStack tempStack = ItemStack.CreateInstance<ItemStack>();
			tempStack.item = cargoItems[i];
			tempStack.quantity = Mathf.FloorToInt(cargoAmt[i] * (1f + (UnityEngine.Random.value - 0.5f) * .5f));
			tempMission.cargoList.Add(tempStack);
			cargoValue += tempStack.item.GetValue() * tempStack.quantity;
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
		
		string description = "Acquire and bring ";
		for (int i = 0; i < tempMission.cargoList.Count - 1; i ++)
			description = description + tempMission.cargoList[i].GetQuantity().ToString() + " " + tempMission.cargoList[i].GetItem().GetName() + ", ";
		
		tempMission.missionDesc = description + " and " + tempMission.cargoList.Last().GetQuantity().ToString() + " " + tempMission.cargoList.Last().GetItem().GetName() + " to " + tempMission.destination.GetName();
		
		return tempMission;
		
	}
	
	protected void GenerateRewards(Mission mission, int cargoValue) {
		int rewardsLeft = numOfRewards, possLeft = rewardItems.Count + rewardEquips.Count, currentValue = 0;
		for (int i =0; i < rewardItems.Count; i++)
		{
			if (UnityEngine.Random.value < (float)rewardsLeft/ possLeft)
			{
				ItemStack tempStack = new ItemStack();
				tempStack.item = rewardItems[i];
				tempStack.quantity = Mathf.FloorToInt(((rewardValue+cargoValue) * (1f - (UnityEngine.Random.value - 0.5f)*.1f)) / (numOfRewards * rewardItems[i].itemValue));
				mission.rewardItems.Add(tempStack);
				currentValue += tempStack.item.itemValue * tempStack.quantity;
				rewardsLeft--;
			}
			possLeft--;
		}
		if (rewardsLeft > 0)
		{
			for (int i =0; i < rewardEquips.Count; i++)
			{
				if (UnityEngine.Random.value < (float)rewardsLeft/ possLeft)
				{
					mission.rewardEquips.Add(rewardEquips[i]);
					currentValue+=rewardEquips[i].itemValue;
					rewardsLeft--;
				}
				possLeft--;
			}
		}
		if (currentValue < rewardValue + cargoValue)
		{
			mission.rewardMoney = ((rewardValue + cargoValue - currentValue) % 100) * 100;
		}
	}
	
}
