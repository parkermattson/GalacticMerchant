using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Mapscreenscript : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler {
	
	public const float BASEFUELDRAIN = 0.1f;
	public const int BASESPEED = 3;
	

    public Image mapImage, mapShipIcon, warpRangeImage;
	public List<RandomEncounter> encountersStation, encountersEmpty, encountersAnomaly, encountersTransmission, encountersDistress, encountersConflict, encountersNatural;
	RandomEncounter currentEncounter = null;
	bool inTransit = false, encounterCombatTrigger = false, inEncounter = false;
	float fuelCounter = 1;
	Vector3 mouseStart;
	
	bool pathTest = true;
	public GameObject travelLinePrefab, locationPrefab, npcSpritePrefab;
	List<Location> travelPath = new List<Location>();
	List<GameObject> travelLines = new List<GameObject>();
	List<NpcSpriteScript> caravansOnMap = new List<NpcSpriteScript>();
	int nextLocation;
	
	public GameObject hullBar, fuelBar,  hoverTooltip, locationTooltip, encounterBox, encounterBox2, combatScreen, selectedLocation = null;

    public TextMeshProUGUI cargoText, moneyText, timeText, encounterNameText, encounterDescText, encounterText1, encounterText2, encounterText3 ,encounterText4, encounterSuccessText, encounterOutcomeText, encounterRewardsText;
	
	void Awake() {
		
		PlaceLocations();
		foreach (CaravanNpc npc in GameControl.instance.caravans)
		{
			GameObject tempLocation;
			tempLocation = Instantiate(npcSpritePrefab, mapImage.transform);
			tempLocation.GetComponent<NpcSpriteScript>().Init(npc);
			caravansOnMap.Add(tempLocation.GetComponent<NpcSpriteScript>());
		}
		mapShipIcon.transform.localPosition = selectedLocation.transform.localPosition + new Vector3(0, selectedLocation.GetComponent<RectTransform>().sizeDelta.y/2,0);
		warpRangeImage.transform.localPosition = GameControl.instance.playerLocation.GetMapPos();
		
		mapShipIcon.transform.SetAsLastSibling();
		hoverTooltip.transform.SetAsLastSibling();
		locationTooltip.transform.SetAsLastSibling();
	}
	
    void Update() {
        if (inTransit && !inEncounter) 
		{
			MoveShip();
			foreach (NpcSpriteScript caravan in caravansOnMap)
			{
				caravan.TakeTurn();
			}
		}
    }

	void OnEnable() {
		SetStatusBox();
    }
	
	void OnDisable() {
		locationTooltip.SetActive(false);
	}
	
	public Location GetLocation(GameObject locationObject) {
		return locationObject.GetComponent<LocationScript>().location;
	}

    public void SetInTransit(bool transiting) {
		warpRangeImage.gameObject.SetActive(false);
        inTransit = transiting;
    }

    private void MoveShip() {
        locationTooltip.SetActive(false);
        mapShipIcon.transform.localPosition = Vector2.MoveTowards(mapShipIcon.transform.localPosition, travelPath[nextLocation].mapPosition, BASESPEED * GameControl.instance.playerShip.GetNetSpeed(true));
		GameControl.instance.PassTime(.05f);
		UpdateLocationColors();
		SetTimeText();
		fuelCounter+= BASEFUELDRAIN / GameControl.instance.playerShip.GetNetFuelEff(true);
		if (fuelCounter >= 1)
		{
			fuelCounter = 0;
			GameControl.instance.playerShip.currentFuel --;
			SetFuelBar();
		}
        if (Mathf.Abs(((Vector2)(mapShipIcon.transform.localPosition)-travelPath[nextLocation].mapPosition).magnitude)<=0.1f)
        {
            
            GameControl.instance.playerLocation = travelPath[nextLocation];
			mapShipIcon.transform.localPosition = travelPath[nextLocation].mapPosition + new Vector2(0, selectedLocation.GetComponent<RectTransform>().sizeDelta.y/2);
			RollEncounter(travelPath[nextLocation]);
			if (GameControl.instance.playerLocation.locationType == LocationType.Station)
			{
				Station tempStation = (Station)GameControl.instance.playerLocation;
				tempStation.RefreshStation();
				
			}
			
			if (nextLocation == travelPath.Count - 1)
			{
				inTransit = false;
				warpRangeImage.transform.localPosition = mapShipIcon.transform.localPosition;
				warpRangeImage.gameObject.SetActive(true);
				while (travelLines.Count > 0)
				{
					Destroy(travelLines[0]);
					travelLines.RemoveAt(0);
				}
			}
			else
			{
				nextLocation++;
			}
        }
    }

    public void SelectLocation(GameObject newSelected) {
        
		if (!inTransit && GetLocation(newSelected) != GameControl.instance.playerLocation)
        {
			if (newSelected != selectedLocation || !locationTooltip.activeSelf)
			{
				selectedLocation = newSelected;
				locationTooltip.SetActive(true);
				SetHoverTooltip(false, selectedLocation);
				locationTooltip.transform.position = selectedLocation.transform.position + new Vector3(selectedLocation.GetComponent<RectTransform>().sizeDelta.x/2,0,0);
				if (pathTest == false)
				{
					float distance = Vector2.Distance(GameControl.instance.playerLocation.GetMapPos(), GetLocation(selectedLocation).GetMapPos());	
					
					locationTooltip.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().SetText(GetLocation(selectedLocation).GetName() + "\n" + GetLocation(selectedLocation).GetDescription() + "\nDistance: " + distance + "\nFuel Cost: " + Mathf.CeilToInt(distance * BASEFUELDRAIN / (BASESPEED * GameControl.instance.playerShip.GetNetSpeed(true) * GameControl.instance.playerShip.GetNetFuelEff(true))));
					if ((int)(distance/50) > GameControl.instance.playerShip.currentFuel)
						locationTooltip.transform.GetComponentInChildren<Button>().interactable = false;
					else if (distance > GameControl.instance.playerShip.GetNetWarpRange(true))
						locationTooltip.transform.GetComponentInChildren<Button>().interactable = false;
					else
						locationTooltip.transform.GetComponentInChildren<Button>().interactable=true;
					
					
				} 
				else
				{
					travelPath = GameControl.instance.FindShortestPath(GameControl.instance.playerLocation, GetLocation(newSelected), GameControl.instance.playerShip.GetNetWarpRange(true));
					nextLocation = 1;
					if (travelPath != null)
					{
						Vector3[] pathCoords = new Vector3[30];
						float fuelCost = 0;
						pathCoords[0] = new Vector3(travelPath[0].mapPosition.x, travelPath[0].mapPosition.y,  0);
						for (int i = 1; i < travelPath.Count; i++)
						{
							pathCoords[i] = new Vector3(travelPath[i].mapPosition.x, travelPath[i].mapPosition.y,  0);
							fuelCost += Vector2.Distance(travelPath[i-1].mapPosition, travelPath[i].mapPosition) * BASEFUELDRAIN / (BASESPEED * GameControl.instance.playerShip.GetNetSpeed(true) * GameControl.instance.playerShip.GetNetFuelEff(true));
							if (i > travelLines.Count)
								travelLines.Add(Instantiate(travelLinePrefab, mapImage.transform));
							travelLines[i-1].transform.SetAsFirstSibling();
							travelLines[i-1].transform.localPosition = (pathCoords[i-1]+pathCoords[i])/2;
							travelLines[i-1].GetComponent<RectTransform>().sizeDelta = new Vector2(Vector3.Distance(pathCoords[i-1], pathCoords[i]), 30);
							travelLines[i-1].transform.localRotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan((pathCoords[i-1].y - pathCoords[i].y)/(pathCoords[i-1].x - pathCoords[i].x)));
						}
						for (int i=travelLines.Count-1; i >= travelPath.Count-1; i--)
						{
							GameObject tempObj = travelLines[i];
							travelLines.Remove(travelLines[i]);
							Destroy(tempObj);
							
						}
						
						locationTooltip.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().SetText(travelPath[1].GetName() + "\n" + travelPath[1].GetDescription() +  "\nFuel Cost: " + Mathf.CeilToInt(fuelCost));
					
					}
					else
					{
						locationTooltip.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().SetText(GetLocation(selectedLocation).GetName() + "\n" + GetLocation(selectedLocation).GetDescription() +  "\nOut of Range");
						locationTooltip.transform.GetComponentInChildren<Button>().interactable=false;
					}
				}
			}
			else 
			{
				locationTooltip.SetActive(false);
				SetHoverTooltip(true, newSelected);
			}
			
        }
    }
	
    public void OnBeginDrag(PointerEventData eventData) {
        mouseStart = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData) {
        Vector3 mouseChange = Input.mousePosition - mouseStart;
        mapImage.transform.position += mouseChange;
        mouseStart = Input.mousePosition;
        CheckBoundary();
    }

    private void CheckBoundary() {
        float scaleRatio = mapImage.transform.localScale.x - 0.6f;
        if (mapImage.transform.localPosition.x > 2280 * scaleRatio) mapImage.transform.localPosition = new Vector3(2280 * scaleRatio, mapImage.transform.localPosition.y);
        if (mapImage.transform.localPosition.x < -2280 * scaleRatio) mapImage.transform.localPosition = new Vector3(-2280 * scaleRatio, mapImage.transform.localPosition.y);
        if (mapImage.transform.localPosition.y > (2250 * scaleRatio + 670f)) mapImage.transform.localPosition = new Vector3(mapImage.transform.localPosition.x, 2250 * scaleRatio + 670f);
        if (mapImage.transform.localPosition.y < (-2250 * scaleRatio - 670f)) mapImage.transform.localPosition = new Vector3(mapImage.transform.localPosition.x, -(2250 * scaleRatio + 670f));
    }

    public void OnScroll(PointerEventData eventData) {
        float scaleChange = 0.25f * Input.GetAxis("Mouse ScrollWheel");
        mapImage.transform.localScale += new Vector3(scaleChange, scaleChange);
        if (mapImage.transform.localScale.x > 1.5) mapImage.transform.localScale = new Vector3(1.5f, 1.5f);
        else if (mapImage.transform.localScale.x < 0.6) mapImage.transform.localScale = new Vector3(0.6f, 0.6f);
        CheckBoundary();

        Canvas canvas = FindObjectOfType<Canvas>();
        LocationScript[] locationList = this.GetComponentsInChildren<LocationScript>();
        foreach (LocationScript l in locationList)
        {
            l.transform.localScale *= canvas.transform.lossyScale.x/l.transform.lossyScale.x;
        }

        mapShipIcon.transform.localScale *= canvas.transform.lossyScale.x / mapShipIcon.transform.lossyScale.x;
    }
    
	public void WaitOneYear() {
		GameControl.instance.gameTime = GameControl.instance.gameTime.AddDays(365);
	}
	
	void PlaceLocations() {
		Canvas canvas = FindObjectOfType<Canvas>();
        foreach (LocationScript l in GetComponentsInChildren<LocationScript>())
        {
           Destroy(l.gameObject);
        }
		
		foreach (Location l in GameControl.instance.locations)
		{
			GameObject tempLocation;
			tempLocation = Instantiate(locationPrefab, mapImage.transform);
			tempLocation.GetComponent<LocationScript>().SetGeneralVars(hoverTooltip, this);
			tempLocation.GetComponent<LocationScript>().location = l;
			tempLocation.transform.localPosition = l.mapPosition;
			if (l == GameControl.instance.playerLocation)
				selectedLocation = tempLocation;
			
			tempLocation.transform.localScale *= canvas.transform.lossyScale.x/tempLocation.transform.lossyScale.x;
		}
		
		UpdateLocationColors();
		
		foreach (Station l in GameControl.instance.stations)
		{
			GameObject tempLocation = Instantiate(locationPrefab, mapImage.transform);
			tempLocation.GetComponent<LocationScript>().SetGeneralVars(hoverTooltip, this);
			tempLocation.GetComponent<LocationScript>().location = l;
			tempLocation.GetComponent<Image>().color = Color.blue;
			tempLocation.transform.localPosition = l.mapPosition;
			if (l == GameControl.instance.playerLocation)
				selectedLocation = tempLocation;
		}
	}
	
	void UpdateLocationColors() {
		foreach (LocationScript l in GetComponentsInChildren<LocationScript>())
		{
			switch (l.location.locationType)
				{
					case LocationType.Empty:
						l.gameObject.GetComponent<Image>().color = Color.grey;
						break;
						
					case LocationType.Natural:
						l.gameObject.GetComponent<Image>().color = Color.green;
						break;
						
					case LocationType.Anomaly:
						l.gameObject.GetComponent<Image>().color = new Color(.6f, 0f, .6f, 1f);
						break;
						
					case LocationType.Distress:
						l.gameObject.GetComponent<Image>().color = Color.yellow;
						break;
						
					case LocationType.Transmission:
						l.gameObject.GetComponent<Image>().color = Color.cyan;
						break;
						
					case LocationType.Conflict:
						l.gameObject.GetComponent<Image>().color = Color.red;
						break;
				}
		}
	}
	
	void RollEncounter(Location loc) {
		
		float rng = UnityEngine.Random.value;
		
		switch (loc.locationType)
		{
			case LocationType.Station:
				if (rng < .25f)
				{
					rng = UnityEngine.Random.value * encountersStation.Count;
					inEncounter = true;
					SetEncounterBox(encountersStation[Mathf.FloorToInt(rng)]);
				}
				break;
			case LocationType.Empty:
				if (rng < .25f)
				{
					rng = UnityEngine.Random.value * encountersEmpty.Count;
					inEncounter = true;
					SetEncounterBox(encountersEmpty[Mathf.FloorToInt(rng)]);
				}
				break;
			case LocationType.Anomaly: 
				if (rng < .25f)
				{
					rng = UnityEngine.Random.value * encountersAnomaly.Count;
					inEncounter = true;
					SetEncounterBox(encountersAnomaly[Mathf.FloorToInt(rng)]);
				}
				break;
			case LocationType.Transmission: 
				if (rng < .25f)
				{
					rng = UnityEngine.Random.value * encountersTransmission.Count;
					inEncounter = true;
					SetEncounterBox(encountersTransmission[Mathf.FloorToInt(rng)]);
				}
				break;
			case LocationType.Distress: 
				if (rng < .25f)
				{
					rng = UnityEngine.Random.value * encountersDistress.Count;
					inEncounter = true;
					SetEncounterBox(encountersDistress[Mathf.FloorToInt(rng)]);
				}
				break;
			case LocationType.Conflict: 
				if (rng < .25f)
				{
					rng = UnityEngine.Random.value * encountersConflict.Count;
					inEncounter = true;
					SetEncounterBox(encountersConflict[Mathf.FloorToInt(rng)]);
				}
				break;
			case LocationType.Natural:
				if (rng < .25f)
				{
					rng = UnityEngine.Random.value * encountersNatural.Count;
					inEncounter = true;
					SetEncounterBox(encountersNatural[Mathf.FloorToInt(rng)]);
				}
				break;
		}
	}
	
	public void ChooseEncounter(int choice) {
		string rewardsText = "";
		encounterBox.SetActive(false);
		encounterBox2.SetActive(true);
		float rng = UnityEngine.Random.value;
		if (rng < currentEncounter.successChance[choice])
		{
			encounterSuccessText.SetText("Success!");
			encounterOutcomeText.SetText(currentEncounter.encounterSuccessDesc[choice]);
			
			if (currentEncounter.outcomeMoney[choice] > 0)
			{
				rewardsText = "- Money Gained: " + currentEncounter.outcomeMoney[choice].ToString() + "\n";
				GameControl.instance.AddMoney(currentEncounter.outcomeMoney[choice]);
			} else if (currentEncounter.outcomeMoney[choice] < 0)
			{
				int money = -currentEncounter.outcomeMoney[choice];
				rewardsText = "- Money Lost: " + money.ToString() + "\n";
				GameControl.instance.AddMoney(currentEncounter.outcomeMoney[choice]);
			}
			
			if (currentEncounter.outcomeHealth[choice] != 0)
			{
				rewardsText = rewardsText + "- Health Lost: " + currentEncounter.outcomeHealth[choice].ToString() + "\n";
				GameControl.instance.playerShip.AddHealth(currentEncounter.outcomeHealth[choice]);
			}
			
			for (int i = 0; i < currentEncounter.GetChoiceItems(choice).Length; i++)
			{
				rewardsText = rewardsText + "-  " + currentEncounter.GetChoiceQuants(choice)[i].ToString() + " " + currentEncounter.GetChoiceItems(choice)[i].itemName + "\n";
				Inventory.instance.AddItem(currentEncounter.GetChoiceItemStack(choice, i));
			}
			
			encounterRewardsText.SetText(rewardsText);
			
		}
		else 
		{
			encounterSuccessText.SetText("Failure!");
			encounterOutcomeText.SetText(currentEncounter.encounterFailureDesc[choice]);
			
			encounterCombatTrigger = currentEncounter.CheckCombatTrigger(choice);
			
			encounterRewardsText.SetText("Impliment failure consequences maybe");
			
		}
		SetStatusBox();
	}
	
	public void EncounterCombatCheck() {
		if (encounterCombatTrigger)
			{
				combatScreen.SetActive(true);
				inEncounter = true;
																																		//Add enemy generation function here
			}
			else inEncounter = false;
	}
	
	void SetEncounterBox(RandomEncounter encounter) {
		encounterBox.SetActive(true);
		currentEncounter = encounter;
		encounterNameText.SetText(encounter.encounterName);
		encounterDescText.SetText(encounter.encounterDescription);
		encounterText1.SetText(encounter.encounterChoiceDesc[0]);
		encounterText2.SetText(encounter.encounterChoiceDesc[1]);
		encounterText3.SetText(encounter.encounterChoiceDesc[2]);
		encounterText4.SetText(encounter.encounterChoiceDesc[3]);
	}
	
	public void SetInEncounter(bool newState) {
		inEncounter = newState;
	}
	
    public void SetStatusBox() {
		SetHullBar();
        SetFuelBar();
        SetCargoSpaceText();
        SetMoneyText();
		SetTimeText();
	}
	
	void SetHullBar() {
        float hullPercent = (float)GameControl.instance.playerShip.currentHull / (float)GameControl.instance.playerShip.maxHull;
        hullBar.transform.localScale = new Vector3(hullPercent, 1, 1);

        if (hullPercent > .66)
            hullBar.GetComponent<Image>().color = Color.green;
        else if (hullPercent > .33)
            hullBar.GetComponent<Image>().color = Color.yellow;
        else
            hullBar.GetComponent<Image>().color = Color.red;
		
		hullBar.transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("Current Hull: {0}/{1}", GameControl.instance.playerShip.currentHull, GameControl.instance.playerShip.maxHull);

    }
	
	void SetFuelBar() {
        float fuelPercent = (float)GameControl.instance.playerShip.currentFuel / (float)GameControl.instance.playerShip.GetFuelMax();
        fuelBar.transform.localScale = new Vector3(fuelPercent, 1, 1);
		
		fuelBar.transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("Current Fuel: {0}/{1}", GameControl.instance.playerShip.currentFuel, GameControl.instance.playerShip.maxFuel);
    }
	
    void SetCargoSpaceText() {
        cargoText.SetText("Cargo Space: {0} / {1}", Inventory.instance.currentCargo, GameControl.instance.playerShip.maxCargo);
    }
	
	void SetMoneyText() {
        moneyText.SetText("Spacebucks: {0}", GameControl.instance.playerMoney);
    }
	
	void SetTimeText() {
		timeText.SetText("Time: " + GameControl.instance.gameTime.ToString("HH:mm, MM/dd/yyyy"));
	}

	public void SetHoverTooltip(bool isActive, GameObject locationObject) {
		if (!locationTooltip.activeSelf)
		{
			hoverTooltip.SetActive(isActive);
			if (isActive)
			{
				LocationScript lScript = locationObject.GetComponent<LocationScript>();
				hoverTooltip.GetComponentInChildren<TextMeshProUGUI>().SetText("Click to Select\n"+lScript.location.GetName()+"\n"+ lScript.location.GetDescription());
				hoverTooltip.transform.position = locationObject.transform.position + new Vector3(locationObject.GetComponent<RectTransform>().sizeDelta.x/2,0,0);
			}
		}
		else hoverTooltip.SetActive(false);
	}
	
}
