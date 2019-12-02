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
	
}
