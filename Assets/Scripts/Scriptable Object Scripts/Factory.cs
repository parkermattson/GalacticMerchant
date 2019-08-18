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
    int currentQueueTime = 0;
    bool isOwned = false;
    


}
