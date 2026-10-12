using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelBegin : Panel
{
	public static PanelBegin Instance { get; private set; }

	private GameObject buttonList;
	private GameObject buttonNewGame;
	private GameObject buttonQuitGame;
	private Sequence showButtonSequence;

	private void Awake()
	{
		Instance = this;

		buttonList = transform.Find("MiddleLeft/ButtonList").gameObject;
		buttonNewGame = transform.Find("MiddleLeft/ButtonList/ButtonNewGame").gameObject;
		buttonQuitGame = transform.Find("MiddleLeft/ButtonList/ButtonQuitGame").gameObject;
	}
	private void OnDestroy()
	{
		Instance = (Instance == this) ? null : Instance;

		showButtonSequence?.Kill();
	}

	private void OnEnable()
	{
		buttonNewGame.GetComponent<Button>().onClick.AddListener(OnButtonNewGameClick);
		buttonQuitGame.GetComponent<Button>().onClick.AddListener(OnButtonQuitGameClick);
	}

	private void OnDisable()
	{
		buttonNewGame.GetComponent<Button>().onClick.RemoveListener(OnButtonNewGameClick);
		buttonQuitGame.GetComponent<Button>().onClick.RemoveListener(OnButtonQuitGameClick);
	}

	private void Start()
	{
		ShowButton();
	}

	private void OnButtonNewGameClick()
	{
		SceneDirector.SceneInstance.SwitchScene("SceneLevel1");
	}

	private void OnButtonQuitGameClick()
	{
		Application.Quit();
	}

	private void ShowButton()
	{
		RectTransform buttonListRect = buttonList.GetComponent<RectTransform>();
		Sequence sequence1 = DOTween.Sequence()
		.Append(UICommonFunc.MoveY(buttonListRect, -100, 0, 1));

		Sequence sequence2 = DOTween.Sequence()
		.Append(UICommonFunc.FadeText(buttonNewGame.GetComponentInChildren<TextMeshProUGUI>(), 0, 1, 0.7f, null));

		Sequence sequence3 = DOTween.Sequence()
		.AppendInterval(0.3f)
		.Append(UICommonFunc.FadeText(buttonQuitGame.GetComponentInChildren<TextMeshProUGUI>(), 0, 1, 0.7f, null));

		showButtonSequence = DOTween.Sequence();
		showButtonSequence.Append(sequence1).Join(sequence2).Join(sequence3);
	}
}
