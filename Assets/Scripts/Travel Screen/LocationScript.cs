using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class LocationScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public Location location;

    public GameObject tooltip;
    public GameObject hoverTooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverTooltip.SetActive(true);
        hoverTooltip.transform.position = transform.position;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTooltip.SetActive(false);
    }

    private void OnEnable()
    {
        transform.localPosition = location.mapPosition;
        hoverTooltip.GetComponentInChildren<TextMeshProUGUI>().SetText("Click to Select\n"+location.GetName()+"\n"+location.GetDescription());
    }

    
}