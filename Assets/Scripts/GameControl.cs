using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameControl : MonoBehaviour {
	
	public static GameControl instance = null;
	
	Inventory inventory = Inventory.instance;
	
	//Game variables
	public string playerName;
	public int playerMoney, playerRace, playerAvatar;
	public int[] activeStats = {1,1,1,1};
	public Crew[] crewMembs = new Crew[4], assignedCrew = new Crew[4];
    public Ship playerShip, defaultShip;
    public Location playerLocation;
	public DateTime gameTime = new DateTime(3000, 1, 1, 9, 0, 0), lastHour = new DateTime(3000, 1, 1, 9, 0, 0), lastWeek = new DateTime(3000, 1, 1, 9, 0, 0);
	public List<Station> initStations, stations;
	public List<Mission> acceptedMissions = new List<Mission>();
	public List<Location> initLocations, locations;
	
	public Sprite[] avatars = new Sprite[3];
	public Sprite[] races = new Sprite[3];
	
	

	// Use this for initialization
	void Awake () {
		
		instance = this;
		
		crewMembs[0] = Crew.CreateInstance<Crew>();
		crewMembs[0].crewName = "Player";
		
		for (int i = 0; i < 4; i++)
		{
			assignedCrew[i] = crewMembs[0];
		}
		
		foreach (Station s in initStations)
		{
			stations.Add(Instantiate(s));
			stations.Last().Init();
		}
		
		foreach (Location l in initLocations)
		{
			locations.Add(Instantiate(l));
			locations.Last().RefreshType();
		}
		
		playerShip = Instantiate(defaultShip);
		
		playerLocation = stations[0];
	}
	
	public void NewChar(CharCreate ccScript)
	{
		playerName = ccScript.GetPlayerName();
		playerRace = ccScript.GetRace();
		playerAvatar = ccScript.GetAvatar();
		playerMoney = 1000;
		crewMembs[0] = ScriptableObject.CreateInstance<Crew>();
		crewMembs[0].SetCrewName(playerName);
		crewMembs[0].SetRace(playerRace);
		crewMembs[0].SetAvatar(playerAvatar);
		crewMembs[0].SetStats(ccScript.GetStats());
		
		for (int i = 0; i < 4; i++)
		{
			assignedCrew[i] = crewMembs[0];
			activeStats[i] = assignedCrew[i].GetStat(i);
		}
	}
	
	public void Save(String saveName)
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/" + saveName + ".dat");
		
		GameData saveData = new GameData();
		
		//Assign local data to savedata here
		saveData.saveName = saveName;
		saveData.playerMoney = playerMoney;
		saveData.playerName = playerName;
		saveData.activeStats = activeStats;
		saveData.playerAvatar = playerAvatar;
		saveData.playerRace = playerRace;
		saveData.crewMembs = crewMembs;
		saveData.assignedCrew = assignedCrew;
		saveData.playerShip = playerShip;
		saveData.playerLocation = playerLocation;
		saveData.items = inventory.items;
		saveData.equipments = inventory.equipments;
		saveData.shipEquipment = inventory.shipEquipment;
		saveData.stations = stations;
		
		bf.Serialize(file, saveData);
		file.Close();
		
	}
	
	public void Load(String saveName)
	{
		if (File.Exists(Application.persistentDataPath + "/" + saveName + ".dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + saveName + ".dat", FileMode.Open);
			GameData loadData = (GameData)bf.Deserialize(file);
			file.Close();
			
			//Assign loaded data to local variables here
				playerName = loadData.playerName;
				playerMoney = loadData.playerMoney;
				activeStats = loadData.activeStats;
				playerAvatar = loadData.playerAvatar;
				playerRace = loadData.playerRace;
				crewMembs = loadData.crewMembs;
				assignedCrew = loadData.assignedCrew;
				playerShip = loadData.playerShip;
				playerLocation = loadData.playerLocation;
				inventory.items = loadData.items;
				inventory.equipments = loadData.equipments;
				inventory.shipEquipment = loadData.shipEquipment;
				stations = loadData.stations;
			
		}
	}
	
	public void PassTime(float hours)
	{
		gameTime = gameTime.AddHours(hours);
		
		if (gameTime > lastHour.AddHours(1))
		{
			foreach (Location l in locations)
			{
				l.RefreshType();
			}
			
			lastHour = gameTime;
		}
		
		while (gameTime > lastWeek.AddDays(7))
		{
			for (int i = 1; i < 4; i++)
			{
				if (crewMembs[i] != null)
					playerMoney-= crewMembs[i].GetPrice();
			}
			lastWeek = lastWeek.AddDays(7);
		}
	}
	
	public void HireCrew(Crew crew)
	{
		for (int i = 0; i < crewMembs.Length; i ++)
		{
			if (crewMembs[i] == null)
			{
				crewMembs[i] = crew;
				break;
			}
			if (i == crewMembs.Length - 1)
			{
				Debug.Log("Crew couldnt be hired");
			}
		}
	}
	
	public float GetQuestMoneyBonus() {
		if (activeStats[0] > 0)
			return 0.1f;
		else return 0;
	}
	
	public float GetMarketPriceBonus() {
		if (activeStats[0] > 1)
			return -.02f * activeStats[0];
		else return 0;
	}
	
	public float GetCrewDiscountBonus() {
		if (activeStats[0] > 2)
			return .85f;
		else return 1;
	}
	
	public int GetActiveStatBonus() {
		if (activeStats[0] > 3)
			return 1;
		else return 0;
	}
	
	public float GetWeaponCooldownBonus() {
		float bonus = 0;
		if (activeStats[1] + GetActiveStatBonus() > 0) {
			bonus+= .1f;
			if (activeStats[1] + GetActiveStatBonus() > 4)
				 bonus += .1f;
		}
		return bonus;
	}
	
	public float GetWeaponChargeBonus() {
		float bonus = 0;
		if (activeStats[1] + GetActiveStatBonus() > 1) {
			bonus+= .04f * (activeStats[1] + GetActiveStatBonus());
			if (activeStats[1] + GetActiveStatBonus() > 4)
				 bonus += .1f;
		}
		return bonus;
	}
	
	public float GetDefenseDurationBonus() {
		float bonus = 0;
		if (activeStats[1] + GetActiveStatBonus() > 2) {
			bonus+= .2f;
			if (activeStats[1] + GetActiveStatBonus() > 4)
				 bonus += .1f;
		}
		return bonus;
	}
	
	public bool GetCombatDodgeBonus() {
		if (activeStats[1] + GetActiveStatBonus() > 3)
			return true;
		else return false;
	}
	
	public float GetDefenseCooldownBonus() {
		if (activeStats[1] + GetActiveStatBonus() > 4)
			return 0.1f;
		else return 0;
	}
	
	public float GetWarpRangeBonus() {
		float bonus = 0;
		if (activeStats[2] + GetActiveStatBonus() > 0)
			bonus+= .1f;
		
		if (activeStats[3] + GetActiveStatBonus() > 2) {
			bonus+= .25f;
			if (activeStats[3] + GetActiveStatBonus() > 4)
				 bonus += .1f;
		}
		return bonus;
	}
	
	public float GetSensorRangeBonus() {
		float bonus = 0;
		if (activeStats[2] + GetActiveStatBonus() > 1) {
			bonus+= .1f * (activeStats[2] + GetActiveStatBonus());
			if (activeStats[2] + GetActiveStatBonus() > 4)
				 bonus += .5f;
		}
		return bonus;
	}
	
	public int GetSensorLevelBonus() {
		if (activeStats[2] + GetActiveStatBonus() > 2)
			return 1;
		else return 0;
	}
	
	public float GetEncounterSuccessBonus() {
		if (activeStats[2] + GetActiveStatBonus() > 3)
			return .25f;
		else return 0;
	}
	
	public float GetFuelEfficiencyBonus() {
		float bonus = 0;
		if (activeStats[3] + GetActiveStatBonus() > 0) {
			bonus+= .15f;
			if (activeStats[3] + GetActiveStatBonus() > 4)
				 bonus += .1f;
		}
		return bonus;
	}
	
	public float GetWarpSpeedBonus() {
		float bonus = 0;
		if (activeStats[3] + GetActiveStatBonus() > 1) {
			bonus+= .15f * (activeStats[3] + GetActiveStatBonus());
			if (activeStats[3] + GetActiveStatBonus() > 4)
				 bonus += .1f;
		}
		return bonus;
	}
	
	public float GetFactorySpeedBonus() {
		if (activeStats[3] + GetActiveStatBonus() > 3)
			return 0.33f;
		else return 0;
	}
	
	[Serializable]
	class GameData
	{
		public string saveName, playerName;
		public int playerMoney, playerRace, playerAvatar;
		public int[] activeStats;
		public Crew[] crewMembs, assignedCrew;
		public Ship playerShip;
		public Location playerLocation;
		public List<ItemStack> items;
		public List<Equipment> equipments;
		public Equipment[] shipEquipment;
		public List<Station> stations;
	}
}