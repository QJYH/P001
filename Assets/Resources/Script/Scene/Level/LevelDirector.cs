using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LevelDirector : SceneDirector
{
	public static LevelDirector LevelInstance { get; private set; }
	public GameObject CurrentHero { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		LevelInstance = this;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		LevelInstance = (LevelInstance == this) ? null : LevelInstance;
	}

	private void OnEnable()
	{
		SwitchSceneAnimation.Instance.OnOpenAnimationFinish.AddListener(OnSceneOpenAnimationFinish);
	}

	private void OnDisable()
	{
		//SwitchSceneAnimation.Instance.OnOpenAnimationFinish.RemoveListener(OnSceneOpenAnimationFinish);
	}

	protected override void Start()
	{
		base.Start();
		AddDustParticle();
		ShowHero();
	}

	private void ShowHero()
	{
		CommonFunc.FadeSprite(CurrentHero.GetComponent<SpriteRenderer>(), 0, 1, 1, null);
	}

	private void AddDustParticle()
	{
		GameObject dust = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Particle/Dust/Dust"));
		dust.transform.position = CurrentHero.transform.position;
	}

	private void OnSceneOpenAnimationFinish()
	{
		UICommonFunc.CreatePanel("PanelLevelHUD", null);
		CurrentHero.GetComponent<Hero>().InputResponse = true;
	}

	protected override void AddModule()
	{
		base.AddModule();
		AddHero();
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
		CurrentHero.GetComponent<Hero>().InputResponse = false;
	}

	protected override Vector2 GetOpenAnimationCenter()
	{
		Vector3 heroScreenPos = MainCamera.GetComponent<Camera>().WorldToScreenPoint(CurrentHero.transform.position);
		return new Vector2(heroScreenPos.x, heroScreenPos.y);
	}

	protected override Vector2 GetCloseAnimationCenter()
	{
		Vector3 heroScreenPos = MainCamera.GetComponent<Camera>().WorldToScreenPoint(CurrentHero.transform.position);
		return new Vector2(heroScreenPos.x, heroScreenPos.y);
	}
}
