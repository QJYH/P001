using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelBegin : Panel
{
	public static PanelBegin Instance { get; private set; }

	private GameObject buttonNewGame;
	private GameObject buttonQuitGame;
	private Tween buttonNewGameTextTween;
	private Tween buttonQuitGameTextTween;

	private void Awake()
	{
		Instance = this;

		buttonNewGame = transform.Find("MiddleLeft/ButtonList/ButtonNewGame").gameObject;
		buttonQuitGame = transform.Find("MiddleLeft/ButtonList/ButtonQuitGame").gameObject;
	}
	private void OnDestroy()
	{
		Instance = null;
	}

	private void Start()
	{
		SetBtnTextAlphaTween(buttonNewGame, buttonNewGameTextTween, 0, 1);
		SetBtnTextAlphaTween(buttonQuitGame, buttonQuitGameTextTween, 0, 1);
	}

	private void SetBtnTextAlphaTween(GameObject button, Tween buttonTextTween, float beginAlpha, float endAlpha)
	{
		TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
		text.alpha = beginAlpha;

		buttonTextTween?.Kill();
		buttonTextTween = DOTween.To(
			delegate ()
			{
				return text.alpha;
			},
			delegate (float value)
			{
				text.alpha = value;
			},
			endAlpha,
			2f
		);
	}
}
