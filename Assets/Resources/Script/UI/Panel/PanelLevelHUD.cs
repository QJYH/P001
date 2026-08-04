using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelHUD : Panel
{
	public static PanelLevelHUD Instance { get; private set; }
	private Tween goldImageScaleTween;
	private Tween goldCountTextUpdateTween;
	private Tween hpSliderUpdateTween;
	private int realGoldCount = 0;
	private int nominalGoldCount = 0;

	private void Awake()
	{
		Instance = this;
	}
	private void OnDestroy()
	{
		Instance = null;

		goldImageScaleTween?.Kill();
		goldCountTextUpdateTween?.Kill();
		hpSliderUpdateTween?.Kill();

	}

	public static void OnHeroHpChange(float hpRate)
	{
		if (Instance == null) return;

		Instance.hpSliderUpdateTween?.Kill();
		Transform sliderHp = Instance.transform.Find("TopLeft/ImageAvatar/SliderHp");
		Instance.hpSliderUpdateTween = DOTween.To(
			delegate ()
			{
				return sliderHp.GetComponent<Slider>().value;
			},
			delegate (float value)
			{
				sliderHp.GetComponent<Slider>().value = value;
			},
			hpRate,
			1f
		);
	}

	public static Vector2 GetGoldIconScreenPos()
	{
		if (Instance == null) return Vector2.zero;

		Transform imageGold = Instance.transform.Find("TopLeft/ImageAvatar/ImageGold");
		return imageGold.position;
	}

	public static void OnPickUpGold(int goldValue)
	{
		if (Instance == null) return;

		Instance.realGoldCount += goldValue;

		Instance.goldImageScaleTween?.Kill();
		Transform imageGold = Instance.transform.Find("TopLeft/ImageAvatar/ImageGold");
		imageGold.localScale = new Vector3(1, 1, 1);
		Instance.goldImageScaleTween = imageGold.DOScale(1.5f, 0.5f).SetLoops(2, LoopType.Yoyo);

		Instance.goldCountTextUpdateTween?.Kill();
		TextMeshProUGUI textGoldCount = imageGold.Find("TextGoldCount").GetComponent<TextMeshProUGUI>();
		Instance.goldCountTextUpdateTween = DOTween.To(
			delegate ()
			{
				return Instance.nominalGoldCount;
			},
			delegate (int value)
			{
				Instance.nominalGoldCount = value;
				textGoldCount.text = Instance.nominalGoldCount.ToString();
			},
			Instance.realGoldCount,
			1f
		);
	}
}
