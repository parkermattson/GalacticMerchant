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
	public NpcFaction faction = NpcFaction.Fac1;
	public List<Factory> initFactories, factories;
	public List<ItemStack> marketInv = new List<ItemStack>(), gains = new List<ItemStack>(), drains = new List<ItemStack>();
	public int stationMoney = 10000;
	public List<StationModule> modules;
	public List<CaravanNpc> stationCaravans = new List<CaravanNpc>();
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
			for (int i = 0; i < m.drainItems.Count; i++)
			{
				ItemStack tempStack = ItemStack.CreateInstance<ItemStack>();
				tempStack.Init(m.drainItems[i], Mathf.CeilToInt(m.drainBase[i]*(1+3* UnityEngine.Random.value)));
				tempStack.AddToList(marketInv);
				priceTable.AddDrain(m.drainItems[i]);
				tempStack = ItemStack.CreateInstance<ItemStack>();
				drains.Add(tempStack.Init(m.drainItems[i], Mathf.CeilToInt(m.drainBase[i]*m.moduleLevel)));
			}
			
			for (int i = 0; i < m.gainItems.Count; i++)
			{
				ItemStack tempStack = ItemStack.CreateInstance<ItemStack>();
				tempStack.Init(m.gainItems[i], Mathf.CeilToInt(m.gainBase[i]*(1+5* UnityEngine.Random.value)));
				tempStack.AddToList(marketInv);
				priceTable.AddGain(m.gainItems[i]);
				tempStack = ItemStack.CreateInstance<ItemStack>();
				gains.Add(tempStack.Init(m.gainItems[i], Mathf.CeilToInt(m.gainBase[i]*m.moduleLevel)));
			}
		}
	}
	
	public void RefreshStation()
	{
		DateTime newTime = GameControl.instance.gameTime;
		float deltaTime = (float)(newTime.Subtract(lastTime).TotalHours);
		bool needCaravan = false, moduleChanged = false;
		List<ItemStack> neededItems = new List<ItemStack>();
		ItemStack tempStack;
		
		GenerateCrewList();
		
		foreach (Factory f in factories)
		{
			f.Refresh(deltaTime);
		}
		
		//if (deltaTime/24 >= 1)
		{
			foreach (StationModule m in modules)
			{
				moduleChanged = m.Refresh(this) || moduleChanged;
			}
			
			if (moduleChanged)
			{
				foreach (StationModule m in modules)
				{
					drains.Clear();
					gains.Clear();
					for (int i = 0; i < m.drainItems.Count; i++)
					{
						tempStack = ItemStack.CreateInstance<ItemStack>();
						drains.Add(tempStack.Init(m.drainItems[i], Mathf.CeilToInt(m.drainBase[i]*m.moduleLevel)));
					}
					
					for (int i = 0; i < m.gainItems.Count; i++)
					{
						tempStack = ItemStack.CreateInstance<ItemStack>();
						gains.Add(tempStack.Init(m.gainItems[i], Mathf.CeilToInt(m.gainBase[i]*m.moduleLevel)));
					}
				}
			}
			
			for (int i = 0; i < drains.Count; i++)
			{
				if (drains[i].GetItem().GetQuantFromList(marketInv) < drains[i].GetQuantity() * 3)
				{
					tempStack = ItemStack.CreateInstance<ItemStack>();
					neededItems.Add(tempStack.Init(drains[i].GetItem(), drains[i].GetQuantity() * 7));
					needCaravan = true;
				}
			}
			
			foreach (ItemStack stack in marketInv)
			{
				tempStack = ItemStack.CreateInstance<ItemStack>();
				stationMoney += (int)(stack.GetQuantity() * stack.GetItem().GetValue() * .2f);
				stack.AddQuantity((int)(stack.GetQuantity() * -.2f));
			}
			lastTime = GameControl.instance.gameTime;
		}
		
		if (needCaravan)
		{
			Debug.Log(locationName + " needs a caravan");
			MakeCaravans(neededItems);
		}
		
	}
	
	public CrewTable GetCrewTable() {
		return crewPool;
	}
	
	public List<Crew> GetAvailableCrew() {
		return availableCrew;
	}
	
	void GenerateCrewList() {
		availableCrew = new List<Crew>();
		
		for (int i = 0; i < crewPool.GetCrewList().Count; i ++)
		{
			if (availableCrew.Count < 3 && UnityEngine.Random.value < (float)(3 - availableCrew.Count)/(float)(crewPool.GetCrewList().Count - i))
			{
				availableCrew.Add(crewPool.GetCrewAt(i));

			}
		}
	}
	
	void MakeCaravans(List<ItemStack> neededItems) {
		foreach (ItemStack need in neededItems)
		{
			bool hasCaravan = false;
			foreach (CaravanNpc caravan in GameControl.instance.caravans)
			{
				if (caravan.startStation == this)
					foreach (ItemStack alreadySent in caravan.buying)
					{
						if (need.GetItem() == alreadySent.GetItem())
							hasCaravan = true;
					}
			}
			if (hasCaravan)
				continue;
			Station caravanDestination = null;
			foreach (Station s in GameControl.instance.stations)
			{
				int index = s.gains.FindIndex(x => x.GetItem() == need.GetItem());
				if (index != -1)
				{
					if (caravanDestination == null || Vector2.Distance(s.mapPosition, mapPosition) > Vector2.Distance(caravanDestination.mapPosition, mapPosition))
					{
						caravanDestination = s;
					}
				}
			}
			if (caravanDestination != null)
			{
				int caravanMoney = (int)Mathf.Min(need.GetItem().GetValue()*need.GetQuantity()*1.25f, stationMoney);
				List<ItemStack> buyList = new List<ItemStack>();
				buyList.Add(need);
				Mapscreenscript.instance.CreateNewCaravan(faction, caravanMoney, this, caravanDestination, buyList, new List<ItemStack>());
				stationMoney -= caravanMoney;
				
			}
		}
	}
	
}
