using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectorSceneBegin : SceneDirector
{
	private void OnEnable()
	{
		SwitchSceneAnimation.Instance.OnOpenAnimationFinish.AddListener(OnSceneOpenAnimationFinish);
	}

	private void OnDisable()
	{
		//SwitchSceneAnimation.Instance.OnOpenAnimationFinish.RemoveListener(OnSceneOpenAnimationFinish);
	}

	private void OnSceneOpenAnimationFinish()
	{
		UICommonFunc.CreatePanel("PanelBegin", null);
	}
}
