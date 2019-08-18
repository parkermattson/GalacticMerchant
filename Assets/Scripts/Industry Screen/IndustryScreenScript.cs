using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndustryScreenScript : MonoBehaviour {

	public GameObject factoryArea, factoryBoxPrefab, queueArea, queueBoxPrefab;
	public TextMeshProUGUI factoryNameText, factoryDescText, queueText;
	
	public RecipeMenuScript rmScript;
	
	public List<Factory> factoryList;
	
	Factory selectedFactory= null;
	
	void OnEnable()
	{
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
		
		for (int i = 0; i < factoryList.Count; i++)
		{
			tempBox = Instantiate(factoryBoxPrefab, factoryArea.transform);
			tempBox.GetComponent<FactoryBox>().SetFactory(factoryList[i], this);
		}
		
		SelectFactory(factoryList[0]);
	}
	
	public void UpdateDescriptionArea()
	{
		factoryNameText.SetText(selectedFactory.factoryName);
		factoryDescText.SetText(selectedFactory.factoryDescription);
		
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
