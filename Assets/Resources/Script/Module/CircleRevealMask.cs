using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class CircleRevealMask : MonoBehaviour
{	
	enum Mode
	{
		None,
		Expand,
		Contract
	}
	private float duration = 1f;
	private float minRadius;
	private float maxRadius;
	private float currentTime = 0f;
	private Mode currentMode = Mode.None;

	

	// Start is called before the first frame update
	void Start()
    {
		Scale();
		minRadius = 0;
		maxRadius = MathF.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenWidth", Screen.width);
		GetComponent<SpriteRenderer>().material.SetFloat("_ScreenHeight", Screen.height);
		GetComponent<SpriteRenderer>().material.SetFloat("_Radius", 0);
    }

	private void Scale()
	{
		Camera mainCamera = SceneDirector.SceneInstance.MainCamera.GetComponent<Camera>();
		Vector3 bottomLeftWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
		Vector3 topRightWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		Vector2 spriteSize = GetComponent<SpriteRenderer>().sprite.rect.size;
		float screenWidth = (topRightWorldPos.x - bottomLeftWorldPos.x) * CommonConst.PixelPerUnit;
		float screenHeight = (topRightWorldPos.y - bottomLeftWorldPos.y) * CommonConst.PixelPerUnit;
		transform.localScale = new Vector2(screenWidth / spriteSize.x, screenHeight / spriteSize.y);
	}

	// Update is called once per frame
	void Update()
    {
		FollowCamera();
		UpdateRadius();
	}

	private void UpdateRadius()
	{
		if (currentMode != Mode.None)
		{
			if (currentTime >= duration)
			{
				Destroy(gameObject);
				return;
			}

			currentTime += Time.deltaTime;
			float startRadius = (currentMode == Mode.Expand) ? minRadius : maxRadius;
			float endRadius = (currentMode == Mode.Expand) ? maxRadius : minRadius;
			float currentRadius = Mathf.Lerp(startRadius, endRadius, currentTime / duration);
			GetComponent<SpriteRenderer>().material.SetFloat("_Radius", currentRadius);
		}
	}

	private void FollowCamera()
	{
		Vector3 cameraPos = SceneDirector.SceneInstance.MainCamera.transform.position;
		transform.position = new Vector3(cameraPos.x, cameraPos.y, 0);
	}

	public void Expand(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		currentMode = Mode.Expand;
	}

	public void Contract(Vector2 centerPos)
	{
		GetComponent<SpriteRenderer>().material.SetVector("_Center", centerPos);
		currentMode = Mode.Contract;
	}
}
