using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AcceptedMissionSlot : MonoBehaviour {

	public Mission mission;
	
	public Image icon;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI locationText;
	public TextMeshProUGUI descText;
	public TextMeshProUGUI rewardText;
	public Button turnInButton, deliverButton;
	
	public void AddMission(Mission newMission)
	{
		string rewardString = "";
		mission = newMission;
		nameText.text = mission.missionName;
		descText.text = mission.GetDesc();
		locationText.text = "Location: " + mission.source.GetName();
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
		
		CheckMissionStatus();
		
	}
	
	void CheckMissionStatus()
	{
		if (mission.completed) turnInButton.interactable = true;
		else turnInButton.interactable = false;
		
		switch (mission.missionType)
		{
			case MissionType.Courier:
				MissionCourier courMission = (MissionCourier)mission;
				bool ready = true;
				foreach (ItemStack stack in courMission.cargoList)
				{
					if (!Inventory.instance.FindItem(stack))
						ready = false;
				}
				if (!ready)
				{
					deliverButton.gameObject.SetActive(true);
					deliverButton.interactable = false;
				}
				break;
			default: break;
		}
	}
	
	public void DeliverCargo()
	{
			MissionCourier courMission = (MissionCourier)mission;
			foreach (ItemStack stack in courMission.cargoList)
			{
				Inventory.instance.RemoveItem(stack);
			}
			courMission.completed = true;
			CheckMissionStatus();
	}
	
	public void TurnInMission()
	{
		GameControl.instance.playerMoney += mission.rewardMoney;
		foreach (ItemStack stack in mission.rewardItems)
			Inventory.instance.AddItem(stack);
		
		foreach (Equipment equip in mission.rewardEquips)
			Inventory.instance.AddEquipment(equip);
		
		GameControl.instance.acceptedMissions.Remove(mission);
		GetComponentInParent<AcademyScreenScript>().UpdateMissionList();
	}
	
	public Mission GetMission()
	{
		return mission;
	}
	
}
