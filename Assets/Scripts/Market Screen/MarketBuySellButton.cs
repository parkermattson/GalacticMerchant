using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketBuySellButton : MonoBehaviour {
    
    MarketScreenScript mksScript;

    public void SetMksScript(MarketScreenScript script)
    {
        mksScript = script;
    }

    public void BuyButton(GameObject equipBox)
    {
        mksScript.BuyItem(equipBox);
    }

    public void SellButton(GameObject equipBox)
    {
        mksScript.SellItem(equipBox);
    }
}