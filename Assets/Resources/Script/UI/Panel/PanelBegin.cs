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
		Destroy(gameObject);
		SceneDirector.SceneInstance.SwitchScene("SceneLevel1");
	}

	private void OnButtonQuitGameClick()
	{
		Application.Quit();
	}

	private void ShowButton()
	{
		buttonNewGame.GetComponentInChildren<TextMeshProUGUI>().alpha = 0;
		buttonQuitGame.GetComponentInChildren<TextMeshProUGUI>().alpha = 0;
		float originalPosY = buttonList.GetComponent<RectTransform>().anchoredPosition.y;

		Sequence subSequence = DOTween.Sequence();
		subSequence.AppendInterval(0.5f).Append(SetBtnTextAlphaTween(buttonQuitGame));
		showButtonSequence = DOTween.Sequence();
		showButtonSequence.Append(buttonList.transform.DOLocalMoveY(originalPosY - 100, 0))
		.Append(buttonList.transform.DOLocalMoveY(originalPosY, 1.5f))
		.Join(SetBtnTextAlphaTween(buttonNewGame))
		.Join(subSequence);
	}

	

	private Tween SetBtnTextAlphaTween(GameObject button)
	{
		TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();

		return DOTween.To(
			delegate ()
			{
				return text.alpha;
			},
			delegate (float value)
			{
				text.alpha = value;
			},
			1f,
			1f
		);
	}
}
