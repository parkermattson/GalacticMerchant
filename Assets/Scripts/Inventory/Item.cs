using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	public string itemName = "Name";
	public Sprite icon = null;
	public int itemValue = 0;
	public int itemWeight = 0;
}
