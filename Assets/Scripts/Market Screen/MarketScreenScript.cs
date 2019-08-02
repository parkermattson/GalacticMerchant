using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuyItem(GameObject equipBox)
    {
        gameControl.playerMoney -= equipBox.GetComponent<EquipSlot>().equipment.GetValue();
        inventory.equipments.Add(equipBox.GetComponent<EquipSlot>().equipment);
        equipBuyList.Remove(equipBox.GetComponent<EquipSlot>().equipment);
        UpdateEquipStore();
    }

    public void SellItem(GameObject equipBox)
    {
        gameControl.playerMoney += equipBox.GetComponent<EquipSlot>().equipment.GetValue();
        inventory.equipments.Remove(equipBox.GetComponent<EquipSlot>().equipment);
        equipBuyList.Add(equipBox.GetComponent<EquipSlot>().equipment);
        UpdateEquipStore();
    }

}
