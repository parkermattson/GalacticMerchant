using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrewSlot : MonoBehaviour {

	public Crew crew;
	public Image avatar;
	public Image race;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI statText1;
	public TextMeshProUGUI statText2;
	public TextMeshProUGUI statText3;
	public TextMeshProUGUI statText4;
	public TextMeshProUGUI salaryText;
	
	public void AddCrew(Crew newCrew)
	{
		GameControl gameController = GameControl.instance;
		int[] stats = new int[4];
		crew = newCrew;
		stats = crew.GetStats();
		avatar.sprite = gameController.avatars[crew.GetAvatar()];
		race.sprite = gameController.races[crew.GetRace()];
		nameText.SetText(crew.GetCrewName());
		statText1.SetText(stats[0].ToString());
		statText2.SetText(stats[1].ToString());
		statText3.SetText(stats[2].ToString());
		statText4.SetText(stats[3].ToString());
		if (salaryText != null)
			salaryText.text = crew.GetPrice().ToString() + " SB";
	}
	
	public void HireCrew()
	{
		Station station = (Station)GameControl.instance.playerLocation;
		GameControl.instance.HireCrew(crew);
		GameControl.instance.playerMoney -= crew.GetPrice();
		station.GetCrewTable().GetCrewList().Remove(crew);
		station.GetAvailableCrew().Remove(crew);
		GetComponentInParent<AcademyScreenScript>().UpdateRecruitmentList();
	}
}
