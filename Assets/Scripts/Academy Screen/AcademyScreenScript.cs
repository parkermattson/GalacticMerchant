using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcademyScreenScript : MonoBehaviour {
	
	public GameObject missionBox;
	public GameObject missionPrefab;
	
	public MissionTable potentialMissions;
	public List<Mission> availableMissions;
	
	void OnEnable()
	{
		UpdateMissionList();
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
}
