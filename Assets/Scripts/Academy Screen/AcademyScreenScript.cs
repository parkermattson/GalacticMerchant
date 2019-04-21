﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcademyScreenScript : MonoBehaviour {
	
	public GameObject missionBox;
	public GameObject missionPrefab;
	public MissionTable allMissions;
	public List<Mission> availableMissions;
	
	public GameObject recruitBox;
	public GameObject crewPrefab;
	public CrewTable allCrew;
	public List<Crew> availableCrew;
	
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
	
	void UpdateRecruitmentList()
	{
		GameObject tempBox;
		
		for (int i = 0; i < recruitBox.transform.childCount; i ++)
		{
			Destroy(recruitBox.transform.GetChild(i).gameObject);
		}
		
		for (int i = 0; i < availableCrew.Count; i++)
		{
			tempBox = Instantiate(crewPrefab, recruitBox.transform);
			//tempBox.GetComponent<CrewSlot>().AddCrew(availableCrew[i]); Make crew slot
		}
		
	}
}
