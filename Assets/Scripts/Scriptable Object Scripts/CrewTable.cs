using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crew Table", menuName = "Crew Table")]
public class CrewTable : ScriptableObject {

	public List<Crew> crewList;
	
	public List<Crew> GetCrewList() {
		return crewList;
	}
	
}
