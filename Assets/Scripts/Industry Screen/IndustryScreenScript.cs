using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndustryScreenScript : MonoBehaviour {

	public GameObject factoryArea, factoryBoxPrefab, queueArea, queueBoxPrefab, orderButton, buyButton;
	public TextMeshProUGUI factoryNameText, factoryDescText, queueText;
	
	public RecipeMenuScript rmScript;
	GameControl gcScript;
	Station station;
	
	Factory selectedFactory= null;
	
	void Awake()
	{
		gcScript = GameControl.instance;
		station = (Station)gcScript.playerLocation;
	}
	
	void OnEnable()
	{
		station = (Station)gcScript.playerLocation;
		UpdateFactoryArea();
	}
	
	void UpdateFactoryArea()
	{
		GameObject tempBox = null;
		foreach (Transform child in factoryArea.transform)
		{
			if (child != transform)
				Destroy(child.gameObject);
		}
		
		for (int i = 0; i < station.factories.Count; i++)
		{
			tempBox = Instantiate(factoryBoxPrefab, factoryArea.transform);
			tempBox.GetComponent<FactoryBox>().SetFactory(station.factories[i], this);
		}
		
		if (station.factories.Count > 0)
			SelectFactory(station.factories[0]);
		else SelectFactory(null);
		UpdateDescriptionArea();
	}
	
	public void UpdateDescriptionArea()
	{
		if (selectedFactory != null)
		{
			orderButton.GetComponent<Button>().interactable = true;
			buyButton.GetComponent<Button>().interactable = true;
			factoryNameText.SetText(selectedFactory.factoryName);
			factoryDescText.SetText(selectedFactory.factoryDescription);
			if (selectedFactory.queueRecipe.Count > 0)
				queueText.SetText("Current Order Complete in: {0} Hours", selectedFactory.currentQueueTime);
			else queueText.SetText("No orders in queue.");
			
			GameObject tempBox;
			
			foreach (Transform child in queueArea.transform)
			{
				if (child != transform)
					Destroy(child.gameObject);
			}
			
			for (int i = 0; i < selectedFactory.queueRecipe.Count; i++)
			{
				tempBox = Instantiate(queueBoxPrefab, queueArea.transform);
				tempBox.GetComponent<QueueBox>().SetQueue(selectedFactory, selectedFactory.queueRecipe[i], selectedFactory.queueAmt[i], i,this);
			}
		} else {
			factoryNameText.SetText("");
			factoryDescText.SetText("");
			queueText.SetText("");
			orderButton.GetComponent<Button>().interactable = false;
			buyButton.GetComponent<Button>().interactable = false;
		}
		
	}
	
	public void OpenRecipeMenu()
	{
		rmScript.selectedFactory = selectedFactory;
		rmScript.gameObject.SetActive(true);
	}
	
	public void SelectFactory(Factory newFactory)
	{
		selectedFactory = newFactory;
		UpdateDescriptionArea();
	}

	
}
