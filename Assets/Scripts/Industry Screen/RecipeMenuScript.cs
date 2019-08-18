using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeMenuScript : MonoBehaviour {

	public GameObject recipeArea, recipeBoxPrefab;
	
	public TextMeshProUGUI nameText, descText;
	public TMP_InputField quantityInput;
	
	public Factory selectedFactory;
	Recipe selectedRecipe;
	
	int orderQuantity;
	
	void OnEnable()
	{
		GameObject tempBox = null;
		
		foreach (Transform child in recipeArea.transform)
		{
			if (child != recipeArea.transform)
				Destroy(child.gameObject);
		}
		
		foreach (Recipe r in selectedFactory.recipeList)
		{
			tempBox = Instantiate(recipeBoxPrefab,recipeArea.transform);
			tempBox.GetComponent<RecipeBox>().SetRecipe(r, this);
		}
		
		SelectRecipe(selectedFactory.recipeList[0]);
	}
	
	public void UpdateRecipe()
	{
		nameText.SetText(selectedRecipe.recipeName);
		descText.SetText(selectedRecipe.recipeDescription);
		quantityInput.text = "1";
		UpdateOrder();
	}
	
	public void UpdateOrder()
	{
		if (!int.TryParse(quantityInput.text, out orderQuantity))
		{
			quantityInput.text = "0";
			orderQuantity = 0;
		}
	}
	
	public void AddJob()
	{
		Recipe recipe = selectedRecipe;
		selectedFactory.queueRecipe.Add(recipe);
		selectedFactory.queueAmt.Add(orderQuantity);
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		for (int i =0; i < recipe.ingredients.Count; i++)
			Inventory.instance.RemoveItem(tempStack.Init(recipe.ingredients[i], recipe.ingredientQuantities[i]));
		orderQuantity = 1;
		quantityInput.text = "1";
	}
	
	public void SelectRecipe(Recipe newSelectedRecipe)
	{
		selectedRecipe = newSelectedRecipe;
		UpdateRecipe();
	}
	
}
