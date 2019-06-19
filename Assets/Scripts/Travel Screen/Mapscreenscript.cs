using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Mapscreenscript : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler {

    public Image mapImage;
    Vector3 mouseStart;
    public GameObject locationTooltip;
    public GameObject hoverTooltip;
    GameObject selectedLocation = null;


    public void SelectLocation(GameObject newSelected)
    {
        selectedLocation = newSelected;
        locationTooltip.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().SetText(selectedLocation.GetComponent<LocationScript>().location.GetName() + "\n" + selectedLocation.GetComponent<LocationScript>().location.GetDescription());
		locationTooltip.SetActive(true);
		locationTooltip.transform.localPosition = selectedLocation.transform.localPosition;
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

        LocationScript[] locationList = this.GetComponentsInChildren<LocationScript>();
        foreach (LocationScript l in locationList)
        {
            l.transform.localScale *= .5f / l.transform.lossyScale.x;
        }
    }

    public void Update()
    {
		if (locationTooltip.activeSelf)
		{
			locationTooltip.transform.position = selectedLocation.transform.position;
			hoverTooltip.SetActive(false);
		}
    }
}
