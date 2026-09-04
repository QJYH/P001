using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		transform.localScale = new Vector3(1.2f, 1.2f, 1);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		transform.localScale = new Vector3(1, 1, 1);
	}
}
