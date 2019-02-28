﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

	public Image icon;
	
	Item item;
	
	public void AddItem (Item newItem)
	{
		item = newItem;
		
		icon.sprite = item.icon;
		icon.enabled = true;
	}
	
	public void ClearSlot ()
	{
		item = null;
		
		icon.sprite=null;
		icon.enabled = false;
	}
	
	public Item getItem()
	{
		return item;
	}
}