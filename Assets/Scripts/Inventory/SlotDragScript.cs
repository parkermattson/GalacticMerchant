using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	
	public static GameObject itemBeingDragged;
	public  GameObject newParent;
	Transform oldParent;
	
	Vector3 startPosition;

	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		oldParent = itemBeingDragged.transform.parent;
		itemBeingDragged.transform.SetParent(newParent.transform);
		startPosition = transform.position;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		itemBeingDragged.transform.SetParent(oldParent);
		itemBeingDragged = null;
		transform.position = startPosition;
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
	
	
}
