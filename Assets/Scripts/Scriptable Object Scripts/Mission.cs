using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType {Bounty, Courier, Mining, Procurement, Research}

public abstract class Mission : ScriptableObject {

	public string missionName = "Mission Name", missionDesc;
	public MissionType missionType = MissionType.Bounty;
	public Station source;
	public bool completed = false;
	public int rewardMoney = 0;
	public List<ItemStack> rewardItems = new List<ItemStack>();
	public List<Equipment> rewardEquips = new List<Equipment>();
	public DateTime expireDate = new DateTime(3000, 1, 1, 9, 0, 0);
	
	public string GetName() {
	return missionName; }
	
	public string GetDesc() {
		return missionDesc;
	}
	
	public MissionType GetMissionType() {
	return missionType; }
	
	public int GetRewardMoney() {
	return rewardMoney; }
	
	public List<ItemStack> GetRewardItems() {
	return rewardItems; }
	
	public List<Equipment> GetRewardEquips() {
	return rewardEquips; }
	

	
}
