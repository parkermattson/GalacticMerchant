using System.Collections;
using System.Collections.Generic;
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
	
	public Image[] missionIcons = new Image[4];
	
	public void AddMission(Mission newMission)
	{
		string rewardString;
		mission = newMission;
		icon = missionIcons[(int)mission.missionType];
		nameText.text = mission.missionName;
		descText.text = mission.missionDesc;
		locationText.text = "Location not added yet";
		rewardString = mission.rewardMoney.ToString() + " SB";
		for (int i = 0; i < mission.rewardItems.Count; i++)
		{
			rewardString = rewardString + ", " + mission.rewardItems[i].GetName();
		}
		for (int i = 0; i < mission.rewardEquips.Count; i++)
		{
			rewardString = rewardString + ", " + mission.rewardEquips[i].GetName();
		}
		rewardText.text = rewardString;
	}
	
	public Mission GetMission()
	{
		return mission;
	}
	
}
