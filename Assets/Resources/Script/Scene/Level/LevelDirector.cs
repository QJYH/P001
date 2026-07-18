using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : SceneDirector
{
	public static LevelDirector LevelInstance { get; private set; }
	protected GameObject currentHero { get; private set; }
	private GameObject levelHUD;

	protected override void Awake()
	{
		base.Awake();
		LevelInstance = this;
		AddHero();
		SetCameraEdge();
		SetCameraTarget();
		CreateHUD();
	}

	private void AddHero()
	{
		GameObject entrance = GameObject.Find("Entrance1");
		Vector3 entrancePos = entrance.transform.position;
		currentHero = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior/Warrior"));
		currentHero.transform.position = entrancePos;

	}
	private void SetCameraEdge()
	{
		GameObject cameraEdge = GameObject.Find("CameraEdge");
		if (cameraEdge != null)
		{
			CinemachineConfiner2D cinemachineConfiner2D = VirtualCamera.GetComponent<CinemachineConfiner2D>();
			cinemachineConfiner2D.m_BoundingShape2D = cameraEdge.GetComponent<PolygonCollider2D>();
			cinemachineConfiner2D.InvalidateCache();
		}
	}
	private void SetCameraTarget()
	{
		VirtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = currentHero.transform;
	}

	protected override Vector2 GetRevealCenter()
	{
		Vector3 heroScreenPos = MainCamera.GetComponent<Camera>().WorldToScreenPoint(currentHero.transform.position);
		return new Vector2(heroScreenPos.x, heroScreenPos.y);
	}

	private void CreateHUD()
	{
		levelHUD = UICommonFunc.CreatePanel("PanelLevelHUD", null);
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
