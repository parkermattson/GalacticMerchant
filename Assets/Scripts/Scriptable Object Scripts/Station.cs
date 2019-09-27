using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType {Mining, Slum, Populous, Paradise, Research, Manufacturing, Farming, Military, Frontier, Refinery}

[CreateAssetMenu(fileName = "New Station", menuName = "Locations/Station")]
public class Station : Location {

	public StationType stationType = StationType.Mining;
	public MarketStockTable stockTable;
	public MarketPriceTable priceTable;
	public int race = 0;
	public List<Factory> initFactories, factories;
	public List<ItemStack> marketInv;
	public int stationMoney = 10000;
	public List<StationModule> modules;
	DateTime lastTime = new DateTime(3000, 1, 1, 9, 0, 0);
	
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
	
	public void Init()
	{
		marketInv = stockTable.GenerateStock();
		foreach (Factory f in initFactories)
		{
			factories.Add(Instantiate(f));
		}
	}
	
	public void RefreshStation()
	{
		DateTime newTime = GameControl.instance.gameTime;
		float deltaTime = (float)(newTime.Subtract(lastTime).TotalHours);
		
		foreach (Factory f in factories)
		{
			f.Refresh(deltaTime);
		}
		
		for (int i = 0; i < deltaTime/24; i++)
		{
			foreach (StationModule m in modules)
			{
				m.Refresh(this);
			}
		}
		
		
	}
}
