using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
	private void OnEnable()
	{
		GetComponentInChildren<HeroTriggerRange>().OnHeroEnter.AddListener(OnHeroEnter);
	}

	private void OnDisable()
	{
		GetComponentInChildren<HeroTriggerRange>().OnHeroEnter.RemoveListener(OnHeroEnter);
	}

	private void OnHeroEnter()
	{
		GameObject heroObj = LevelDirector.LevelInstance.CurrentHero;
		Hero hero = heroObj.GetComponent<Hero>();
		hero.InputResponse = false;
		hero.AutoRunTo(transform.position, 1);
		CommonFunc.FadeSprite(hero.GetComponent<SpriteRenderer>(), 1, 0, 1, null);
		DOVirtual.DelayedCall(1, () => { UICommonFunc.CreatePanel("PanelWin", null); });
		
	}
}
