using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,  IPointerClickHandler{

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
	
	public Item GetItem()
	{
		return item;
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		GetComponentInParent<CargoScreenScript>().SelectSlot(gameObject);
	}
}
