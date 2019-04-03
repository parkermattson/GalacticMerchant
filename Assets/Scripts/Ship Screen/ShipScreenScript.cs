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
	private Transform equipSlotBox;
	
	[SerializeField]
	private GameObject equipList;
	
	[SerializeField]
	private Transform shipSlotPrefab;
	
	Inventory inventory;
	
	[SerializeField]
	private GameObject[] crewBoxes = new GameObject[4];
	
	GameControl gcScript;
	EquipSlot[] equipSlots;
	
	
	// Use this for initialization
	void Awake () {
		inventory = Inventory.instance;
		inventory.onEquipmentChangedCallback += EquipmentChangeUpdate;
		
		gcScript = gameController.GetComponent<GameControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnEnable()
	{
		EquipmentChangeUpdate();
	}
	
	void DisplayEquipmentList()
	{
		GameObject tempSlot;
		
		equipSlots = equipSlotBox.GetComponentsInChildren<EquipSlot>();
		for (int i = 0; i < equipSlots.Length; i++)
		{
			Destroy(equipSlotBox.GetChild(i).gameObject);
		}
		
		for (int j = 0; j < 4; j++)
		{
			for (int i = 0; i < inventory.equipments.Count; i ++)
			{
				if ((int)inventory.equipments[i].equipSlot == j)
				{
					tempSlot = Instantiate(shipSlotPrefab, equipSlotBox).gameObject;
					tempSlot.GetComponent<EquipSlot>().AddEquipment(inventory.equipments[i]);
					tempSlot.GetComponentInChildren<SlotDragScript>().newParent = equipList;
				}
			}
		}
	}
	
	void EquipmentChangeUpdate()
	{
		DisplayEquipmentList();
		SetHullBar();
		SetFuelBar();
		SetCargoSpace();
		SetFuelEfficiency();
		SetSensorRange();
		SetCrewBoxes();
	}
	
	void SetHullBar()
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
	
	void SetFuelBar()
	{
		float fuelPercent = (float)gcScript.shipState.currentFuel / (float)gcScript.shipState.playerShip.maxFuel;
		fuelBar.transform.localScale = new Vector3(fuelPercent, 1, 1);
	}
	
	void SetCargoSpace()
	{
		cargoText.SetText("Cargo Space: {0} / {1}", gcScript.shipState.currentCargo, gcScript.shipState.playerShip.maxCargo);
	}
	
	void SetFuelEfficiency()
	{
		fuelText.SetText("Fuel Efficiency: {0} CTU/FC", gcScript.shipState.netFuelEff);
	}
	
	void SetSensorRange()
	{
		sensorText.SetText("Sensor Range: {0} CTUs", gcScript.shipState.netSensorRange);
	}
	
	void SetCrewBoxes()
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
