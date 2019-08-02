﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType {Mining, Slum, Populous, Paradise, Research, Manufacturing, Farming, Military, Frontier, Refinery}

[CreateAssetMenu(fileName = "New Station", menuName = "Station")]
public class Station : Location {

	public StationType stationType = StationType.Mining;
	public MarketStockTable stockTable;
	public int race = 0;
	
	public StationType GetStationType()
	{
		return stationType;
	}
	
	public MarketStockTable GetStockTable()
	{
		return stockTable;
	}
	
	public int GetRace()
	{
		return race;
	}
}
