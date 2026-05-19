using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class SceneDirector : MonoBehaviour
{
	protected GameObject mainCamera;
	protected GameObject mainCanvas;
	protected GameObject mainEventSystem;


	protected virtual void Awake()
	{
		mainCamera = GameObject.FindAnyObjectByType<CameraScript>().gameObject;
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

	private void InitCanvas()
	{
		mainCanvas = new GameObject("MainCanvas");
		mainCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		mainCanvas.AddComponent<CanvasScaler>();
		mainCanvas.AddComponent<GraphicRaycaster>();

		mainEventSystem = new GameObject("EvenSystem");
		mainEventSystem.AddComponent<EventSystem>();
		mainEventSystem.AddComponent<InputSystemUIInputModule>();
	}

	private void ShowMask()
	{
		GameObject mask = Instantiate(Resources.Load<GameObject>("Prefab/UI/CircleRevealMask"));
		mask.transform.SetParent(mainCanvas.transform, false);
		mask.GetComponent<RectTransform>().StretchToFillParent(null, null);
		Vector2 centerScreenPos = GetRevealCenter();
		mask.GetComponent<CircleRevealMask>().Expand(centerScreenPos);
	}

	protected virtual Vector2 GetRevealCenter()
	{
		return new Vector2(Screen.width / 2, Screen.height / 2);
	}
}
