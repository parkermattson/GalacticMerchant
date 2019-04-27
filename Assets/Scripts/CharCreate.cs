using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharCreate : MonoBehaviour {
	
	public int[] stats = {1, 1, 1, 1};
	public TextMeshProUGUI[] statText = new TextMeshProUGUI[4];
	
	public TextMeshProUGUI pointText;
	int points = 3;
	
	public Image avatarImage;
	int currentAvatar = 0;
	
	string playerName = "";
	
	int race = 0;
	public TextMeshProUGUI raceText;
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
		statText[statNum].SetText("{0}", stats[statNum]);
		pointText.SetText("Available Points: {0}", points);
	}
	
	public void StatTextDown(int statNum)
	{
		if (stats[statNum] > 1)
		{
			stats[statNum]--;
			points++;
		}
		statText[statNum].SetText("{0}", stats[statNum]);
		pointText.SetText("Available Points: {0}", points);
	}
	
	public void AvatarRight()
	{
		if (currentAvatar < GameControl.instance.avatars.Length -1)
		{
			currentAvatar++;
			avatarImage.sprite = GameControl.instance.avatars[currentAvatar];
		}
	}
	
	public void AvatarLeft()
	{
		if (currentAvatar > 0)
		{
			currentAvatar--;
			avatarImage.sprite = GameControl.instance.avatars[currentAvatar];
		}
	}
	
	public void SetPlayerName(TextMeshProUGUI nameInputText)
	{
		playerName = nameInputText.text;
	}
	
	public void SetRace(int newRace)
	{
		race = newRace;
		raceText.SetText(raceDesc[race]);
	}
	
}
