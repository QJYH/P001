using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTextureGenerator : MonoBehaviour
{
	GameObject shadowReceiver;

	void Start()
    {
		AddShadowCamera();
		AddShadowReceiver();

	}

	private void Update()
	{
		CommonFunc.FollowMainCamera(shadowReceiver);
	}

	private void AddShadowReceiver()
	{
		shadowReceiver = Instantiate(Resources.Load<GameObject>("Prefab/Module/ShadowReceiver"));
		CommonFunc.ScaleToFillScreen(shadowReceiver);
	}

	private void AddShadowCamera()
	{
		Instantiate(Resources.Load<GameObject>("Prefab/Module/Camera/ShadowCamera"));
	}
}
