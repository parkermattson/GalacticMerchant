using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CargoScreenScript : MonoBehaviour {

	public GameObject selectedSlot;
	public TextMeshProUGUI itemName, itemQuantity, itemWeight, itemDesc;
	
	void OnEnable()
	{
		Slot slotScript = selectedSlot.GetComponent<Slot>();
		if (slotScript.GetItem() != null)
		{
			itemName.SetText(slotScript.GetItem().GetName());
			itemDesc.SetText(slotScript.GetItem().GetDescription());
			//itemValue.SetText("Qt: " + slotScript.GetItem().GetValue());
			//itemWeight.SetText("Wt: " + slotScript.GetItem().GetWeight());
		}
		else {
			itemName.SetText("");
			itemDesc.SetText("");
			itemQuantity.SetText("");
			itemWeight.SetText("");
		}
	}
	
	public void SelectSlot(GameObject slot)
	{
		selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
		selectedSlot = slot;
		selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
		Slot slotScript = selectedSlot.GetComponent<Slot>();
		if (slotScript.GetItem() != null)
		{
			itemName.SetText(slotScript.GetItem().GetName());
			itemDesc.SetText(slotScript.GetItem().GetDescription());
			//itemValue.SetText("Qt: " + slotScript.GetItem().GetValue());
			//itemWeight.SetText("Wt: " + slotScript.GetItem().GetWeight());
		} else
		{
			itemName.SetText("");
			itemDesc.SetText("");
			//itemValue.SetText("");
			//itemWeight.SetText("");
		}
	}
}
