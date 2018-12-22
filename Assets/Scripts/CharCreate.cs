using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharCreate : MonoBehaviour {
	
	public int[] stats = {1, 1, 1, 1};
	public GameObject[] statText = new GameObject[4];

	public void StatTextUp(int statNum)
	{
		if (stats[statNum] < 5)
			stats[statNum]++;
		statText[statNum].GetComponent<TextMeshProUGUI>().SetText("{0}", stats[statNum]);
	}
	
	public void StatTextDown(int statNum)
	{
		if (stats[statNum] > 1)
			stats[statNum]--;
		statText[statNum].GetComponent<TextMeshProUGUI>().SetText("{0}", stats[statNum]);
	}
	
}
