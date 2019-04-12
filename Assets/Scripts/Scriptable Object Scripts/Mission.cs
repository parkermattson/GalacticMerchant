using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType {Combat, Courier, Mining, Express}

[CreateAssetMenu(fileName = "New Mission", menuName = "Mission")]
public class Mission : ScriptableObject {

	public string missionName = "Mission Name";
	public string missionDesc = "Mission description";
	public MissionType missionType = MissionType.Combat;
	public int rewardMoney = 0;
	public List<Item> rewardItems;
	public List<Equipment> rewardEquips;
	
	public string GetName() {
	return missionName; }
	
	public string GetDesc() {
	return missionDesc; }
	
	public MissionType GetMissionType() {
	return missionType; }
	
	public int GetRewardMoney() {
	return rewardMoney; }
	
	public List<Item> GetRewardItems() {
	return rewardItems; }
	
	public List<Equipment> GetRewardEquips() {
	return rewardEquips; }
	

	
}
