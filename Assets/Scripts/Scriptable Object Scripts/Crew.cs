using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crew", menuName = "Crew")]
public class Crew : ScriptableObject {

	public string crewName = "";
	public int race = 0;
	public int avatar = 0;
	public int[] stats = {1,1,1,1};
	
	public void SetCrewName(string newName){
		crewName = newName;
	}
	
	public void SetRace(int newRace) {
		race = newRace;
	}
	
	public void SetAvatar(int newAvatar) {
		avatar = newAvatar;
	}
	
	public void SetStats(int[] newStats)
	{
		stats = newStats;
	}
	
	public void SetStat(int statNum, int newStat) {
		stats[statNum] = newStat;
	}
	
	public string GetCrewName(){
		return crewName;
	}
	
	public int GetRace() {
		return race;
	}
	
	public int GetAvatar() {
		return avatar;
	}
	
	public int[] GetStats() {
		return stats;
	}
	
	public int GetStat(int statNum) {
		return stats[statNum];
	}
}
