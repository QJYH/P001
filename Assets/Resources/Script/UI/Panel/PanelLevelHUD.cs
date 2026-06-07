using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelHUD : MonoBehaviour
{
	internal void OnHeroHpChange(float hpRate)
	{
		Transform sliderHp = transform.Find("TopLeft/ImageAvatar/SliderHp");
		sliderHp.GetComponent<Slider>().value = hpRate;
	}
}
