using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class ItemMakerScript : MonoBehaviour {

	public TMP_InputField nameInput, descInput, valueInput, vRangeInput, medianInput, qRangeInput;
	public TMP_Dropdown tierInput, typeInput;
	public Sprite[] assetSprites;

	public void MakeItem()
	{
		Item newItem = ScriptableObject.CreateInstance<Item>();
		newItem.itemName = nameInput.text;
		newItem.itemDesc = descInput.text;
		newItem.itemType = (ItemType)typeInput.value;
		newItem.itemTier = tierInput.value + 1;
		newItem.itemValue = int.Parse(valueInput.text);
		newItem.priceRange = int.Parse(vRangeInput.text);
		newItem.medianQuant = int.Parse(medianInput.text);
		newItem.quantityRange = int.Parse(qRangeInput.text);
		
		string assetName = "Assets/Scriptable Objects/Items";
		
		switch (typeInput.value)
		{
			case 0: assetName = assetName + "/RawMats/raw_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[0];
				break;
				
			case 1: assetName = assetName + "/Refined/ref_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[1];
				break;
				
			case 2: assetName = assetName + "/Components/comp_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[2];
				break;
				
			case 3: assetName = assetName + "/Industrial/ind_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[3];
				break;
			
			case 4: assetName = assetName + "/Consumer/cons_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[4];
				break;
			
			case 5: assetName = assetName + "/Agriculture/ag_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[5];
				break;
			
			case 6: assetName = assetName + "/Pharma/pha_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[6];
				break;
			
			case 7: assetName = assetName + "/Exotics/exo_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[7];
				break;
				
			case 8: assetName = assetName + "/Artifacts/art_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[8];
				break;
			
			case 9: assetName = assetName + "/Military/mil_" + nameInput.text.Replace(" ", "").ToLower() + ".asset";
				newItem.icon = assetSprites[9];
				break;
		}
		
		AssetDatabase.CreateAsset(newItem, assetName);
		
		nameInput.text = "";
		descInput.text = "";
		valueInput.text = "";
		vRangeInput.text = "";
		medianInput.text = "";
		qRangeInput.text = "";
	}
}
