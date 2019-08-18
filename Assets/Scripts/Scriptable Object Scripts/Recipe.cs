using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject {

	public string recipeName = "";
	public string recipeDescription = "";
	public List<Item> ingredients;
	public List<int> ingredientQuantities;
	public Item product;
	public int productQuantity;
	public int completionTime;
	
	public string GetName() {
		return recipeName;
	}
	
	public string GetDesc() {
		return recipeDescription;
	}
	
	public List<Item> GetIngredients() {
		return ingredients;
	}
	
	public List<int> GetIngredientQuantites() {
		return ingredientQuantities;
	}
	
	public Item GetProduct() {
		return product;
	}
	
	public int GetProductQuantity() {
		return productQuantity;
	}
	
}
