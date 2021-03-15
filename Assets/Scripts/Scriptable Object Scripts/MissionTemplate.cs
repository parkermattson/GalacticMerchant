using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class MissionTemplate : ScriptableObject {

	public string missionName;
	public int rewardValue = 0, numOfRewards = 1, timeToComplete;
	public List<Item> rewardItems;
	public List<Equipment> rewardEquips;
	
	public abstract Mission GenerateMission(Station startStation);
	
	protected void GenerateRewards(Mission mission) {
		int rewardsLeft = numOfRewards, possLeft = rewardItems.Count + rewardEquips.Count, currentValue = 0;
		for (int i =0; i < rewardItems.Count; i++)
		{
			if (UnityEngine.Random.value < (float)rewardsLeft/ possLeft)
			{
				ItemStack tempStack = ItemStack.CreateInstance<ItemStack>();
				tempStack.item = rewardItems[i];
				tempStack.quantity = Mathf.FloorToInt((rewardValue * (1f - (UnityEngine.Random.value - 0.5f)*.1f)) / (numOfRewards * rewardItems[i].itemValue));
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
		if (currentValue < rewardValue)
		{
			mission.rewardMoney = ((rewardValue - currentValue) % 100) * 100;
		}
	}
}
