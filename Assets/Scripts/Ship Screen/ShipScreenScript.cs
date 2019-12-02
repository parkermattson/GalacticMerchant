using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipScreenScript : MonoBehaviour {
	
	[SerializeField]
	private GameObject gameController, equipList, hullBar, fuelBar, shipSlotPrefab;
	
	[SerializeField]
	private TextMeshProUGUI cargoText, fuelText, warpSpeedText, warpRangeText, sensorLevelText, sensorRangeText;
	
	[SerializeField]
	private Transform equipSlotBox;
	
	[SerializeField]
	private ShipSlot[] shipSlots;
	
	EquipSlot[] equipSlots;
	
	
	// Use this for initialization
	void Awake () {
		Inventory.instance.onEquipmentChangedCallback += EquipmentChangeUpdate;
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
			for (int i = 0; i < Inventory.instance.equipments.Count; i ++)
			{
				if ((int)Inventory.instance.equipments[i].equipSlot == j)
				{
					tempSlot = Instantiate(shipSlotPrefab, equipSlotBox);
					tempSlot.GetComponent<EquipSlot>().AddEquipment(Inventory.instance.equipments[i]);
					tempSlot.GetComponentInChildren<SlotDragScript>().newParent = equipList;
				}
			}
		}
	}
	
	void EquipmentChangeUpdate()
	{
		SetShipSlots();
		DisplayEquipmentList();
		SetShipStatText();
	}
	
	void SetShipSlots() {
		shipSlots[0].AddShipEquipment(GameControl.instance.playerShip.command);
		shipSlots[1].AddShipEquipment((Equipment)GameControl.instance.playerShip.weapon);
		shipSlots[2].AddShipEquipment(GameControl.instance.playerShip.sensor);
		shipSlots[3].AddShipEquipment(GameControl.instance.playerShip.engine);
	}
	
	void SetShipStatText()
	{
		float hullPercent = (float)GameControl.instance.playerShip.currentHull / (float)GameControl.instance.playerShip.maxHull;
		hullBar.transform.localScale = new Vector3(hullPercent, 1, 1);

		if (hullPercent > .66)
			hullBar.GetComponent<Image>().color = Color.green;
		else if (hullPercent > .33)
			hullBar.GetComponent<Image>().color = Color.yellow;
		else 
			hullBar.GetComponent<Image>().color = Color.red;
		float fuelPercent = (float)GameControl.instance.playerShip.currentFuel / (float)GameControl.instance.playerShip.maxFuel;
		fuelBar.transform.localScale = new Vector3(fuelPercent, 1, 1);
		cargoText.SetText("Cargo Space: {0} / {1}", Inventory.instance.currentCargo, GameControl.instance.playerShip.maxCargo);
		fuelText.SetText("Fuel Efficiency: {0} CTU/FC", GameControl.instance.playerShip.GetNetFuelEff());
		warpRangeText.SetText("Warp Range: {0} CTUs", GameControl.instance.playerShip.GetNetWarpRange());
		warpSpeedText.SetText("Warp Speed: {0} CTU/h", GameControl.instance.playerShip.GetNetSpeed());
		sensorRangeText.SetText("Sensor Range: {0} CTUs", GameControl.instance.playerShip.GetNetSensorRange());
		sensorLevelText.SetText("Sensor Level: {0}", GameControl.instance.playerShip.GetSensorLevel());
	}
}
