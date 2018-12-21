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
	
	

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void charCreate(GameObject nameText)
	{
		playerName = nameText.GetComponent<TextMeshPro>().text;
	}
	
	public void statUp(int stat)
	{
		if (stats[stat] < 5)
			stats[stat] ++;
	}
	
	public void statDown(int stat)
	{
		if (stats[stat] > 1)
			stats[stat] --;
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
	
	[Serializable]
	class GameData
	{
		public string playerName;
		public int money, race, avatar;
		public int[] stats;
		}
	}