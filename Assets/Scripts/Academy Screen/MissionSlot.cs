using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionSlot : MonoBehaviour {

	public Mission mission;
	
	public Image icon;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI locationText;
	public TextMeshProUGUI descText;
	public TextMeshProUGUI rewardText;
	
	public void AddMission(Mission newMission)
	{
		string rewardString = "";
		mission = newMission;
		nameText.text = mission.missionName;
		descText.text = mission.GetDesc();
		locationText.text = mission.destination.GetName();
		if (mission.rewardMoney > 0) {
			rewardString = mission.rewardMoney.ToString() + " SB";
			if (mission.rewardItems.Count > 0 || mission.rewardEquips.Count > 0)
				rewardString = rewardString + ",  ";
		}	
		if (mission.rewardItems.Count > 0)
		{
			for (int i = 0; i < mission.rewardItems.Count-1; i++)
			{
				rewardString = rewardString + mission.rewardItems[i].GetQuantity().ToString() + " " + mission.rewardItems[i].GetItem().GetName() + ", ";
			}
			rewardString = rewardString + mission.rewardItems.Last().GetQuantity().ToString() + " " + mission.rewardItems.Last().GetItem().GetName();
			if (mission.rewardEquips.Count > 0)
				rewardString = rewardString + ", ";
		}
		for (int i = 0; i < mission.rewardEquips.Count; i++)
		{
			rewardString = rewardString + mission.rewardEquips[i].GetName();
		}
		rewardText.text = rewardString;
		
	}
	
	public Mission GetMission()
	{
		return mission;
	}
	
}
