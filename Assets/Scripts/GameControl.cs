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
	
	Inventory inventory;
	
	//Game variables
	public string playerName;
	public int playerMoney, playerRace, playerAvatar;
	public int[] activeStats = {1,1,1,1};
	public Crew[] crewMembs = new Crew[4], assignedCrew = new Crew[4];
    public Ship playerShip, defaultShip;
    public Location playerLocation;
	public DateTime gameTime, lastWeek;
	public List<Station> initStations, stations;
	public List<Mission> acceptedMissions = new List<Mission>();
	
	public Sprite[] avatars = new Sprite[3];
	public Sprite[] races = new Sprite[3];
	
	

	// Use this for initialization
	void Awake () {
		
		instance = this;
		
		inventory = Inventory.instance;
		
		crewMembs[0] = Crew.CreateInstance<Crew>();
		crewMembs[0].crewName = "Player";
		
		assignedCrew[0] = crewMembs[0];
		
		for (int i = 1; i < 4; i++)
		{
			crewMembs[i] = null;
			assignedCrew[i] = crewMembs[0];
		}
		
		gameTime = new DateTime(3000, 1, 1, 9, 0, 0);
		lastWeek = new DateTime(3000, 1, 1, 9, 0, 0);
		
		foreach (Station s in initStations)
		{
			stations.Add(Instantiate(s));
			stations.Last().Init();
		}
		
		playerShip = Instantiate(defaultShip);
		
		playerLocation = stations[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void NewChar(CharCreate ccScript)
	{
		playerName = ccScript.GetPlayerName();
		playerRace = ccScript.GetRace();
		playerAvatar = ccScript.GetAvatar();
		playerMoney = 100;
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