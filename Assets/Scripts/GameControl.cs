using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour {
	
	//Game variables
	
	public string playerName;
	public int money, race, avatar;
	public int[] stats = {1,1,1,1};
	public CrewMember[] crewMembs = new CrewMember[4];
	
	

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
		
		for (int i = 0; i < 4; i++)
		{
			crewMembs[i] = new CrewMember();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void NewChar(GameObject ccScreen)
	{
		CharCreate ccScript = ccScreen.GetComponent<CharCreate>();
		playerName = ccScript.GetPlayerName();
		race = ccScript.GetRace();
		avatar = ccScript.GetAvatar();
		stats = ccScript.GetStats();
		crewMembs[0].enabled = true;
		crewMembs[0].name = playerName;
		crewMembs[0].race = race;
		crewMembs[0].avatar = avatar;
		crewMembs[0].stats = stats;
	}
	
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		
		GameData saveData = new GameData();
		//Assign local data to savedata here
		saveData.money = money;
		saveData.playerName = playerName;
		saveData.stats = stats;
		saveData.avatar = avatar;
		saveData.race = race;
		
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
				money = loadData.money;
				stats = loadData.stats;
				avatar = loadData.avatar;
				race = loadData.race;
			
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
	
	[Serializable]
	class GameData
	{
		public string playerName;
		public int money, race, avatar;
		public int[] stats;
	}
}