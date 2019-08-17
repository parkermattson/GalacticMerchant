using System;
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
	public int[] playerStats = {1,1,1,1};
	public Crew[] crewMembs = new Crew[4];
    public ShipState shipState = new ShipState();
	public Ship defaultShip;
    public Location playerLocation;
	public DateTime gameTime;
	
	public Sprite[] avatars = new Sprite[3];
	public Sprite[] races = new Sprite[3];
	
	

	// Use this for initialization
	void Awake () {
		
		instance = this;
		
		inventory = Inventory.instance;
		
		for (int i = 0; i < 4; i++)
		{
			crewMembs[i] = null;
		}
		
		shipState.playerShip = defaultShip;
		
		gameTime = new DateTime(3000, 1, 1, 9, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void NewChar(GameObject ccScreen)
	{
		CharCreate ccScript = ccScreen.GetComponent<CharCreate>();
		playerName = ccScript.GetPlayerName();
		Debug.Log(playerName);
		playerRace = ccScript.GetRace();
		playerAvatar = ccScript.GetAvatar();
		playerStats = ccScript.GetStats();
		playerMoney = 100;
		crewMembs[0] = ScriptableObject.CreateInstance<Crew>();
		crewMembs[0].SetCrewName(playerName);
		crewMembs[0].SetRace(playerRace);
		crewMembs[0].SetAvatar(playerAvatar);
		crewMembs[0].SetStats(playerStats);
	}
	
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		
		GameData saveData = new GameData();
		
		//Assign local data to savedata here
		saveData.playerMoney = playerMoney;
		saveData.playerName = playerName;
		saveData.playerStats = playerStats;
		saveData.playerAvatar = playerAvatar;
		saveData.playerRace = playerRace;
		saveData.crewMembs = crewMembs;
		saveData.shipState = shipState;
		saveData.playerLocation = playerLocation;
		saveData.items = inventory.items;
		saveData.equipments = inventory.equipments;
		saveData.shipEquipment = inventory.shipEquipment;
		
		bf.Serialize(file, saveData);
		file.Close();
		
	}
	
	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			GameData loadData = (GameData)bf.Deserialize(file);
			file.Close();
			
			//Assign loaded data to local variables here
				playerName = loadData.playerName;
				playerMoney = loadData.playerMoney;
				playerStats = loadData.playerStats;
				playerAvatar = loadData.playerAvatar;
				playerRace = loadData.playerRace;
				crewMembs = loadData.crewMembs;
				shipState = loadData.shipState;
				playerLocation = loadData.playerLocation;
				inventory.items = loadData.items;
				inventory.equipments = loadData.equipments;
				inventory.shipEquipment = loadData.shipEquipment;
			
		}
	}
	
	public class CrewMember
	{
		public string name = "";
		public int race = 0;
		public int avatar = 0;
		public int[] stats = {1,1,1,1};
		public bool enabled = false;
	}
	
	public class ShipState
	{
		public Ship playerShip;
		public int currentHull = 10;
		public int currentFuel  = 100;
		public int currentCargo = 0;
		public float netFuelEff = 1;
		public int netSensorRange = 20;
		public int netWarpRange = 350;
		public int netSpeed = 1;
	}
	
	[Serializable]
	class GameData
	{
		public string playerName;
		public int playerMoney, playerRace, playerAvatar;
		public int[] playerStats;
		public Crew[] crewMembs;
		public ShipState shipState;
		public Location playerLocation;
		public List<ItemStack> items;
		public List<Equipment> equipments;
		public Equipment[] shipEquipment;
	}
}