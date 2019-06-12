using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class MechanicScreenScript : MonoBehaviour {

	const int HULLCAP = 100;
	const int FUELCAP = 1000;
	const int SIGNALCAP = 100;
	
	Inventory inventory;
	
	public GameObject currentScreen;
	
	public Image shipGraphic;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI descText;
	public TextMeshProUGUI cargoText;
	public TextMeshProUGUI fuelText;
	public TextMeshProUGUI speedText;
	public TextMeshProUGUI priceText;
	public GameObject hullBar;
	public GameObject fuelBar;
	public GameObject signalBar;
	
	public Ship displayShip;
	public List<Ship> shipList;
	public int shipIndex = 0;
	
	public GameObject equipBoxPrefab;
	public GameObject equipSellBox;
	public GameObject equipBuyBox;
	
	public EquipmentStockTable equipStock;
	List<Equipment> equipBuyList = new List<Equipment>();
	
	void Start()
	{
		inventory = Inventory.instance;
		
		GenerateStock();
	}
	
	void OnEnable()
	{
		inventory = Inventory.instance;
		UpdateShipStore();
		UpdateEquipStore();
	}
	
	public void SwitchTab(GameObject newScreen) {
		currentScreen.SetActive(false);
		currentScreen = newScreen;
		currentScreen.SetActive(true);
	}
	
	public void UpdateShipStore() {
		SetShipGraphic();
		SetNameText();
		SetDescText();
		SetCargoText();
		SetFuelBar();
		SetEffText();
		SetHullBar();
		SetSpeedText();
		SetSignalBar();
		SetPriceText();
	}
	
	public void PrevShip() {
		if (shipIndex < 1)
		{shipIndex = shipList.Count-1;} 
		else shipIndex--;
		displayShip = shipList[shipIndex];
		UpdateShipStore();
	}
	
	public void NextShip() {
		if (shipIndex >= shipList.Count-1)
		{shipIndex = 0;} 
		else shipIndex++;
		displayShip = shipList[shipIndex];
		UpdateShipStore();
	}

	void SetShipGraphic() {
		shipGraphic.sprite = displayShip.graphic;
	}
	
	void SetNameText() {
		nameText.SetText(displayShip.GetName());
	}
	
	void SetDescText() {
		descText.SetText(displayShip.GetDesc());
	}
	
	void SetHullBar() {
		float hullPercent = (float)displayShip.GetHullMax()/HULLCAP;
		hullBar.transform.localScale = new Vector3(hullPercent, 1, 1);
	}
	
	void SetFuelBar() {
		float fuelPercent = (float)displayShip.GetFuelMax()/ FUELCAP;
		fuelBar.transform.localScale = new Vector3(fuelPercent, 1, 1);
	}
	
	void SetSignalBar() {
		float signalPercent = (float)displayShip.GetRange()/ SIGNALCAP;
		signalBar.transform.localScale = new Vector3(signalPercent, 1, 1);
	}
	
	void SetCargoText() {
		cargoText.SetText("{0}", displayShip.GetCargoMax());
	}
	
	void SetEffText() {
		fuelText.SetText("{0:1}", displayShip.GetFuelEff());
	}
	
	void SetSpeedText() {
		speedText.SetText("{0:1}", displayShip.GetSpeed());
	}
	
	void SetPriceText() {
		priceText.SetText("{0}", displayShip.GetPrice());
	}

	public void UpdateEquipStore() {
		GameObject tempBox;
		
		for (int i = 0; i < equipBuyBox.transform.childCount; i++)
		{
			Destroy(equipBuyBox.transform.GetChild(i).gameObject);
		}
		
		for (int i = 0; i < equipBuyList.Count; i++)
		{
			tempBox = Instantiate(equipBoxPrefab, equipBuyBox.transform);
			tempBox.GetComponent<EquipSlot>().AddEquipment(equipBuyList[i]);
		}
		
		for (int i = 0; i < equipSellBox.transform.childCount; i++)
		{
			Destroy(equipSellBox.transform.GetChild(i).gameObject);
		}
		
		for (int i = 0; i < inventory.equipments.Count; i ++)
		{
			tempBox = Instantiate(equipBoxPrefab, equipSellBox.transform);
			tempBox.GetComponent<EquipSlot>().AddEquipment(inventory.equipments[i]);
		}
		
	}
	
	void GenerateStock()
	{
		List<Equipment> stockList = equipStock.GetEquipmentList();
		List<float> chanceList = equipStock.GetChance();
		
		for (int i = 0; i < stockList.Count; i++)
		{
			if (Random.value > chanceList[i]) {
				equipBuyList.Add(stockList[i]);
			}
		}
	}
	
}
