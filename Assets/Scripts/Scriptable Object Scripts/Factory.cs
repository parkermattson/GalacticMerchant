using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Factory", menuName = "Factory")]
public class Factory : ScriptableObject {

	public Sprite factoryIcon;
    public string factoryName;
    public string factoryDescription;
    public List<Recipe> recipeList;
    public List<Recipe> queueRecipe;
    public List<int> queueAmt;
    public int currentQueueTime = 0;
    bool isOwned = false;
	DateTime lastTime = new DateTime(3000, 1, 1, 9, 0, 0);
    
	public void Refresh(DateTime newTime)
	{
		int deltaTime = (int)(newTime.Subtract(lastTime).TotalHours);
		Debug.Log(deltaTime);
		Debug.Log(currentQueueTime);
		while (deltaTime> 0)
		{
			if (currentQueueTime > deltaTime)
			{
				currentQueueTime -= deltaTime;
				deltaTime = 0;
			}
			else {
				if (queueRecipe.Count > 0)
				{
				deltaTime -= currentQueueTime;
				FinishNextOrder();
				}
				else deltaTime = 0;
			}
			
		}
		lastTime = newTime;
	}
	
	void FinishNextOrder()
	{
		ItemStack newItem = ScriptableObject.CreateInstance<ItemStack>();
		Inventory.instance.AddItem(newItem.Init(queueRecipe[0].product, queueRecipe[0].productQuantity * queueAmt[0]));
		queueRecipe.RemoveAt(0);
		queueAmt.RemoveAt(0);
		if (queueRecipe.Count > 0)
			currentQueueTime = queueRecipe[0].completionTime * queueAmt[0];
	}

}
