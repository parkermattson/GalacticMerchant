using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Location", menuName = "Location")]
public class Location : ScriptableObject {

    public string locationName = "Name";
    public string locationDesc = "Location Description";
    public Sprite icon = null;
    public int locationIndustry = 0;
    public int locationRace = 0;
    public Vector2 mapPosition;

    public string GetName()
    {
        return locationName;
    }

    public string GetDescription()
    {
        return locationDesc;
    }

    public int GetIndustry()
    {
        return locationIndustry;
    }

    public Vector2 GetMapPos()
    {
        return mapPosition;
    }

    public int GetRace()
    {
        return locationRace;
    }
}
