using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Locations/Encounter")]
public class RandomEncounter : ScriptableObject {

	public string encounterName, encounterDescription;
	public string[] encounterChoiceDesc, encounterSuccessDesc, encounterFailureDesc;
	public Item[] choiceOutcomeItems1, choiceOutcomeItems2, choiceOutcomeItems3, choiceOutcomeItems4;
	public int[] choiceOutcomeQuants1, choiceOutcomeQuants2, choiceOutcomeQuants3, choiceOutcomeQuants4, outcomeMoney, outcomeHealth;
	public float[] successChance;
	
	public Item[] GetChoiceItems(int choice) {
		switch (choice)
		{
			case 0: return choiceOutcomeItems1;
			
			case 1: return choiceOutcomeItems2;
			
			case 2: return choiceOutcomeItems3;
			
			case 3: return choiceOutcomeItems4;
			
			default: return choiceOutcomeItems1;
			
			
		}
	}
	
	public int[] GetChoiceQuants(int choice) {
		switch (choice)
		{
			case 0: return choiceOutcomeQuants1;
			
			case 1: return choiceOutcomeQuants2;
			
			case 2: return choiceOutcomeQuants3;
			
			case 3: return choiceOutcomeQuants4;
			
			default: return choiceOutcomeQuants1;
			
		}
	}
	
	public ItemStack GetChoiceItemStack(int choice, int stackNum) {
		ItemStack newStack = ItemStack.CreateInstance<ItemStack>();
		return newStack.Init(GetChoiceItems(choice)[stackNum], GetChoiceQuants(choice)[stackNum]);
	}
}
