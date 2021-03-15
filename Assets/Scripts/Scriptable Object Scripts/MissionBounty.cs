using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBounty : Mission {

	public Npc targetNpc;
	int numOfTargets = 1, targetsKilled = 0;
	
	public void ProgObj() {
		targetsKilled++;
		if (targetsKilled >= numOfTargets)
		{
			completed = true;
		}
	}
}
