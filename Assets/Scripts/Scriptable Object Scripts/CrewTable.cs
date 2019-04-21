using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewTable : ScriptableObject {

	public List<Crew> crewList;
	
	public List<Crew> GetCrewList() {
		return crewList;
	}
	
}
