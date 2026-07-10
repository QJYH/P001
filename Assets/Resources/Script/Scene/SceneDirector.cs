using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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
		Application.targetFrameRate = 60;
		InitCamera();
		InitCanvas();
	}

	

	// Start is called before the first frame update
	protected virtual void Start()
    {
		ShowMask();
	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void InitCamera()
	{
		MainCamera = GameObject.FindAnyObjectByType<Camera>().gameObject;
		VirtualCamera = Instantiate(Resources.Load<GameObject>("Prefab/VirtualCamera"));
		MainCamera.AddComponent<CinemachineBrain>();
	}

	private void InitCanvas()
	{
		MainCanvas = new GameObject("MainCanvas");
		MainCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		MainCanvas.AddComponent<CanvasScaler>();
		MainCanvas.AddComponent<GraphicRaycaster>();

		MainEventSystem = new GameObject("EvenSystem");
		MainEventSystem.AddComponent<EventSystem>();
		MainEventSystem.AddComponent<InputSystemUIInputModule>();
	}

	private void ShowMask()
	{
		GameObject mask = Instantiate(Resources.Load<GameObject>("Prefab/UI/CircleRevealMask"));
		mask.transform.SetParent(MainCanvas.transform, false);
		mask.GetComponent<RectTransform>().StretchToFillParent(null, null);
		Vector2 centerScreenPos = GetRevealCenter();
		mask.GetComponent<CircleRevealMask>().Expand(centerScreenPos);
	}

	protected virtual Vector2 GetRevealCenter()
	{
		return new Vector2(Screen.width / 2, Screen.height / 2);
	}

}
