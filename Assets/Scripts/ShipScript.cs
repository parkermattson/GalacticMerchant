using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipScript : MonoBehaviour {
	
	[SerializeField]
	private GameObject gameController;
	
	[SerializeField]
	private GameObject hullBar;
	
	[SerializeField]
	private GameObject fuelBar;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnEnable()
	{
		SetHullBar();
		SetFuelBar();
	}
	
	void SetHullBar()
	{
		GameControl gcScript = gameController.GetComponent<GameControl>();
		float hullPercent = (float)gcScript.ship.currentHull / (float)gcScript.ship.maxHull;
		hullBar.transform.localScale = new Vector3(hullPercent, 1, 1);

		if (hullPercent > .66)
			hullBar.GetComponent<Image>().color = Color.green;
		else if (hullPercent > .33)
			hullBar.GetComponent<Image>().color = Color.yellow;
		else 
			hullBar.GetComponent<Image>().color = Color.red;
		
	}
	
	void SetFuelBar()
	{
		GameControl gcScript = gameController.GetComponent<GameControl>();
		float fuelPercent = (float)gcScript.ship.currentFuel / (float)gcScript.ship.maxFuel;
		fuelBar.transform.localScale = new Vector3(fuelPercent, 1, 1);
	}
}
