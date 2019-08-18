using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RecipeBox : MonoBehaviour, IPointerClickHandler {

	Recipe recipe;
	RecipeMenuScript rmScript;
	
	public TextMeshProUGUI nameText;
	public Image icon;
	
	public void SetRecipe(Recipe newRecipe, RecipeMenuScript script)
	{
		recipe = newRecipe;
		rmScript = script;
		nameText.SetText(recipe.recipeName);
		icon.sprite = recipe.product.icon;
	}
	
	public Recipe GetRecipe()
	{
		return recipe;
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		rmScript.SelectRecipe(recipe);
	}
	
	
	
}
