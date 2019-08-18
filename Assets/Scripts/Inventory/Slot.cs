using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Slot : MonoBehaviour,  IPointerClickHandler{

	public Image icon;
	
	public TextMeshProUGUI qtText;
	
	[SerializeField]
	ItemStack stack;
	
	public void AddItem (ItemStack newStack)
	{
		stack = newStack;
		
		icon.sprite = stack.GetItem().icon;
		icon.enabled = true;
		qtText.SetText(stack.GetQuantity().ToString());
		qtText.transform.gameObject.SetActive(true);
	}
	
	public void ClearSlot ()
	{
		stack = null;
		
		icon.sprite=null;
		icon.enabled = false;
	}
	
	public ItemStack GetStack()
	{
		return stack;
	}
	
	public Item GetItem()
	{
		if (stack != null)
			return stack.GetItem();
		else return null;
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		GetComponentInParent<CargoScreenScript>().SelectSlot(gameObject);
	}
}
