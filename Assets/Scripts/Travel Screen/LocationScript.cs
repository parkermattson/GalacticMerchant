using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class LocationScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public Location location;

    GameObject tooltip;
    GameObject hoverTooltip;
	Mapscreenscript mapScript;
	
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverTooltip.SetActive(true);
		hoverTooltip.GetComponentInChildren<TextMeshProUGUI>().SetText("Click to Select\n"+location.GetName()+"\n"+location.GetDescription());
        hoverTooltip.transform.position = transform.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTooltip.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
	{
		mapScript.SelectLocation(transform.gameObject);
	}
	
	public void SetGeneralVars(GameObject newTooltip, GameObject newHoverTooltip, Mapscreenscript newMapScript)
	{
		tooltip = newTooltip;
		hoverTooltip = newHoverTooltip;
		mapScript = newMapScript;
	}

    
}