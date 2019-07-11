using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicBuySellButton : MonoBehaviour {
	
	MechanicScreenScript msScript;
	
	public void SetMsScript(MechanicScreenScript script)
	{
		msScript = script;
	}

	public void BuyButton(GameObject equipBox)
	{
		msScript.BuyEquipment(equipBox);
	}
	
	public void SellButton(GameObject equipBox)
	{
		msScript.SellEquipment(equipBox);
	}
}
