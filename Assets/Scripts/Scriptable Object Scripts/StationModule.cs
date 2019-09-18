using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Station Module", menuName = "Station Module")]
public class StationModule : ScriptableObject {

	float moduleLevel = 1f;
	public List<Item> drainItems, gainItems;
	public List<int> drainBase, drainInc, gainBase, gainInc, eqBase, eqInc;
	


	
}
