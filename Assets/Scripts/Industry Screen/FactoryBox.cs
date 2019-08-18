using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class FactoryBox : MonoBehaviour, IPointerClickHandler {

	Factory factory;
	IndustryScreenScript indScript;
	
	public TextMeshProUGUI nameText, descText;
	public Image factoryIcon;

	
	public void SetFactory(Factory newFactory, IndustryScreenScript script)
	{
		factory = newFactory;
		indScript = script;
		nameText.SetText(factory.factoryName);
		descText.SetText(factory.factoryDescription);
		factoryIcon.sprite = factory.factoryIcon;
	}
	
	public Factory GetFactory()
	{
		return factory;
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		indScript.SelectFactory(factory);
	}
	
}
