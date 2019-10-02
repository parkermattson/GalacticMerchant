using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationType {Station, Empty, Planet, Nebula, AsteroidField, Outpost}

[CreateAssetMenu(fileName = "New Location", menuName = "Locations/Location")]
public class Location : ScriptableObject {

    public string locationName = "Name";
    public string locationDesc = "Location Description";
	public LocationType locationType = LocationType.Empty;
    public Sprite icon = null;

    public Vector2 mapPosition;

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
}
