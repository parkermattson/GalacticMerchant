using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewScreenScript : MonoBehaviour {

	public GameObject gameController;
	public GameObject[] crewBox = new GameObject[4];
	public Sprite[] avatars = new Sprite[3];
	public Sprite[] races = new Sprite[3];

	void OnEnable()
	{
		GameControl gameScript = gameController.GetComponent<GameControl>();
		for (int i = 0; i < 4; i++)
		{
			if (gameScript.crewMembs[i] != null)
			{
				crewBox[i].SetActive(true);
				crewBox[i].transform.Find("Portrait Image").GetComponent<Image>().sprite = avatars[gameScript.crewMembs[i].avatar];
				crewBox[i].transform.Find("Race Image").GetComponent<Image>().sprite = races[gameScript.crewMembs[i].race];
				crewBox[i].transform.Find("Name Text 2").GetComponent<TextMeshProUGUI>().SetText(gameScript.crewMembs[i].name);
				crewBox[i].transform.Find("Stat 1 Text").GetComponent<TextMeshProUGUI>().SetText("{0}", gameScript.crewMembs[i].stats[0]);
				crewBox[i].transform.Find("Stat 2 Text").GetComponent<TextMeshProUGUI>().SetText("{0}", gameScript.crewMembs[i].stats[1]);
				crewBox[i].transform.Find("Stat 3 Text").GetComponent<TextMeshProUGUI>().SetText("{0}", gameScript.crewMembs[i].stats[2]);
				crewBox[i].transform.Find("Stat 4 Text").GetComponent<TextMeshProUGUI>().SetText("{0}", gameScript.crewMembs[i].stats[3]);
			}
			else 
				crewBox[i].SetActive(false);
		}
	}
}
