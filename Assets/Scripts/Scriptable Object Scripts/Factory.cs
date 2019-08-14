using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : ScriptableObject {

    private string factoryName;
    private string factoryDescription;
    private List<Recipe> recipteList;
    private List<Recipe> queueRecipe;
    private List<int> queueAmt;
    private int currentQueueTime;
    private bool isOwned;
    


}
