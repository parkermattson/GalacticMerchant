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
	public Button acceptButton;
	
	public void AddMission(Mission newMission)
	{
		string rewardString = "";
		mission = newMission;
		nameText.text = mission.missionName;
		descText.text = mission.GetDesc();
		locationText.text = mission.source.GetName();
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
		
		if (GameControl.instance.acceptedMissions.Count >= 5 || !CourierCargoCheck())
			acceptButton.interactable = false;
	}
	
	public Mission GetMission()
	{
		return mission;
	}
	
	public void AcceptMission()
	{
		GameControl.instance.acceptedMissions.Add(mission);
		if (mission.missionType == MissionType.Courier)
		{
			MissionCourier cMission = (MissionCourier)mission;
			foreach (ItemStack stack in cMission.cargoList)
			{
				Inventory.instance.AddItem(stack);
			}
		}
		GetComponentInParent<AcademyScreenScript>().availableMissions.Remove(mission);
		GetComponentInParent<AcademyScreenScript>().UpdateMissionList();
	}
	
	bool CourierCargoCheck()
	{
		int cargoTotal = 0;
		if (mission.missionType == MissionType.Courier)
		{
			MissionCourier cMission = (MissionCourier)mission;
			foreach (ItemStack stack in cMission.cargoList)
			{
				cargoTotal+=stack.GetWeight();
			}
			if (cargoTotal <= GameControl.instance.playerShip.GetCargoMax() - Inventory.instance.currentCargo)
			{
				return true;
			}
			else return false;
		} else return true;
	}
	
}
