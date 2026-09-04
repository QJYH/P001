using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelDirector : SceneDirector
{
	public static LevelDirector LevelInstance { get; private set; }
	public GameObject CurrentHero { get; private set; }
	

	protected override void Awake()
	{
		base.Awake();
		LevelInstance = this;
		AddModule();
		AddHero();
		CreateHUD();
	}

	private void OnEnable()
	{
		OnOpenAnimationFinish.AddListener(OnSceneOpenAnimationFinish);
	}

	private void OnDisable()
	{
		OnOpenAnimationFinish.RemoveListener(OnSceneOpenAnimationFinish);
	}

	private void OnSceneOpenAnimationFinish()
	{
		UICommonFunc.CreatePanel("PanelLevelHUD", null);
	}

	private void AddModule()
	{
		gameObject.AddComponent<TileMapManager>();
		gameObject.AddComponent<ShadowTextureGenerator>();
	}

	private void AddHero()
	{
		GameObject entrance = GameObject.Find("Entrance");
		Vector3 entrancePos = entrance.transform.position;
		CurrentHero = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior/Warrior"));
		CurrentHero.transform.position = entrancePos;
		VirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = CurrentHero.transform;
	}

	protected override Vector2 GetRevealCenter()
	{
		Vector3 heroScreenPos = MainCamera.GetComponent<Camera>().WorldToScreenPoint(CurrentHero.transform.position);
		return new Vector2(heroScreenPos.x, heroScreenPos.y);
	}

	private void CreateHUD()
	{
		
	}	
}
