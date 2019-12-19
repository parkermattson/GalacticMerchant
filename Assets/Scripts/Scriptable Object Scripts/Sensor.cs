using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sensor", menuName = "Inventory/Sensor")]
public class Sensor : Equipment {

	public Sensor() {
		equipSlot = EquipmentSlot.Sensors;
	}
	
	public int sensorLevel = 1;
	public int sensorRange = 300;
	
	public int GetSensorLevel() {
		return sensorLevel;
	}
	
	public int GetSensorRange() {
		return sensorRange;
	}
	
}
