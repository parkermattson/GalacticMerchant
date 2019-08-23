using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndustryScreenScript : MonoBehaviour {

	public GameObject factoryArea, factoryBoxPrefab, queueArea, queueBoxPrefab;
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
		
		SelectFactory(station.factories[0]);
		UpdateDescriptionArea();
	}
	
	public void UpdateDescriptionArea()
	{
		factoryNameText.SetText(selectedFactory.factoryName);
		factoryDescText.SetText(selectedFactory.factoryDescription);
		queueText.SetText("Recipes in queue: " + selectedFactory.queueRecipe.Count.ToString());
		
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
