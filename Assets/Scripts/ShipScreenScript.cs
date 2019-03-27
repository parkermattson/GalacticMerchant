using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipScreenScript : MonoBehaviour {
	
	[SerializeField]
	private GameObject gameController;
	
	[SerializeField]
	private GameObject hullBar;
	
	[SerializeField]
	private GameObject fuelBar;
	
	[SerializeField]
	private TextMeshProUGUI cargoText;
	
	[SerializeField]
	private TextMeshProUGUI fuelText;
	
	[SerializeField]
	private TextMeshProUGUI sensorText;
	
	[SerializeField]
	private GameObject[] crewBoxes = new GameObject[4];
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnEnable()
	{
		GameControl gcScript = gameController.GetComponent<GameControl>();
		SetHullBar(gcScript);
		SetFuelBar(gcScript);
		SetCargoSpace(gcScript);
		SetFuelEfficiency(gcScript);
		SetSensorRange(gcScript);
		SetCrewBoxes(gcScript);
	}
	
	void SetHullBar(GameControl gcScript)
	{
		float hullPercent = (float)gcScript.shipState.currentHull / (float)gcScript.shipState.playerShip.maxHull;
		hullBar.transform.localScale = new Vector3(hullPercent, 1, 1);

		if (hullPercent > .66)
			hullBar.GetComponent<Image>().color = Color.green;
		else if (hullPercent > .33)
			hullBar.GetComponent<Image>().color = Color.yellow;
		else 
			hullBar.GetComponent<Image>().color = Color.red;
		
	}
	
	void SetFuelBar(GameControl gcScript)
	{
		float fuelPercent = (float)gcScript.shipState.currentFuel / (float)gcScript.shipState.playerShip.maxFuel;
		fuelBar.transform.localScale = new Vector3(fuelPercent, 1, 1);
	}
	
	void SetCargoSpace(GameControl gcScript)
	{
		cargoText.SetText("Cargo Space: {0} / {1}", gcScript.shipState.currentCargo, gcScript.shipState.playerShip.maxCargo);
	}
	
	void SetFuelEfficiency(GameControl gcScript)
	{
		fuelText.SetText("Fuel Efficiency: {0} CTU/FC", gcScript.shipState.netFuelEff);
	}
	
	void SetSensorRange(GameControl gcScript)
	{
		sensorText.SetText("Sensor Range: {0} CTUs", gcScript.shipState.netSensorRange);
	}
	
	void SetCrewBoxes(GameControl gcScript)
	{
		for (int i = 0; i < 4; i++)
		{
			if (gcScript.crewMembs[i].enabled)
			{
				crewBoxes[i].SetActive(true);
				crewBoxes[i].transform.Find("NameText").GetComponent<TextMeshProUGUI>().SetText(gcScript.crewMembs[i].name);
				crewBoxes[i].transform.Find("SocialText").GetComponent<TextMeshProUGUI>().SetText("{0}", gcScript.crewMembs[i].stats[0]);
				crewBoxes[i].transform.Find("EngText").GetComponent<TextMeshProUGUI>().SetText("{0}", gcScript.crewMembs[i].stats[1]);
				crewBoxes[i].transform.Find("CombatText").GetComponent<TextMeshProUGUI>().SetText("{0}", gcScript.crewMembs[i].stats[2]);
				crewBoxes[i].transform.Find("SciText").GetComponent<TextMeshProUGUI>().SetText("{0}", gcScript.crewMembs[i].stats[3]);
			}
			else 
				crewBoxes[i].SetActive(false);
		}
	}
}
