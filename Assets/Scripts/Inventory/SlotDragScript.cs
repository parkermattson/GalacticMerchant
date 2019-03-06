using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotDragScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	
	public static GameObject slotBox,  slot;
	public  GameObject newParent;
	Transform oldParent;
	
	Vector3 startPosition;

	public void OnBeginDrag (PointerEventData eventData)
	{
		slotBox = gameObject;
		slot = slotBox.transform.Find("Equip Slot").gameObject;
		oldParent = slot.transform.parent;
		slot.transform.SetParent(newParent.transform);
		startPosition = slot.transform.position;
		slot.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		slot.transform.position = Input.mousePosition;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		slot.transform.SetParent(oldParent);
		slot.transform.position = startPosition;
		slot.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
	
	
}
