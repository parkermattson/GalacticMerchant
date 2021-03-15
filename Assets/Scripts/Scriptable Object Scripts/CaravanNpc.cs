using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaravanNpc : Npc {
	/*
	List<Station> route = new List<Station>();
	public List<int> routeStationNums;
	int routePosition = 0;
	
	public void Awake()
	{
		foreach (int i in routeStationNums)
		{
			route.Add(GameControl.instance.stations[i]);
		}
		currentLocation = route[0];
		currentCoordinates = currentLocation.mapPosition;
	}*/
	
	public Station startStation, endStation;
	public List<ItemStack> buying = new List<ItemStack>(), selling = new List<ItemStack>();
	bool reachedEnd = false;
	
	public CaravanNpc Init(NpcFaction newNpcFaction, int newMoney, Ship newNpcShip, Station newStartStation, Station newEndStation, List<ItemStack> buyList, List<ItemStack> sellList) {
		//Generate Name Here
		npcType = NpcType.Caravan;
		faction = newNpcFaction;
		npcShip = newNpcShip;
		npcMoney = newMoney;
		startStation = newStartStation;
		endStation = newEndStation;
		buying = buyList;
		selling = sellList;
		
		currentCoordinates = startStation.mapPosition;
		currentLocation = startStation;
		return this;
	}
	
	public override bool MovementAI() {
		if (moveState == MoveState.Waiting)
		{
			if (waitTimer > 0)
			{
				waitTimer-=3;
			}
			else 
			{
				FindNextDestination();
				moveState = MoveState.Moving;
			}
		}
		else if (moveState == MoveState.Moving)
		{
			if (Vector2.Distance(currentCoordinates, destination.mapPosition) < Mapscreenscript.BASESPEED * npcShip.GetNetSpeed(false))
			{
				currentCoordinates = destination.mapPosition;
				currentLocation = destination;
				if (currentLocation == endStation)
				{
					TradeAtStation(endStation);
					reachedEnd = true;
					waitTimer = 1440;
				} else if (currentLocation == startStation && reachedEnd == true)
				{
					foreach (ItemStack stack in cargo)
					{
						stack.AddToList(startStation.marketInv);
					}
					startStation.stationMoney += npcMoney;
					return true;
				} else waitTimer = 60;
				moveState = MoveState.Waiting;
			}
			else
			{
				currentCoordinates = Vector2.MoveTowards(currentCoordinates, destination.mapPosition, Mapscreenscript.BASESPEED * npcShip.GetNetSpeed(false));
			}
		}
		return false;
		
	}
	
	public override void FindNextDestination() {
		if (!reachedEnd)
			destination = GameControl.instance.FindShortestPath(currentLocation, (Location)endStation, npcShip.GetNetWarpRange(false))[1];
		else destination = GameControl.instance.FindShortestPath(currentLocation, (Location)startStation, npcShip.GetNetWarpRange(false))[1];
	}
	
	void TradeAtStation(Station station){
		foreach (ItemStack buy in buying) 
		{
			int index = station.marketInv.FindIndex(x => x.GetItem() == buy.GetItem());
			if (index != -1 && station.marketInv[index].GetPrice(-1) < buy.GetItem().GetValue())
			{
				int buyQuant = Mathf.Min(buy.GetQuantity(), station.marketInv[index].GetQuantity());
				float addQuant = buyQuant/2;
				while ((station.marketInv[index].GetPrice(-buyQuant) < buy.GetItem().GetValue()*.95 && station.marketInv[index].GetPrice(-buyQuant) > buy.GetItem().GetValue()) || (station.marketInv[index].GetPrice(-buyQuant)*buyQuant < npcMoney && station.marketInv[index].GetPrice(-buyQuant)*buyQuant > npcMoney*.9))
				{
					if (station.marketInv[index].GetPrice(-buyQuant) > buy.GetItem().GetValue() || station.marketInv[index].GetPrice(-buyQuant)*buyQuant > npcMoney)
					{
						buyQuant=(int)(buyQuant-addQuant);
					} else if (station.marketInv[index].GetPrice(-buyQuant) < buy.GetItem().GetValue()*.95)
					{
						buyQuant=(int)(buyQuant+addQuant);
					}
					addQuant/=2;
				}
				int price = Mathf.CeilToInt(station.marketInv[index].GetPrice(-buyQuant));
				buy.quantity = buyQuant;
				buy.RemoveFromList(station.marketInv);
				buy.AddToList(cargo);
				station.stationMoney += price;
				npcMoney -= price;
				
				Debug.Log("Caravan traded " + buy.GetQuantity() + " " + buy.GetItem().GetName() + " at " + station.GetName());
			}
		}
	}
	
}
