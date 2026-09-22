using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneDirector : MonoBehaviour
{
	public static SceneDirector SceneInstance { get; private set; }
	public GameObject MainCanvas { get; private set; }
	public GameObject MainEventSystem { get; private set; }
	public GameObject MainCamera { get; private set; }
	public GameObject VirtualCamera { get; private set; }

	protected virtual void Awake()
	{
		SceneInstance = this;
		
		InitCamera();
		InitCanvas();
		AddModule();

		DOVirtual.DelayedCall(0.1f, () => { SceneStart(); });
	}

	protected virtual void OnDestroy()
	{
		SceneInstance = (SceneInstance == this) ? null : SceneInstance;
	}

	protected virtual void SceneStart()
	{
		SwitchSceneAnimation.Instance.PlayOpenAnimation(GetOpenAnimationCenter());
	}

	protected virtual void AddModule()
	{
		AddSwitchSceneAnimation();
	}

	private void AddSwitchSceneAnimation()
	{
		Instantiate(Resources.Load<GameObject>("Prefab/Module/SwitchSceneAnimation"));
	}

	private void InitCamera()
	{
		MainCamera = GameObject.Find("MainCamera");
		VirtualCamera = Instantiate(Resources.Load<GameObject>("Prefab/Module/Camera/VirtualCamera"));
		GameObject cameraEdge = GameObject.Find("CameraArea");
		if (cameraEdge != null)
		{
			CinemachineConfiner2D cinemachineConfiner2D = VirtualCamera.GetComponent<CinemachineConfiner2D>();
			cinemachineConfiner2D.m_BoundingShape2D = cameraEdge.GetComponent<PolygonCollider2D>();
			cinemachineConfiner2D.InvalidateCache();
		}
	}

	private void InitCanvas()
	{
		MainCanvas = new GameObject("MainCanvas");
		MainCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		MainCanvas.AddComponent<CanvasScaler>();
		MainCanvas.AddComponent<GraphicRaycaster>();

		MainEventSystem = new GameObject("EventSystem");
		MainEventSystem.AddComponent<EventSystem>();
		MainEventSystem.AddComponent<InputSystemUIInputModule>();
	}

	public void SwitchScene(string newSceneName)
	{
		ReleaseAllPanel();
		SwitchSceneAnimation.Instance.PlayCloseAnimation(GetCloseAnimationCenter());
		SwitchSceneAnimation.Instance.OnCloseAnimationFinish.AddListener(() => { SceneManager.LoadScene(newSceneName); });
	}

	private void ReleaseAllPanel()
	{
		foreach (Transform transform in MainCanvas.transform)
		{
			Destroy(transform.gameObject);
		}
	}

	protected virtual Vector2 GetOpenAnimationCenter()
	{
		return new Vector2(Screen.width / 2, Screen.height / 2);
	}

	protected virtual Vector2 GetCloseAnimationCenter()
	{
		return new Vector2(Screen.width / 2, Screen.height / 2);
	}

}
