using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelDirector : SceneDirector
{
	public static LevelDirector LevelInstance { get; private set; }
	protected GameObject CurrentHero { get; private set; }
	

	protected override void Awake()
	{
		base.Awake();
		LevelInstance = this;
		AddModule();
		AddHero();
		AddBlockRecordRange();
		CreateHUD();
	}

	private void AddModule()
	{
		gameObject.AddComponent<TileMapManager>();
	}

	private void AddHero()
	{
		GameObject entrance = GameObject.Find("Entrance");
		Vector3 entrancePos = entrance.transform.position;
		CurrentHero = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior/Warrior"));
		CurrentHero.transform.position = entrancePos;
		VirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = CurrentHero.transform;
	}

	private void AddBlockRecordRange()
	{
		Instantiate(Resources.Load<GameObject>("Prefab/Module/Range/BlockRecordRange"));
	}

	protected override Vector2 GetRevealCenter()
	{
		Vector3 heroScreenPos = MainCamera.GetComponent<Camera>().WorldToScreenPoint(CurrentHero.transform.position);
		return new Vector2(heroScreenPos.x, heroScreenPos.y);
	}

	private void CreateHUD()
	{
		UICommonFunc.CreatePanel("PanelLevelHUD", null);
	}	
}
