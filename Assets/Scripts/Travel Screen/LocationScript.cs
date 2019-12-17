using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class LocationScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler{

    public Location location;
	
	Mapscreenscript mapScript;
	
	Vector3 mouseStart;
	
    public void OnPointerEnter(PointerEventData eventData)
    {
		mapScript.SetHoverTooltip(true, gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mapScript.SetHoverTooltip(false, gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
	{
		mapScript.SelectLocation(gameObject);
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
		mapScript = newMapScript;
	}

    
}