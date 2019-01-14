using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharCreate : MonoBehaviour {
	
	public int[] stats = {1, 1, 1, 1};
	public GameObject[] statText = new GameObject[4];
	
	public GameObject pointText;
	int points = 3;
	
	public GameObject avatarImage;
	public Sprite[] avatars;
	int currentAvatar = 0;
	
	string playerName = "";
	
	int race = 0;
	string[] raceDesc = {"Human description", "Dwarf description", "Reptile description"};
	
	//Getters
	
	public string GetPlayerName()
	{
		return playerName;
	}
	
	public int[] GetStats()
	{
		return stats;
	}
	
	public int GetRace()
	{
		return race;
	}
	
	public int GetAvatar()
	{
		return currentAvatar;
	}
	
	//Setters

	public void StatTextUp(int statNum)
	{
		if (stats[statNum] < 5 && points > 0)
		{
			stats[statNum]++;
			points --;
		}
		statText[statNum].GetComponent<TextMeshProUGUI>().SetText("{0}", stats[statNum]);
		pointText.GetComponent<TextMeshProUGUI>().SetText("Available Points: {0}", points);
	}
	
	public void StatTextDown(int statNum)
	{
		if (stats[statNum] > 1)
		{
			stats[statNum]--;
			points++;
		}
		statText[statNum].GetComponent<TextMeshProUGUI>().SetText("{0}", stats[statNum]);
		pointText.GetComponent<TextMeshProUGUI>().SetText("Available Points: {0}", points);
	}
	
	public void AvatarRight()
	{
		if (currentAvatar < avatars.Length -1)
		{
			currentAvatar++;
			avatarImage.GetComponent<Image>().sprite = avatars[currentAvatar];
		}
	}
	
	public void AvatarLeft()
	{
		if (currentAvatar > 0)
		{
			currentAvatar--;
			avatarImage.GetComponent<Image>().sprite = avatars[currentAvatar];
		}
	}
	
	public void SetPlayerName(GameObject nameInputText)
	{
		playerName = nameInputText.GetComponent<TextMeshProUGUI>().text;
	}
	
	public void SetRace(int newRace)
	{
		race = newRace;
		GameObject raceText = GameObject.Find("Race Text");
		raceText.GetComponent<TextMeshProUGUI>().SetText(raceDesc[race]);
	}
	
}
