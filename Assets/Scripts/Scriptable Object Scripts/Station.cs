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
	public CrewTable initCrewPool, crewPool;
	public int race = 0;
	public List<Factory> initFactories, factories;
	public List<ItemStack> marketInv = new List<ItemStack>();
	public int stationMoney = 10000;
	public List<StationModule> modules;
	DateTime lastTime = new DateTime(3000, 1, 1, 9, 0, 0);
	List<Crew> availableCrew;
	
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
		//marketInv = stockTable.GenerateStock();
		priceTable = ScriptableObject.CreateInstance<MarketPriceTable>();
		crewPool = Instantiate(initCrewPool);
		GenerateCrewList();
		foreach (Factory f in initFactories)
		{
			factories.Add(Instantiate(f));
		}
		foreach (StationModule m in modules)
		{
			m.Init();
			foreach (Item drain in m.drainItems)
			{
				priceTable.AddDrain(drain);
			}
			
			for (int i = 0; i < m.gainItems.Count; i++)
			{
				ItemStack tempStack = ItemStack.CreateInstance<ItemStack>();
				tempStack.Init(m.gainItems[i], Mathf.CeilToInt(m.gainBase[i]*(1+5* UnityEngine.Random.value)));
				tempStack.AddToList(marketInv);
				priceTable.AddGain(m.gainItems[i]);
			}
		}
	}
	
	public void RefreshStation()
	{
		DateTime newTime = GameControl.instance.gameTime;
		float deltaTime = (float)(newTime.Subtract(lastTime).TotalHours);
		int daysSince = (int)Mathf.Min(deltaTime/24, 50);
		
		GenerateCrewList();
		
		foreach (Factory f in factories)
		{
			f.Refresh(deltaTime);
		}
		
		for (int i = 0; i < daysSince; i++)
		{
			foreach (StationModule m in modules)
			{
				m.Refresh(this);
			}
			foreach (ItemStack stack in marketInv)
			{
				stationMoney += (int)(stack.GetQuantity() * stack.GetItem().GetValue() * .2f);
				stack.AddQuantity((int)(stack.GetQuantity() * -.2f));
			}
		}
		
		lastTime = GameControl.instance.gameTime;
	}
	
	public CrewTable GetCrewTable()
	{
		return crewPool;
	}
	
	public List<Crew> GetAvailableCrew()
	{
		return availableCrew;
	}
	
	void GenerateCrewList()
	{
		availableCrew = new List<Crew>();
		
		for (int i = 0; i < crewPool.GetCrewList().Count; i ++)
		{
			if (availableCrew.Count < 3 && UnityEngine.Random.value < (float)(3 - availableCrew.Count)/(float)(crewPool.GetCrewList().Count - i))
			{
				availableCrew.Add(crewPool.GetCrewAt(i));

			}
		}
	}
	
}
