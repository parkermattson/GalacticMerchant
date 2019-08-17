using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class LocationScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler{

    public Location location;

    GameObject hoverTooltip;
	Mapscreenscript mapScript;
	
	Vector3 mouseStart;
	
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
	
	public void OnBeginDrag(PointerEventData eventData)		//For tweaking locations more easily
	{
		//mouseStart = Input.mousePosition;
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		/*Vector3 mouseChange = Input.mousePosition - mouseStart;
		transform.position += mouseChange;
		location.mapPosition = (Vector2)transform.localPosition;
		mouseStart = Input.mousePosition; */
	}
	
	public void SetGeneralVars(GameObject newHoverTooltip, Mapscreenscript newMapScript)
	{
		hoverTooltip = newHoverTooltip;
		mapScript = newMapScript;
	}

    
}