using Cinemachine;
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
		SetCameraEdge();
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
		currentHero = Instantiate(Resources.Load<GameObject>("Prefab/Sprite/Creature/Hero/Warrior/Warrior"));
		currentHero.transform.position = entrancePos;

	}
	private void SetCameraEdge()
	{
		GameObject cameraEdge = GameObject.Find("CameraEdge");
		if (cameraEdge != null)
		{
			CinemachineConfiner2D cinemachineConfiner2D = virtualCamera.GetComponent<CinemachineConfiner2D>();
			cinemachineConfiner2D.m_BoundingShape2D = cameraEdge.GetComponent<PolygonCollider2D>();
			cinemachineConfiner2D.InvalidateCache();
		}
	}
	private void SetCameraTarget()
	{
		virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = currentHero.transform;
		virtualCamera.GetComponent<CinemachineCameraOffset>().m_Offset = new Vector3(0, -0.2f, 0);
	}

	protected override Vector2 GetRevealCenter()
	{
		Vector3 heroScreenPos = mainCamera.GetComponent<Camera>().WorldToScreenPoint(currentHero.transform.position);
		return new Vector2(heroScreenPos.x, heroScreenPos.y);
	}
}
