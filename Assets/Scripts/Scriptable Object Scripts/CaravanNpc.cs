using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Caravan", menuName = "NPCs/Caravan")]
public class CaravanNpc : Npc {
	
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
	}
	
	public override void MovementAI() {
		if (moveState == MoveState.Waiting)
		{
			if (waitTimer > 0)
			{
				waitTimer-=3;
			}
			else 
			{
				waitTimer = 1440;
				if (currentLocation == route[routePosition])
				{
					TradeAtStation();
					if (routePosition < route.Count)
						routePosition++;
					else routePosition = 0;
				}
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
				if (currentLocation == route[routePosition])
				{
					TradeAtStation();
				}
				moveState = MoveState.Waiting;
			}
			else
			{
				currentCoordinates = Vector2.MoveTowards(currentCoordinates, destination.mapPosition, Mapscreenscript.BASESPEED * npcShip.GetNetSpeed(false));
			}
		}
		
	}
	
	public override void FindNextDestination() {
		destination = GameControl.instance.FindShortestPath(currentLocation, (Location)route[routePosition], npcShip.GetNetWarpRange(false))[1];
	}
	
	void TradeAtStation(){
		Debug.Log(npcName + " is trading at " + currentLocation.GetName());
	}
	
}
