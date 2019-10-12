using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcademyScreenScript : MonoBehaviour {
	
	public GameObject missionBox;
	public GameObject missionPrefab;
	public GameObject playerMissionBox;
	public GameObject playerMissionPrefab;
	public MissionTable allMissions;
	public List<Mission> availableMissions;
	
	public Item testItem1, testItem2, testItem3;
	
	public GameObject recruitBox;
	public GameObject crewPrefab;
	Station station;
	
	void OnEnable()
	{
		station = (Station)GameControl.instance.playerLocation;
		GenerateMissionList();
		UpdateMissionList();
		UpdateRecruitmentList();
	}
	
	void GenerateMissionList()
	{
		MissionCourier mission1 = ScriptableObject.CreateInstance<MissionCourier>();
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		mission1.missionName = "Courier Mission 1";
		mission1.missionType = MissionType.Courier;
		mission1.rewardMoney = 10000;
		mission1.cargoList.Add(tempStack.Init(testItem1, 100));
		tempStack = ScriptableObject.CreateInstance<ItemStack>();
		mission1.cargoList.Add(tempStack.Init(testItem2, 500));
		tempStack = ScriptableObject.CreateInstance<ItemStack>();
		mission1.rewardItems.Add(tempStack.Init(testItem3, 1000));
		mission1.destination = station;
		availableMissions.Add(mission1);
		
		MissionCourier mission2 = ScriptableObject.CreateInstance<MissionCourier>();
		tempStack = ScriptableObject.CreateInstance<ItemStack>();
		mission2.missionName = "Courier Mission 2";
		mission2.missionType = MissionType.Courier;
		mission2.rewardMoney = 5000;
		mission2.cargoList.Add(tempStack.Init(testItem2, 300));
		tempStack = ScriptableObject.CreateInstance<ItemStack>();
		mission2.cargoList.Add(tempStack.Init(testItem3, 400));
		tempStack = ScriptableObject.CreateInstance<ItemStack>();
		mission2.rewardItems.Add(tempStack.Init(testItem1, 500));
		mission2.destination = station;
		availableMissions.Add(mission2);
	}
	

	void UpdateMissionList()
	{
		GameObject tempBox;
		
		for (int i = 0; i < missionBox.transform.childCount; i++)
		{
			Destroy(missionBox.transform.GetChild(i).gameObject);
		}
		
		for (int i = 0; i  < availableMissions.Count; i++)
		{
			tempBox = Instantiate(missionPrefab, missionBox.transform);
			tempBox.GetComponent<MissionSlot>().AddMission(availableMissions[i]);
		}
		
	}
	
	public void UpdateRecruitmentList()
	{
		Debug.Log("Updating recruitment list");
		GameObject tempBox;
		List<Crew> availableCrew = station.GetAvailableCrew();
		
		for (int i = 0; i < recruitBox.transform.childCount; i ++)
		{
			Destroy(recruitBox.transform.GetChild(i).gameObject);
		}
		
		for (int i = 0; i < availableCrew.Count; i++)
		{
			tempBox = Instantiate(crewPrefab, recruitBox.transform);
			tempBox.GetComponent<CrewSlot>().AddCrew(availableCrew[i]);
			if (GameControl.instance.playerMoney < availableCrew[i].GetPrice())
			{
				tempBox.GetComponentInChildren<Button>().interactable = false;
			}
		}
		
	}
}
