using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuControl : MonoBehaviour {

	public GameObject activeScreen;
	public GameObject activeBar;
	
	public GameObject tooltip;
	
	public TextMeshProUGUI tooltipText; 
	
	public void screenChange(GameObject newScreen)
	{
		activeScreen.SetActive(false);
		activeScreen = newScreen;
		activeScreen.SetActive(true);
	}
	
	public void ButtonBarChange(GameObject newBar)
	{
		activeBar.SetActive(false);
		activeBar = newBar;
		activeBar.SetActive(true);
	}
	
	public void ShowTooltip(Vector3 position, string description)
	{
		tooltip.SetActive(true);
		tooltip.transform.position = position;
		tooltipText.text = description;
	}
	
	public void HideTooltip()
	{
		tooltip.SetActive(false);
	}

    public void MoveToolTipPivot(Vector2 xy)
    {
        tooltip.gameObject.GetComponent<RectTransform>().pivot = xy;
    }
}
