using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelWin : Panel
{
	public static PanelWin Instance { get; private set; }
	private GameObject buttonBack;

	private void Awake()
	{
		Instance = this;

		buttonBack = transform.Find("ButtonBack").gameObject;	
	}
	private void OnDestroy()
	{
		Instance = (Instance == this) ? null : Instance;
	}

	private void OnEnable()
	{
		buttonBack.GetComponent<Button>().onClick.AddListener(OnButtonBackClick);
	}

	private void OnDisable()
	{
		buttonBack.GetComponent<Button>().onClick.RemoveListener(OnButtonBackClick);
	}

	private void Start()
	{
		ShowPanel();
	}

	private void ShowPanel()
	{
		transform.Find("ImageRibbon").localScale = new Vector3(0, 1, 1);
		DOTween.Sequence()
		.Append(transform.Find("ImageRibbon").DOScaleX(1, 1).SetEase(Ease.OutBack));

		RectTransform buttonBack = transform.Find("ButtonBack").GetComponent<RectTransform>();
		DOTween.Sequence()
		.AppendInterval(1)
		.Append(UICommonFunc.MoveY(buttonBack, 100, 0, 1))
		.Join(UICommonFunc.FadeText(buttonBack.GetComponentInChildren<TextMeshProUGUI>(), 0, 1, 1, null));
	}

	private void OnButtonBackClick()
	{
		SceneDirector.SceneInstance.SwitchScene("SceneBegin");
	}
}
