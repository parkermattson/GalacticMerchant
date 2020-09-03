using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSpriteScript : MonoBehaviour {

	public Npc spriteNpc;
	public Image npcImage;
	
	public void Init(Npc newNpc) {
		spriteNpc = newNpc;
		switch (spriteNpc.faction)
		{
			case NpcFaction.Fac1:
				npcImage.color = Color.green;
				break;
			case NpcFaction.Fac2:
				npcImage.color = Color.gray;
				break;
		}
		transform.localPosition = spriteNpc.currentCoordinates;
	}
	
	public void TakeTurn()
	{
		spriteNpc.MovementAI();
		transform.localPosition = spriteNpc.currentCoordinates;
	}
}
