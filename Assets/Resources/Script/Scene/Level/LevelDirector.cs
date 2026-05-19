using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : SceneDirector
{
	protected GameObject currentHero;


	protected override void Awake()
	{
		base.Awake();
		AddHero();
		SetCameraTarget();
	}

	

	protected override void Start()
	{
		base.Start();
	}

	private void AddHero()
	{
		GameObject entrance = GameObject.Find("Entrance1");
		Vector3 entrancePos = entrance.transform.position;
		currentHero = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior"));
		currentHero.transform.position = entrancePos;

	}

	private void SetCameraTarget()
	{
		mainCamera.GetComponent<CameraScript>().SetTarget(currentHero, CameraScript.TargetType.Hero);
	}

	protected override Vector2 GetRevealCenter()
	{
		Vector3 heroScreenPos = mainCamera.GetComponent<Camera>().WorldToScreenPoint(currentHero.transform.position);
		return new Vector2(heroScreenPos.x, heroScreenPos.y);
	}
}
