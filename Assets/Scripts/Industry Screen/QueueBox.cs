using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QueueBox : MonoBehaviour {

	Factory factory;
	Recipe recipe;
	IndustryScreenScript indScript;
	int quantity, queueIndex;
	

	public Image recipeIcon;
	public TextMeshProUGUI nameText, quantityText;
	
	public void SetQueue(Factory newFactory, Recipe newRecipe, int newQuantity, int newIndex, IndustryScreenScript script)
	{
		factory = newFactory;
		recipe = newRecipe;
		quantity = newQuantity;
		queueIndex = newIndex;
		indScript = script;
		
		nameText.SetText(recipe.recipeName);
		quantityText.SetText("Quantity: " + quantity.ToString());
		recipeIcon.sprite = recipe.product.icon;
	}
	
	public void ClearQueue()
	{
		factory.queueRecipe.RemoveAt(queueIndex);
		factory.queueAmt.RemoveAt(queueIndex);
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		for (int i = 0; i < recipe.ingredients.Count; i++)
		{
			tempStack.Init(recipe.ingredients[i], recipe.ingredientQuantities[i]);
			Inventory.instance.AddItem(tempStack);
		}
		indScript.UpdateDescriptionArea();
		
	}
}
