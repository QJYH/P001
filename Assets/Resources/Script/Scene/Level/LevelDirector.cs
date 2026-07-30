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
		SetCameraEdge();
		SetCameraTarget();
		CreateHUD();
	}

	private void AddModule()
	{
		gameObject.AddComponent<TileMapManager>();
	}

	private void AddHero()
	{
		GameObject entrance = GameObject.Find("Entrance1");
		Vector3 entrancePos = entrance.transform.position;
		CurrentHero = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior/Warrior"));
		CurrentHero.transform.position = entrancePos;

	}
	private void SetCameraEdge()
	{
		GameObject cameraEdge = GameObject.Find("CameraArea");
		if (cameraEdge != null)
		{
			CinemachineConfiner2D cinemachineConfiner2D = VirtualCamera.GetComponent<CinemachineConfiner2D>();
			cinemachineConfiner2D.m_BoundingShape2D = cameraEdge.GetComponent<PolygonCollider2D>();
			cinemachineConfiner2D.InvalidateCache();
		}
	}
	private void SetCameraTarget()
	{
		VirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = CurrentHero.transform;
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

	public static void OnHeroDie()
	{
		if (LevelInstance == null) return;

		LevelInstance.StartCoroutine(LevelInstance.DelayCreatePanelHeroDie());
	}

	private IEnumerator DelayCreatePanelHeroDie()
	{
		yield return new WaitForSeconds(1.1f);
		UICommonFunc.CreatePanel("PanelHeroDie", null);
	}
}
