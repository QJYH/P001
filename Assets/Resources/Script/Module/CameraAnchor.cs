using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnchor : MonoBehaviour
{
	private HeroTriggerRange heroTriggerRange;

	private void Awake()
	{
		heroTriggerRange = GetComponentInChildren<HeroTriggerRange>();
	}

	private void OnEnable()
	{
		heroTriggerRange.OnHeroEnter.AddListener(OnHeroEnter);
		heroTriggerRange.OnHeroExit.AddListener(OnHeroExit);
	}

	private void OnDisable()
	{
		heroTriggerRange.OnHeroEnter.RemoveListener(OnHeroEnter);
		heroTriggerRange.OnHeroExit.RemoveListener(OnHeroExit);
	}

	private void OnHeroExit()
	{
		GameObject hero = LevelDirector.LevelInstance.CurrentHero;
		SceneDirector.SceneInstance.VirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = hero.transform;
	}

	private void OnHeroEnter()
	{
		SceneDirector.SceneInstance.VirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = transform;

	}
}
