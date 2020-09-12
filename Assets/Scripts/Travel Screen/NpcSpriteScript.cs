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
		if (spriteNpc.MovementAI())
		{
			Mapscreenscript.instance.caravansOnMap.Remove(this);
			GameControl.instance.caravans.Remove((CaravanNpc)spriteNpc);
			Destroy(spriteNpc);
			Destroy(gameObject);
		}
		transform.localPosition = spriteNpc.currentCoordinates;
	}
}
