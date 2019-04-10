using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcademyScreenScript : MonoBehaviour {
	
	public GameObject missionBox;
	public GameObject missionPrefab;
	
	public List<MissionTable> potentialMissions;
	List<Mission> availableMissions;
	

	void UpdateMissionList()
	{
		GameObject tempBox;
		
		for (int i = 0; i < missionBox.transform.childCount; i++)
		{
			Destroy(missionBox.transform.GetChild(i).gameObject);
		}
		
		for (int i = 0; i  < 5; i++)
		{
			tempBox = Instantiate(missionPrefab, missionBox.transform);
			//Make mission box script and use here
		}
		
	}
}
