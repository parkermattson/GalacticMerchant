using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LocationScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private MenuControl menuControl;

    [TextArea]
    [SerializeField]
    private string locationDescription;
    
    void Start ()
    {
        menuControl = GameObject.FindObjectOfType(typeof(MenuControl)) as MenuControl;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuControl.ShowTooltip(transform.position, locationDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menuControl.HideTooltip();
    }
}
