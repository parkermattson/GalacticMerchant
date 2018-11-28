using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour {

	//Menu Vars
	public GameObject activeScreen;
	
	public void screenChange(GameObject newScreen)
	{
		activeScreen.SetActive(false);
		activeScreen = newScreen;
		activeScreen.SetActive(true);
	}
}
