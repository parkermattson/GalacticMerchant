using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicScreenScript : MonoBehaviour {

	public GameObject currentScreen;
	
	
	public void SwitchTab(GameObject newScreen) {
		currentScreen.SetActive(false);
		currentScreen = newScreen;
		currentScreen.SetActive(true);
	}
}
