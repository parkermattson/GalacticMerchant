using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NpcType {Caravan, Military, Bandit}
public enum NpcFaction {Fac1, Fac2}

public class Npc : ScriptableObject {

	string npcName = "Name", npcDesc = "Description";
	NpcType npcType = NpcType.Caravan;
	NpcFaction faction = NpcFaction.Fac1;
	Ship npcShip;
	List<ItemStack> cargo = new List<ItemStack>();
	int npcMoney;
	Vector2 currentPostion;
	Location destination;
	bool inTransit;
	
}
