using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType {Caravan, Military, Bandit}
public enum NpcFaction {Fac1, Fac2}
public enum MoveState {Waiting, Moving}

public class Npc : ScriptableObject {

	public MoveState moveState = MoveState.Waiting;
	public string npcName = "Name", npcDesc = "Description";
	public NpcType npcType = NpcType.Caravan;
	public NpcFaction faction = (NpcFaction)0;
	public Ship npcShip;
	public List<ItemStack> cargo = new List<ItemStack>();
	public int npcMoney, waitTimer = 1440;
	public Vector2 currentCoordinates;
	public Location destination, currentLocation;
	
	public virtual void MovementAI() {
	}
	
	public virtual void FindNextDestination() {
	}
	
	
	
}
