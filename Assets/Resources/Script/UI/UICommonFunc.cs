using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UICommonFunc
{
    public static void StretchToFillParent(this RectTransform rect, Vector2? offsetMin, Vector2? offsetMax)
	{
		rect.anchorMin = Vector2.zero;
		rect.anchorMax = Vector2.one;
		rect.offsetMin = (offsetMin != null) ? (Vector2)offsetMin : Vector2.zero;
		rect.offsetMax = (offsetMax != null) ? (Vector2)offsetMax : Vector2.zero;
	}
}
