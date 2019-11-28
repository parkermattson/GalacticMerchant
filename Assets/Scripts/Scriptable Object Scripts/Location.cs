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
	
	public void RollEncounter()
	{
		float rng = Random.value;
		
		switch (locationType)
		{
			case LocationType.Station:
				if (rng < .1f)
				{
					Debug.Log("Random Encounter Trigger");
				}
				break;
			case LocationType.Empty:
				if (rng < .1f)
				{
					Debug.Log("Random Encounter Trigger");
				}
				break;
			case LocationType.Anomaly: 
				if (rng < .1f)
				{
					Debug.Log("Random Encounter Trigger");
				}
				break;
			case LocationType.Transmission: 
				if (rng < .1f)
				{
					Debug.Log("Random Encounter Trigger");
				}
				break;
			case LocationType.Distress: 
				if (rng < .1f)
				{
					Debug.Log("Random Encounter Trigger");
				}
				break;
			case LocationType.Conflict: 
				if (rng < .1f)
				{
					Debug.Log("Random Encounter Trigger");
				}
				break;
			case LocationType.Natural:
				if (rng < .1f)
				{
					Debug.Log("Random Encounter Trigger");
				}
				break;
		}
	}
}
