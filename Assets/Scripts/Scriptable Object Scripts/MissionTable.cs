using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New MissionTable", menuName = "MissionTable")]
public class MissionTable : ScriptableObject {

	public List<Mission> missionList;
	public List<float> missionChance;
	
}
