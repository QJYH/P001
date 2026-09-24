using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public static class UICommonFunc
{
    public static void StretchToFillParent(this RectTransform rect, Vector2? offsetMin, Vector2? offsetMax)
	{
		rect.anchorMin = Vector2.zero;
		rect.anchorMax = Vector2.one;
		rect.offsetMin = (offsetMin != null) ? (Vector2)offsetMin : Vector2.zero;
		rect.offsetMax = (offsetMax != null) ? (Vector2)offsetMax : Vector2.zero;
	}

	public static GameObject CreatePanel(string panelName, Transform parent)
	{
		string panelDir = string.Format("Prefab/UI/Panel/{0}/{1}", panelName, panelName);
		Transform parentTransform = (parent != null) ? parent : SceneDirector.SceneInstance.MainCanvas.transform;
		GameObject panel = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(panelDir));
		panel.transform.SetParent(parentTransform, false);

		return panel;
	}

	public static Tween FadeImage(Image image, float beginAlpha, float endAlpha, float duration, Action callBackFunc)
	{
		Color color = image.color;
		image.color = new Color(color.r, color.g, color.b, beginAlpha);
		return DOTween.To(
			delegate ()
			{
				return image.color.a;
			},
			delegate (float value)
			{
				Color color = image.color;
				image.color = new Color(color.r, color.g, color.b, value);
			},
			endAlpha,
			duration
		).OnComplete(() => {
			callBackFunc?.Invoke();
		});
	}

	public static Tween FadeText(TextMeshProUGUI text, float beginAlpha, float endAlpha, float duration, Action callBackFunc)
	{
		text.alpha = beginAlpha;
		return DOTween.To(
			delegate ()
			{
				return text.alpha;
			},
			delegate (float value)
			{
				text.alpha = value;
			},
			endAlpha,
			duration
		).OnComplete(() => {
			callBackFunc?.Invoke();
		});
	}

	public static Tween Move(RectTransform rectTransform, Vector2 beginOffset, Vector2 endOffset, float duration)
	{
		Vector2 originalPosition = rectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(originalPosition.x + beginOffset.x, originalPosition.y + beginOffset.y);
		return rectTransform.DOLocalMove(new Vector2(originalPosition.x + endOffset.x, originalPosition.y + endOffset.y), duration);
	}

	public static Tween MoveX(RectTransform rectTransform, float beginOffset, float endOffset, float duration)
	{
		return Move(rectTransform, new Vector2(beginOffset, 0), new Vector2(endOffset, 0), duration);
	}

	public static Tween MoveY(RectTransform rectTransform, float beginOffset, float endOffset, float duration)
	{
		return Move(rectTransform, new Vector2(0, beginOffset), new Vector2(0, endOffset), duration);
	}
}
