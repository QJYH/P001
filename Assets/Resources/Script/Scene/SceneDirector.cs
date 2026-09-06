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
	protected UnityEvent OnOpenAnimationFinish = new UnityEvent();
	protected UnityEvent OnCloseAnimationFinish = new UnityEvent();

	protected virtual void Awake()
	{
		SceneInstance = this;
		Application.targetFrameRate = 60;
		InitCamera();
		InitCanvas();
		AddModule();
		AddHero();
		PlayOpenAnimation();
	}

	protected virtual void OnDestroy()
	{
		SceneInstance = (SceneInstance == this) ? null : SceneInstance;
	}

	protected virtual void AddModule()
	{
		
	}

	protected virtual void AddHero()
	{
		
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

	private void PlayOpenAnimation()
	{
		GameObject maskObject = Instantiate(Resources.Load<GameObject>("Prefab/Module/CircleRevealMask"));
		maskObject.transform.position = MainCamera.transform.position;
		Vector2 centerScreenPos = GetOpenAnimationRevealCenter();
		CircleRevealMask mask = maskObject.GetComponent<CircleRevealMask>();
		mask.Expand(centerScreenPos);

		Sequence sequence = DOTween.Sequence();
		sequence.AppendInterval(mask.Duration);
		sequence.AppendCallback(() => { OnOpenAnimationFinish.Invoke(); });
	}

	protected virtual Vector2 GetOpenAnimationRevealCenter()
	{
		return new Vector2(Screen.width / 2, Screen.height / 2);
	}

	public void SwitchScene(string newSceneName)
	{
		PlayCloseAnimation();
		OnCloseAnimationFinish.AddListener(() => { SceneManager.LoadScene(newSceneName); });
	}

	private void PlayCloseAnimation()
	{
		GameObject maskObject = Instantiate(Resources.Load<GameObject>("Prefab/Module/CircleRevealMask"));
		maskObject.transform.position = MainCamera.transform.position;
		Vector2 centerScreenPos = GetCloseAnimationRevealCenter();
		CircleRevealMask mask = maskObject.GetComponent<CircleRevealMask>();
		mask.Contract(centerScreenPos);

		Sequence sequence = DOTween.Sequence();
		sequence.AppendInterval(mask.Duration);
		sequence.AppendCallback(() => { OnCloseAnimationFinish.Invoke(); });
	}

	private Vector2 GetCloseAnimationRevealCenter()
	{
		return new Vector2(Screen.width / 2, Screen.height / 2);
	}

}
