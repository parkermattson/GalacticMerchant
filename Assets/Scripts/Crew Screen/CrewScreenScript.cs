﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewScreenScript : MonoBehaviour {

	public GameObject[] crewSlots;

	void OnEnable()
	{
		Crew[] crew = GameControl.instance.crewMembs;
		for (int i = 0; i < 4; i++)
		{
			if (crew[i] != null)
			{
				crewSlots[i].GetComponent<CrewSlot>().AddCrew(crew[i]);
				crewSlots[i].SetActive(true);
			}
			else crewSlots[i].SetActive(false);
		}
	}
}
