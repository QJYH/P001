using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PanelHeroDie : MonoBehaviour
{
	private Texture2D screenshot;
	private readonly int maxBlurTimes = 8;
	private readonly float blurDurationPerTimes = 0.2f;

	private void Awake()
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
		}
	}
}
