using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Mapscreenscript : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler {

    public Image mapImage;
    public Image mapShipIcon;
	public Image warpRangeImage;
    Vector3 mouseStart;
    public GameObject locationTooltip;
    public GameObject hoverTooltip;
    GameObject selectedLocation = null;
    bool inTransit = false;
	float fuelCounter = 0;
	
	[SerializeField]
	private GameObject locationPrefab;
	
	[SerializeField]
	private List<Location> locations;

    [SerializeField]
    private GameControl gcScript;

    [SerializeField]
    private GameObject hullBar;

    [SerializeField]
    private GameObject fuelBar;

    [SerializeField]
    private TextMeshProUGUI cargoText;

    [SerializeField]
    private TextMeshProUGUI moneyText;
	
	[SerializeField]
	private TextMeshProUGUI timeText;
	
	public Location GetLocation(GameObject locationObject)
	{
		return locationObject.GetComponent<LocationScript>().location;
	}

    public void SetInTransit(bool transiting)
    {
		warpRangeImage.gameObject.SetActive(false);
        inTransit = transiting;
    }

    private void MoveShip()
    {
        locationTooltip.SetActive(false);
        mapShipIcon.transform.localPosition = Vector2.MoveTowards(mapShipIcon.transform.localPosition, selectedLocation.transform.localPosition, 5f);
		gcScript.gameTime = gcScript.gameTime.AddHours(.25);
		SetTime();
		fuelCounter+=.1f;
		if (fuelCounter >= 1)
		{
			fuelCounter = 0;
			gcScript.shipState.currentFuel --;
			SetFuelBar();
		}
        if (Mathf.Abs((mapShipIcon.transform.localPosition-selectedLocation.transform.localPosition).magnitude)<=0.1f)
        {
            inTransit = false;
            gcScript.playerLocation = GetLocation(selectedLocation);
			if (gcScript.playerLocation.locationType == LocationType.Station)
			{
				Station tempStation = (Station)gcScript.playerLocation;
				tempStation.RefreshFactories();
				
			}
			
			warpRangeImage.transform.localPosition = mapShipIcon.transform.localPosition;
			warpRangeImage.gameObject.SetActive(true);
        }
    }

    public void SelectLocation(GameObject newSelected)
    {
        if (!inTransit && GetLocation(newSelected) != gcScript.playerLocation)
        {
			if (newSelected != selectedLocation || !locationTooltip.activeSelf)
			{
				selectedLocation = newSelected;
				float distance = Vector2.Distance(gcScript.playerLocation.GetMapPos(), GetLocation(selectedLocation).GetMapPos());
				locationTooltip.SetActive(true);
				locationTooltip.transform.localPosition = selectedLocation.transform.localPosition;
				locationTooltip.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(GetLocation(selectedLocation).GetName() + "\n" + GetLocation(selectedLocation).GetDescription() + "\nDistance: " + distance + "\t\tFuel Cost: " + (int)(distance/50));
				locationTooltip.transform.localPosition = selectedLocation.transform.localPosition;
				if ((int)(distance/50) > gcScript.shipState.currentFuel)
					locationTooltip.transform.GetChild(0).GetComponent<Button>().interactable = false;
				else if (distance > gcScript.shipState.netWarpRange)
					locationTooltip.transform.GetChild(0).GetComponent<Button>().interactable = false;
				else
					locationTooltip.transform.GetChild(0).GetComponent<Button>().interactable=true;
			}
			else 
			{
				locationTooltip.SetActive(false);
				hoverTooltip.SetActive(true);
			}
			
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseStart = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseChange = Input.mousePosition - mouseStart;
        mapImage.transform.position += mouseChange;
        mouseStart = Input.mousePosition;
        CheckBoundary();
    }

    private void CheckBoundary()
    {
        float scaleRatio = mapImage.transform.localScale.x - 0.6f;
        if (mapImage.transform.localPosition.x > 2280 * scaleRatio) mapImage.transform.localPosition = new Vector3(2280 * scaleRatio, mapImage.transform.localPosition.y);
        if (mapImage.transform.localPosition.x < -2280 * scaleRatio) mapImage.transform.localPosition = new Vector3(-2280 * scaleRatio, mapImage.transform.localPosition.y);
        if (mapImage.transform.localPosition.y > (2250 * scaleRatio + 670f)) mapImage.transform.localPosition = new Vector3(mapImage.transform.localPosition.x, 2250 * scaleRatio + 670f);
        if (mapImage.transform.localPosition.y < (-2250 * scaleRatio - 670f)) mapImage.transform.localPosition = new Vector3(mapImage.transform.localPosition.x, -(2250 * scaleRatio + 670f));
    }

    public void OnScroll(PointerEventData eventData)
    {
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

    public void Update()
    {
		if (locationTooltip.activeSelf)
		{
			locationTooltip.transform.position = selectedLocation.transform.position;
			hoverTooltip.SetActive(false);
		}
        if (inTransit) MoveShip();
    }

    void OnEnable()
    {
		PlaceLocations();
		mapShipIcon.transform.localPosition = gcScript.playerLocation.GetMapPos();
		warpRangeImage.transform.localPosition = gcScript.playerLocation.GetMapPos() ;
		warpRangeImage.transform.localPosition -= new Vector3(25, 0);
        SetHullBar();
        SetFuelBar();
        SetCargoSpace();
        SetMoney();
		SetTime();
    }
	
	void PlaceLocations()
	{
		LocationScript[] locationList = this.GetComponentsInChildren<LocationScript>();
        foreach (LocationScript l in locationList)
        {
           Destroy(l.gameObject);
        }
		
		foreach (Location l in locations)
		{
			GameObject tempLocation;
			tempLocation = Instantiate(locationPrefab, mapImage.transform);
			tempLocation.GetComponent<LocationScript>().SetGeneralVars(hoverTooltip, this);
			tempLocation.GetComponent<LocationScript>().location = l;
			tempLocation.transform.localPosition = l.mapPosition;
		}
		
		foreach (Station l in gcScript.stations)
		{
			GameObject tempLocation;
			tempLocation = Instantiate(locationPrefab, mapImage.transform);
			tempLocation.GetComponent<LocationScript>().SetGeneralVars(hoverTooltip, this);
			tempLocation.GetComponent<LocationScript>().location = l;
			tempLocation.GetComponent<Image>().color = Color.red;
			tempLocation.transform.localPosition = l.mapPosition;
		}
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
		
		hullBar.transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("Current Hull: {0}/{1}", gcScript.shipState.currentHull, gcScript.shipState.playerShip.maxHull);

    }
    void SetFuelBar()
    {
        float fuelPercent = (float)gcScript.shipState.currentFuel / (float)gcScript.shipState.playerShip.maxFuel;
        fuelBar.transform.localScale = new Vector3(fuelPercent, 1, 1);
		
		fuelBar.transform.parent.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("Current Fuel: {0}/{1}", gcScript.shipState.currentFuel, gcScript.shipState.playerShip.maxFuel);
    }
    void SetCargoSpace()
    {
        cargoText.SetText("Cargo Space: {0} / {1}", gcScript.shipState.currentCargo, gcScript.shipState.playerShip.maxCargo);
    }
    void SetMoney()
    {
        moneyText.SetText("Spacebucks: {0}", gcScript.playerMoney);
    }
	
	void SetTime()
	{
		timeText.SetText("Time: " + gcScript.gameTime.ToString("HH:mm, MM/dd/yyyy"));
	}
}
