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
		transform.Find("VirtualCamera").gameObject.SetActive(false);
	}

	private void OnHeroEnter()
	{
		transform.Find("VirtualCamera").gameObject.SetActive(true);
	}
}
