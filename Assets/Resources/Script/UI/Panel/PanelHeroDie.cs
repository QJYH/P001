using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelHeroDie : Panel
{
	public static PanelHeroDie Instance { get; private set; }

	private Texture2D screenshot;
	private const int maxBlurTimes = 8;
	private const float blurDurationPerTimes = 0.2f;
	private GameObject buttonRestart;
	private GameObject buttonQuit;

	private void Awake()
	{
		Instance = this;

		buttonRestart = transform.Find("ImageBackground/ButtonRestart").gameObject;
		buttonQuit = transform.Find("ImageBackground/ButtonQuit").gameObject;
	}
	private void OnDestroy()
	{
		Instance = (Instance == this) ? null : Instance;
	}

	private void OnEnable()
	{
		buttonRestart.GetComponent<Button>().onClick.AddListener(OnButtonRestartClick);
		buttonQuit.GetComponent<Button>().onClick.AddListener(OnButtonQuitClick);
	}

	private void OnDisable()
	{
		buttonRestart.GetComponent<Button>().onClick.RemoveListener(OnButtonRestartClick);
		buttonQuit.GetComponent<Button>().onClick.RemoveListener(OnButtonQuitClick);
	}

	private void Start()
	{
		StartCoroutine(CaptureScreen());
	}

	private IEnumerator CaptureScreen()
	{
		yield return new WaitForEndOfFrame();

		screenshot = ScreenCapture.CaptureScreenshotAsTexture();
		screenshot.Apply();

		transform.Find("ImageBlur").gameObject.SetActive(true);
		SetBlurImage();

		for (int times = 1; times <= maxBlurTimes; times++)
		{
			StartCoroutine(BlurScreenshot(times));
		}
	}

	private void SetBlurImage()
	{
		Rect rect = new Rect(0, 0, screenshot.width, screenshot.height);
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		Sprite blurSprite = Sprite.Create(screenshot, rect, pivot);
		transform.Find("ImageBlur").GetComponent<Image>().sprite = blurSprite;

	}

	private IEnumerator BlurScreenshot(int times)
	{
		yield return new WaitForSeconds(times * blurDurationPerTimes);

		int textureWidth = (int)screenshot.width/2;
		int textureHeigh = (int)screenshot.height/2;
		Texture2D blurScreen = new Texture2D(textureWidth, textureHeigh);
		for (int x = 0; x < textureWidth; x++)
		{
			for (int y = 0; y < textureHeigh; y++)
			{
				Color targetPixel1 = screenshot.GetPixel(2 * x, 2 * y);
				Color targetPixel2 = screenshot.GetPixel(2 * x + 1, 2 * y);
				Color targetPixel3 = screenshot.GetPixel(2 * x, 2 * y + 1);
				Color targetPixel4 = screenshot.GetPixel(2 * x + 1, 2 * y + 1);
				Color targetPixel = (targetPixel1 * 1 / 4) + (targetPixel2 * 1 / 4) + (targetPixel3 * 1 / 4) + (targetPixel4 * 1 / 4);
				blurScreen.SetPixel(x, y, targetPixel);
			}
		}
		screenshot = blurScreen;
		screenshot.Apply();
		SetBlurImage();

		if (times == maxBlurTimes)
		{
			transform.Find("ImageMask").gameObject.SetActive(true);
			transform.Find("ImageBackground").gameObject.SetActive(true);
			ShowPanelTween();
		}
	}

	private void ShowPanelTween()
	{
		RectTransform imageBackground = transform.Find("ImageBackground").GetComponent<RectTransform>();
		DOTween.Sequence()
		.Append(UICommonFunc.MoveY(imageBackground, -100, 0, 1))
		.Join(UICommonFunc.FadeImage(imageBackground.GetComponent<Image>(), 0, 1, 1, null));

		DOTween.Sequence()
		.Append(UICommonFunc.FadeText(transform.Find("ImageBackground/ButtonRestart").GetComponentInChildren<TextMeshProUGUI>(), 0, 1, 1, null))
		.Join(UICommonFunc.FadeText(transform.Find("ImageBackground/ButtonQuit").GetComponentInChildren<TextMeshProUGUI>(), 0, 1, 1, null));
	}

	private void OnButtonRestartClick()
	{
		SceneDirector.SceneInstance.SwitchScene(SceneManager.GetActiveScene().name);
	}

	private void OnButtonQuitClick()
	{
		Application.Quit();
	}
}
