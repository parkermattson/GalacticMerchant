using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeMenuScript : MonoBehaviour {

	public GameObject recipeArea, recipeBoxPrefab, orderButton;
	
	public TextMeshProUGUI nameText, descText, detailText;
	public TMP_InputField quantityInput;
	
	public Factory selectedFactory;
	Recipe selectedRecipe;
	
	int orderQuantity, oldOrderQuantity;
	
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
		quantityInput.text = "0";
		UpdateOrder();
	}
		
	
	public void UpdateOrder()
	{
		string[] tempIngred = new string[4];
		int[] tempQuants = {0,0,0,0};
		ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
		orderButton.GetComponent<Button>().interactable = true;
		if (!int.TryParse(quantityInput.text, out orderQuantity))
		{
			orderQuantity = 0;
		}
		detailText.text = "";
		for (int i = 0; i < selectedRecipe.GetIngredients().Count; i++)
		{
			tempQuants[i] = selectedRecipe.GetIngredientQuantities()[i] * orderQuantity;
			tempIngred[i] = tempQuants[i].ToString() + " " + selectedRecipe.GetIngredients()[i].itemName;
			detailText.SetText(detailText.text + tempIngred[i] + "\n");
			
			
			if (!(Inventory.instance.FindItem(tempStack.Init(selectedRecipe.ingredients[i], tempQuants[i]))))
			{
				orderButton.GetComponent<Button>().interactable = false;
			}
		}
		 detailText.SetText(detailText.text + "{0} Hours to complete\nProduces {1} " + selectedRecipe.product.itemName, selectedRecipe.completionTime * orderQuantity, selectedRecipe.productQuantity * orderQuantity);
	}
	
	public void AddJob()
	{
		if (orderQuantity > 0)
		{
			Recipe recipe = selectedRecipe;
			selectedFactory.queueRecipe.Add(recipe);
			selectedFactory.queueAmt.Add(orderQuantity);
			if (selectedFactory.queueRecipe.Count == 1)
				selectedFactory.currentQueueTime = recipe.completionTime * orderQuantity;
			ItemStack tempStack = ScriptableObject.CreateInstance<ItemStack>();
			for (int i =0; i < recipe.ingredients.Count; i++)
				Inventory.instance.RemoveItem(tempStack.Init(recipe.ingredients[i], recipe.ingredientQuantities[i]));
			orderQuantity = 0;
			quantityInput.text = "0";
		}
	}
	
	public void SelectRecipe(Recipe newSelectedRecipe)
	{
		selectedRecipe = newSelectedRecipe;
		UpdateRecipe();
	}
	
	public void CheckInput()
	{
		if (!int.TryParse(quantityInput.text, out orderQuantity))
		{
			quantityInput.text = "0";
			orderQuantity = 0;
		}
	}
	
	public void PlusMinusButton(bool plus)
	{
		int addQuant = 1;
		if (plus)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				addQuant *= 10;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				addQuant *= 100;
			}
		} else 
		{
			addQuant *= -1;
			if (Input.GetKey(KeyCode.LeftControl))
			{
				addQuant *= 10;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				addQuant *= 100;
			}
		}
		
		orderQuantity += addQuant;
		if (orderQuantity < 0) orderQuantity = 0;
		quantityInput.text = orderQuantity.ToString();
		UpdateOrder();
	}
	
}
