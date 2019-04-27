using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewScreenScript : MonoBehaviour {

	public GameObject gameController;
	public GameObject[] crewSlots = new GameObject[4];

	void OnEnable()
	{
		Crew[] crew = gameController.GetComponent<GameControl>().crewMembs;
		for (int i = 0; i < 4; i++)
		{
			if (crew[i] != null)
			{
				crewSlots[i].GetComponent<CrewSlot>().AddCrew(crew[i]);
			}
			else crewSlots[i].SetActive(false);
		}
	}
}
