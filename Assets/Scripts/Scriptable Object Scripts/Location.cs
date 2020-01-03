﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationType {Station, Empty, Anomaly, Transmission, Distress, Conflict, Natural}

[CreateAssetMenu(fileName = "New Location", menuName = "Locations/Location")]
public class Location : ScriptableObject {

    public string locationName = "Name";
    public string locationDesc = "Location Description";
	public LocationType locationType = LocationType.Empty;
    public Sprite icon = null;
    public Vector2 mapPosition;
	
	DateTime encounterStart = new DateTime (3000, 1, 1, 9, 0, 0);

    public string GetName()
    {
        return locationName;
    }

    public string GetDescription()
    {
        return locationDesc;
    }

    public Vector2 GetMapPos()
    {
        return mapPosition;
    }
	
	public void RefreshType(){
		if (locationType == LocationType.Empty)
		{
			int locType = (int)(UnityEngine.Random.value * 100) + 1;
				if (locType > 6)
					locType = 1;
				
			locationType = (LocationType)locType;
			encounterStart = GameControl.instance.gameTime;
		}
		else if (GameControl.instance.gameTime > encounterStart.AddHours(24))
		{
			locationType = LocationType.Empty;
		}
		
	}
	
}
