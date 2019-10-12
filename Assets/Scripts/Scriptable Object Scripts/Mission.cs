using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType {Combat, Courier, Mining, Express}

public abstract class Mission : ScriptableObject {

	public string missionName = "Mission Name";
	public MissionType missionType = MissionType.Combat;
	public Station source;
	public bool completed = false;
	public int rewardMoney = 0;
	public List<ItemStack> rewardItems = new List<ItemStack>();
	public List<Equipment> rewardEquips = new List<Equipment>();
	
	public string GetName() {
	return missionName; }
	
	public abstract string GetDesc();
	
	public MissionType GetMissionType() {
	return missionType; }
	
	public int GetRewardMoney() {
	return rewardMoney; }
	
	public List<ItemStack> GetRewardItems() {
	return rewardItems; }
	
	public List<Equipment> GetRewardEquips() {
	return rewardEquips; }
	

	
}
