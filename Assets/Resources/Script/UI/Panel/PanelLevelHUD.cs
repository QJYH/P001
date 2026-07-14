using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelHUD : Panel
{
	public static PanelLevelHUD Instance { get; private set; }
	private void Awake()
	{
		Instance = this;
	}
	private void OnDestroy()
	{
		Instance = null;
	}

	public static void OnHeroHpChange(float hpRate)
	{
		if (Instance == null) return;

		Transform sliderHp = Instance.transform.Find("TopLeft/ImageAvatar/SliderHp");
		sliderHp.GetComponent<Slider>().value = hpRate;
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

		//todo
	}
}
